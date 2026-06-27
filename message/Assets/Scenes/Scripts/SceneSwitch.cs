using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // 存储所有场景序号，按构建列表顺序填写
    private int[] sceneIndex = { 0, 1 };
    private int currentSceneId;

    void Start()
    {
        // 获取当前所在场景序号
        currentSceneId = SceneManager.GetActiveScene().buildIndex;
    }

    // 切换下一场景按钮调用
    public void NextScene()
    {
        // 当前是最后一个就切回第一个，循环切换
        if (currentSceneId >= sceneIndex.Length - 1)
        {
            SceneManager.LoadScene(sceneIndex[0]);
        }
        else
        {
            SceneManager.LoadScene(currentSceneId + 1);
        }
    }

    // 切换上一场景按钮调用
    public void PrevScene()
    {
        // 当前是第一个就切到最后一个，循环切换
        if (currentSceneId <= 0)
        {
            SceneManager.LoadScene(sceneIndex[sceneIndex.Length - 1]);
        }
        else
        {
            SceneManager.LoadScene(currentSceneId - 1);
        }
    }
}