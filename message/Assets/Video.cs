using UnityEngine;
using UnityEngine.Video;

public class VideoAutoClose : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoUI; // 填你的RawImage/整个视频UI父物体

    void Start()
    {
        // 注册视频播放完成事件
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // 视频播放完毕自动执行
    void OnVideoEnd(VideoPlayer vp)
    {
        // 关闭视频UI界面
        videoUI.SetActive(false);

        // 可选：播放完自动跳转场景
        // ScenesManager.Instance.LoadScene("MainGame");
    }

    void OnDestroy()
    {
        // 移除监听，防止内存泄漏
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}