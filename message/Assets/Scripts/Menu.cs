using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("菜单设置")]
    public GameObject pauseMenuPanel;      // 暂停菜单面板
    public GameObject settingsPanel;       // 设置面板
    public bool isPaused = false;          // 是否暂停

    // 记录上一个打开的面板，用于返回
    private GameObject lastActivePanel;

    void Start()
    {
        // 确保游戏开始时菜单隐藏
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // 确保设置面板初始隐藏
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // 解锁鼠标（防止卡在锁定状态）
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 初始时，上一个面板为暂停菜单
        lastActivePanel = pauseMenuPanel;
    }

    void Update()
    {
        // 按 Esc 键切换暂停
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果设置面板打开，先关闭设置面板
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            TogglePause();
        }
    }

    /// <summary>
    /// 切换暂停状态
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        // 确保设置面板隐藏
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // 记录当前面板
        lastActivePanel = pauseMenuPanel;

        // 显示并解锁鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("游戏已暂停");
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // 确保设置面板也关闭
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Debug.Log("游戏已恢复");
    }

    /// <summary>
    /// 打开设置面板
    /// </summary>
    public void OpenSettings()
    {
        // 记录当前打开的面板（暂停菜单）
        lastActivePanel = pauseMenuPanel;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Debug.Log("打开设置界面");
    }

    /// <summary>
    /// 关闭设置面板，返回上一个菜单
    /// </summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // 返回上一个面板
        if (lastActivePanel != null)
        {
            lastActivePanel.SetActive(true);
            Debug.Log($"返回: {lastActivePanel.name}");
        }

        Debug.Log("关闭设置界面");
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("游戏重新开始");
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void GoToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1f;
        isPaused = false;
        ScenesManager.Instance.Loadscenes.SetActive(true);
        ScenesManager.Instance.LoadScene("3D场景");
        Debug.Log("返回主菜单");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("退出游戏");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}