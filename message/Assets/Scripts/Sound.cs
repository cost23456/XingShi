using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [Header("音效设置")]
    public AudioClip clickSound;           // 点击音效
    public float volume = 0.7f;            // 音量

    private AudioSource audioSource;

    void Start()
    {
        // 获取或创建 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        // 自动绑定按钮点击事件
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlayClickSound);
            Debug.Log("按钮音效已绑定");
        }
        else
        {
            Debug.LogWarning("此物体没有 Button 组件！请将脚本挂载到按钮上，或手动调用 PlayClickSound()");
        }

        // 检查音效是否赋值
        if (clickSound == null)
        {
            Debug.LogWarning("clickSound 未赋值！请在 Inspector 中拖入音效文件");
        }
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
            Debug.Log("点击音效已播放");
        }
        else
        {
            Debug.LogWarning($"音效未播放 - audioSource: {audioSource != null}, clickSound: {clickSound != null}");
        }
    }
}