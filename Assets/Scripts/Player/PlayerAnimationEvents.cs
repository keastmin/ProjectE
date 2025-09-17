using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Animator _animator;

    public bool CanNextComboAttack = false;
    public bool CanExitComboAttack = false;
    public bool ComboAttackFinish = false;

    private void Awake()
    {
        InitValue();
    }

    private void InitValue()
    {
        CanNextComboAttack = false;
        CanExitComboAttack = false;
        ComboAttackFinish = false;
    }

    public void NextAttackOn()
    {
        CanNextComboAttack = true;
    }

    public void NextAttackOff()
    {
        CanNextComboAttack = false;
    }

    public void EarlyExitComboAttack()
    {
        CanExitComboAttack = true;
    }

    public void ComboAttackFinishFlag()
    {
        ComboAttackFinish = true;
    }
}
