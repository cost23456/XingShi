using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class duihua : MonoBehaviour
{
    public Text dialogText;  // 对话文本组件
    public GameObject dialogPanel;  // 对话框面板
    public string dialogContent = "这是一段对话内容，按下空格键结束对话。";  // 对话内容
    public Image NPC1; 
    public Image NPC2;
    private bool isPlayerInRange = false;  // 玩家是否在碰撞范围内
    private bool isDialogShowing = false;  // 对话框是否显示中

    void Start()
    {
        NPC1.enabled = true;
        NPC2.enabled = false;
        // 确保对话框一开始是隐藏的
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);

        }
    }

    void Update()
    {
        // 当玩家在范围内且按下空格键时触发/关闭对话
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Mouse0))
        {
            NPC1.enabled = false;
            NPC2.enabled = true;
            dialogContent  ="XX通道收集到一条信使信号，请通行。";
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
            if (!isDialogShowing)
            {
                ShowDialog();
            }
            else
            {
                HideDialog();
            }
            }
        }
    }

    /// <summary>
    /// 当玩家进入碰撞区域时调用
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            ShowDialog();
            Debug.Log("玩家进入对话区域，按空格键开始对话");
        }
    }

    /// <summary>
    /// 当玩家离开碰撞区域时调用
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // 玩家离开时自动关闭对话框
            if (isDialogShowing)
            {
                HideDialog();
            }
        }
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    private void ShowDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
            isDialogShowing = true;
            
            // 设置对话文本
            if (dialogText != null)
            {
                dialogText.text = dialogContent;
            }
            
            Debug.Log("对话框已显示");
        }
    }

    /// <summary>
    /// 隐藏对话框
    /// </summary>
    private void HideDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
            isDialogShowing = false;
            Debug.Log("对话框已关闭");
        }
    }
}