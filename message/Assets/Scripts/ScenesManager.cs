using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class ScenesManager : Singleton<ScenesManager>
{
    public List<GameObject> DontDesObj;
    public Slider progressBar;
    public Text progressText;
    public float loadSpeed = 0.5f;
    public GameObject Loadscenes;

    private void Start()
    {
        foreach (GameObject Obj in DontDesObj)
        {
            if (Obj != null)
            {
                DontDestroyOnLoad(Obj);
            }
        }
    }
    // 异步加载场景
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 开始异步加载
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false; // 禁止自动激活
        float fakeProgress = 0f;
        if(progressBar==null || progressText == null)
        {
            yield return  null;
        }
        // 循环更新进度
        while (!op.isDone)
        {
            // 重点：让进度条慢慢涨，而不是瞬间加载完
            if (fakeProgress < 0.9f)
            {
                fakeProgress += Time.deltaTime * loadSpeed;
            }
            // 进度条更新（Unity最后10%为激活阶段）
            float progress = Mathf.Clamp01(fakeProgress / 0.9f);
            progressBar.value = progress;
            progressText.text = (progress * 100).ToString("0") + "%";

            // 加载完成（进度到0.9）
            if (progress >= 0.9f)
            {
                progressText.text = "按任意键继续...";
                if (Input.anyKeyDown)
                {
                    op.allowSceneActivation = true; // 激活场景
                    Loadscenes.SetActive(false);//把加载场景退出
                }
            }
            yield return null;
        }
    }
}