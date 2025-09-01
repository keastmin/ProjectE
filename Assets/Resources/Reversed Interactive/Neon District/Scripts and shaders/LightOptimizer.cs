using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightOptimizer : MonoBehaviour
{

    public Transform m_MainCamera;
    public float lightRange;
    public float fadeDistance;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main.transform;
        lightRange = GetComponent<HDAdditionalLightData>().range * 0.8f;

        fadeDistance = GetComponent<HDAdditionalLightData>().shadowFadeDistance;
    }

    // Update is called once per frame
    void Update()
    {
        

        float dist = Vector3.Distance(m_MainCamera.position, transform.position);
            if (dist < fadeDistance)
            {
                GetComponent<HDAdditionalLightData>().EnableShadows(true);
            }
            else
            {
                GetComponent<HDAdditionalLightData>().EnableShadows(false);
            }

        if (dist < fadeDistance * 0.5f)
        {
            GetComponent<HDAdditionalLightData>().SetShadowResolution(128);
        }
        else if (dist < fadeDistance * 0.75f)
        {
            GetComponent<HDAdditionalLightData>().SetShadowResolution(80);
        }
        else if (dist < fadeDistance)
        {
            GetComponent<HDAdditionalLightData>().SetShadowResolution(40);
        }


    }
}
