using UnityEngine;
using UnityEngine.UI;

public class SimpleVolumeControl : MonoBehaviour
{
    [Header("音频源")]
    public AudioSource[] audioSources;       // 要控制的音频源
    public Slider volumeSlider;              // 音量滑块

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;

            // 加载保存的音量
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
            volumeSlider.value = savedVolume;

            // 添加监听
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            // 应用初始音量
            OnVolumeChanged(savedVolume);
        }
    }

    private void OnVolumeChanged(float value)
    {
        // 控制所有音频源
        foreach (AudioSource source in audioSources)
        {
            if (source != null)
                source.volume = value;
        }

        // 保存音量设置
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }
}