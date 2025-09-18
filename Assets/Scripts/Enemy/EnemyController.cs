using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("Path")]
    [SerializeField] private float _repathInterval = 0.25f;
    [SerializeField] private float _sampleRadius = 1.5f;
    [SerializeField] private int _areaMask = NavMesh.AllAreas;
    [SerializeField] private float _cornerReachDist = 0.25f;     // 코너 도달 판단
    [SerializeField] private float _arrivalDist = 0.6f;          // 최종 도착 판단

    [Header("Movement (Rigidbody)")]
    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField] private float _accel = 20f;                 // 수평 가속
    [SerializeField] private float _rotationSpeedDeg = 720f;     // 초당 회전각
    [SerializeField] private bool _freezeXZRotation = true;      // 흔한 캐릭터 설정

    [Header("Gizmo")]
    [SerializeField] private bool _drawOnlyWhenSelected = false;
    [SerializeField] private float _cornerSphereRadius = 0.12f;
    [SerializeField] private Color _completeColor = Color.green;
    [SerializeField] private Color _partialColor = Color.yellow;
    [SerializeField] private Color _invalidColor = Color.red;
    [SerializeField] private bool _labelCornerIndex = true;

    // 내부 상태
    private Rigidbody _rb;
    private NavMeshPath _path;
    private float _repathTimer;
    private int _cornerIndex;
    private bool _suspended; // 외력 동안 true

    private void OnValidate()
    {
        if (_path == null) _path = new NavMeshPath();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_freezeXZRotation)
            _rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        if (_path == null) _path = new NavMeshPath();
    }

    private void OnEnable()
    {
        _repathTimer = 0f;
        _cornerIndex = 0;
        RecalculatePath();
    }

    private void Update()
    {
        // 에디터 정지 상태에서도 경로 갱신해서 Gizmo 확인 가능
        float dt = Application.isPlaying ? Time.deltaTime : 0.016f;
        _repathTimer -= dt;
        if (_repathTimer <= 0f)
        {
            RecalculatePath();
            _repathTimer = Mathf.Max(0.01f, _repathInterval);
        }
    }

    private void FixedUpdate()
    {
        if (_suspended) return;

        if (_path == null || _path.corners == null || _path.corners.Length == 0) return;
        if (_path.status == NavMeshPathStatus.PathInvalid) return;

        // 마지막 코너까지 추종
        Vector3 pos = transform.position;

        // 최종 목적지 도달 체크
        Vector3 final = _path.corners[_path.corners.Length - 1];
        Vector3 toFinalFlat = final - pos; toFinalFlat.y = 0f;
        if (toFinalFlat.sqrMagnitude <= _arrivalDist * _arrivalDist)
        {
            // 부드럽게 정지(수평 속도 감쇠)
            Vector3 v = _rb.linearVelocity;
            Vector3 horiz1 = new Vector3(v.x, 0f, v.z);
            horiz1 = Vector3.MoveTowards(horiz1, Vector3.zero, _accel * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector3(horiz1.x, v.y, horiz1.z);
            return;
        }

        // 현재 목표 코너
        _cornerIndex = Mathf.Clamp(_cornerIndex, 0, _path.corners.Length - 1);
        Vector3 corner = _path.corners[_cornerIndex];
        Vector3 toCorner = corner - pos; toCorner.y = 0f;

        // 코너 도달 시 다음 코너로
        if (toCorner.sqrMagnitude <= _cornerReachDist * _cornerReachDist)
        {
            if (_cornerIndex < _path.corners.Length - 1) _cornerIndex++;
            corner = _path.corners[_cornerIndex];
            toCorner = corner - pos; toCorner.y = 0f;
        }

        // 원하는 수평 속도
        Vector3 desired = toCorner.sqrMagnitude > 0.0001f
            ? toCorner.normalized * _moveSpeed
            : Vector3.zero;

        // 수평 가속, 수직 속도는 보존(점프/에어본 유지)
        Vector3 vel = _rb.linearVelocity;
        Vector3 horiz = new Vector3(vel.x, 0f, vel.z);
        horiz = Vector3.MoveTowards(horiz, new Vector3(desired.x, 0f, desired.z), _accel * Time.fixedDeltaTime);
        _rb.linearVelocity = new Vector3(horiz.x, vel.y, horiz.z);

        // 진행 방향으로 회전
        if (horiz.sqrMagnitude > 0.01f)
        {
            Quaternion r = Quaternion.LookRotation(new Vector3(horiz.x, 0f, horiz.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, r, _rotationSpeedDeg * Time.fixedDeltaTime);
        }
    }

    // === 경로계산 ===
    public bool RecalculatePath()
    {
        if (_target == null) return false;

        bool okStart = NavMesh.SamplePosition(transform.position, out var startHit, _sampleRadius, _areaMask);
        bool okEnd = NavMesh.SamplePosition(_target.position, out var endHit, _sampleRadius, _areaMask);
        if (!okStart || !okEnd) { _path.ClearCorners(); return false; }

        bool ok = NavMesh.CalculatePath(startHit.position, endHit.position, _areaMask, _path);
        if (ok)
        {
            // PathComplete/Partial 모두 추종 가능(Partial이면 가능한 지점까지만)
            _cornerIndex = 0;
        }
        return ok;
    }

    // === 외력(넉백/에어본) 중 AI 제어 중단/재개 ===
    public void SuspendFor(float seconds)
    {
        if (!gameObject.activeInHierarchy) return;
        StopAllCoroutines();
        StartCoroutine(CoSuspend(seconds));
    }
    public void ApplyKnockback(Vector3 velocity, float suspendSeconds = 0.25f)
    {
        SuspendFor(suspendSeconds);
        // 수평은 덮어쓰기, 수직은 더 큰값을 유지
        var v = _rb.linearVelocity;
        _rb.linearVelocity = new Vector3(velocity.x, Mathf.Max(v.y, velocity.y), velocity.z);
    }
    System.Collections.IEnumerator CoSuspend(float t)
    {
        _suspended = true;
        yield return new WaitForSeconds(t);
        _suspended = false;
        RecalculatePath(); // 재개 시 경로 최신화
    }

    // === Gizmos (경로 시각화) ===
    private void OnDrawGizmos()
    {
        if (_drawOnlyWhenSelected) return;
        DrawPathGizmos();
    }
    private void OnDrawGizmosSelected()
    {
        if (!_drawOnlyWhenSelected) return;
        DrawPathGizmos();
    }
    private void DrawPathGizmos()
    {
        if (_path == null || _path.corners == null || _path.corners.Length == 0) return;

        Color c = _invalidColor;
        if (_path.status == NavMeshPathStatus.PathComplete) c = _completeColor;
        else if (_path.status == NavMeshPathStatus.PathPartial) c = _partialColor;

#if UNITY_EDITOR
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.color = c;
        Handles.DrawAAPolyLine(3f, _path.corners);
#endif
        Gizmos.color = c;
        for (int i = 0; i < _path.corners.Length - 1; i++)
            Gizmos.DrawLine(_path.corners[i], _path.corners[i + 1]);

        foreach (var p in _path.corners)
            Gizmos.DrawSphere(p + Vector3.up * 0.02f, _cornerSphereRadius);

#if UNITY_EDITOR
        if (_labelCornerIndex)
        {
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = c;
            for (int i = 0; i < _path.corners.Length; i++)
                Handles.Label(_path.corners[i] + Vector3.up * 0.1f, $"[{i}]", style);
        }
#endif
    }

    // 외부에서 코너 배열을 읽고 싶을 때
    public Vector3[] GetCorners() => _path?.corners;
    public NavMeshPathStatus PathStatus => _path != null ? _path.status : NavMeshPathStatus.PathInvalid;
}
