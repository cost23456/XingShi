using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class ImageAnimationController : MonoBehaviour
{
    public CanvasGroup BgCvsGroup;
    public GameObject Promote;
    public Button closeButton;

    [Header("主角控制")]
    public MonoBehaviour playerController;    // 主角控制脚本
    public MonoBehaviour playerInput;          // 主角输入脚本（可选）

    private Sequence mSeq;
    private bool isOpen = false;

    private void Awake()
    {
        this.mSeq = DOTween.Sequence();
    }

    private void Start()
    {
        // 绑定关闭按钮
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        // 开场动画
        this.BgCvsGroup.alpha = 0;
        this.mSeq.Append(BgCvsGroup.DOFade(1, 0.8f));
        this.Promote.transform.localScale = Vector3.zero;
        this.mSeq.Join(Promote.transform.DOScale(Vector3.one, 1.0f));

        // 打开界面时禁用主角
        DisablePlayer();
        isOpen = true;
    }

    private void DisablePlayer()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (playerInput != null)
            playerInput.enabled = false;

        // 可选：解锁鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("主角已禁用");
    }


    private void EnablePlayer()
    {
        if (playerController != null)
            playerController.enabled = true;

        if (playerInput != null)
            playerInput.enabled = true;

        // 可选：锁定鼠标（游戏模式）
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("主角已启用");
    }


    public void ClosePanel()
    {
        Sequence closeSeq = DOTween.Sequence();

        closeSeq.Join(Promote.transform.DOScale(Vector3.zero, 0.5f));
        closeSeq.Join(BgCvsGroup.DOFade(0, 0.5f));

        closeSeq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            // 关闭界面时启用主角
            EnablePlayer();
        });

        Debug.Log("关闭面板");
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
    }
}