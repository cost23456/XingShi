using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 引入UI命名空间，使用Image组件
using Cinemachine; 

public class Camerashake : MonoBehaviour
{
    public GameObject Player;
    public CinemachineVirtualCamera cinemachinVirtualCamera;
    public float shakeDuration = 2f;  // 抖动持续时间
    public Image displayImage;  // 你的UI图片组件，用于显示图片
    public Image honguang;
    public Text jg1;
    public Text jg2;

    private CameraShaker cameraShaker;
    private bool isShaking = false;

    void Start()
    {
        // 获取 CameraShaker 组件引用
        if (cinemachinVirtualCamera != null)
        {
            cameraShaker = cinemachinVirtualCamera.GetComponent<CameraShaker>();
        }

        // 确保图片一开始是隐藏的
        if (displayImage != null&&honguang!=null&&jg1!=null&&jg2!=null)
        {
            displayImage.enabled = false;
            honguang.enabled = false;
            jg1.enabled = false;
            jg2.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isShaking)
        {
            Debug.Log("触发了碰撞");
            
            // 启动图片显示协程
            StartCoroutine(DisplayImage());
            
            // 启动屏幕震动协程
            StartCoroutine(StartShake());
        }
    }

    /// <summary>
    /// 启动抖动协程
    /// 开启 CameraShaker，等待指定时间后关闭
    /// </summary>
    IEnumerator StartShake()
    {
        isShaking = true;
        
        // 检查 CameraShaker 是否有效
        if (cameraShaker == null)
        {
            Debug.LogWarning("Camerashake: CameraShaker 组件未找到！");
            isShaking = false;
            yield break;
        }
        else
        {
            Debug.Log("CameraShaker 已存在，无需创建");
        }
        
        // 开启抖动
        cameraShaker.enabled = true;
        Debug.Log("CameraShaker 已开启");
        
        // 等待指定的抖动持续时间
        yield return new WaitForSeconds(shakeDuration);
        
        // 关闭抖动
        cameraShaker.enabled = false;
        isShaking = false;
        Debug.Log("CameraShaker 已关闭");
    }

    /// <summary>
    /// 启动图片显示协程
    /// 显示 0.5 秒，隐藏 0.5 秒，持续 4 秒
    /// </summary>
    IEnumerator DisplayImage()
    {
        float displayTime = 0f;
        
        while (displayTime < 4f)
        {
            // 显示图片
            if (displayImage != null&&honguang!=null&&jg1!=null&&jg2!=null)
            {
                displayImage.enabled = true;
                honguang.enabled = true;
                jg1.enabled = true;
                jg2.enabled = true;
                
                           }
            Debug.Log("图片显示");

            // 等待 0.5 秒
            yield return new WaitForSeconds(0.5f);

            // 隐藏图片
            if (displayImage != null&&honguang!=null&&jg1!=null&&jg2!=null)
            {
                displayImage.enabled = false;
                honguang.enabled = false;
                jg1.enabled = false;
                jg2.enabled = false;
                
            }
            Debug.Log("图片隐藏");

            // 等待 0.5 秒
            yield return new WaitForSeconds(0.5f);

            // 累加时间
            displayTime += 1f; // 0.5 秒显示 + 0.5 秒隐藏共计 1 秒
        }
    }
}