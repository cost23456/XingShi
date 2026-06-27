using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
   private CinemachineVirtualCamera cinemachinVirtualCamera;
   private CinemachineBasicMultiChannelPerlin cinemachin;
   public float shaketime = 1f;
   public float shaketimetotal = 4f;
   public float shakeMagnitude = 5f;
   private bool hasTriggered = false; 
    void Awake()
    {
        cinemachinVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachin = cinemachinVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    void Update()
    {
        if (shaketime > 0)
        {
            shaketime -= Time.deltaTime;
            cinemachin.m_AmplitudeGain = Mathf.Lerp(0,shakeMagnitude, shaketime / shaketimetotal);
        }
        if (!hasTriggered)
        {
            hasTriggered = true;
            TriggerShake();
        }

    }
    private void TriggerShake()
    {
        shaketime = shaketimetotal;
        cinemachin.m_AmplitudeGain = shakeMagnitude;
        
    }
}
