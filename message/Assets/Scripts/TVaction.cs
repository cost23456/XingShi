using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TVaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("透明度设置")]
    public Image targetImage;
    public float normalAlpha = 0f;        // 默认透明度（0 = 完全透明）
    public float highlightAlpha = 0.2f;   // 悬停透明度（20%）

    [Header("对话设置")]
    public GameObject dialogCanvas;       // 对话画布

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        // 初始化透明度为0
        SetAlpha(normalAlpha);

        // 确保对话画布初始隐藏
        if (dialogCanvas != null)
            dialogCanvas.SetActive(false);
    }

    // 鼠标悬停 - 透明度变化
    #region 透明度控制
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(highlightAlpha);
        Debug.Log("鼠标悬停 - 透明度: 20%");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(normalAlpha);
        Debug.Log("鼠标离开 - 透明度: 0%");
    }

    private void SetAlpha(float alpha)
    {
        if (targetImage == null) return;

        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
    #endregion

    // 鼠标点击 - 显示对话画布
    #region 点击控制
    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogCanvas != null)
        {
            // 切换显示/隐藏（点击一次显示，再点击隐藏）
            bool isActive = dialogCanvas.activeSelf;
            dialogCanvas.SetActive(!isActive);
            Debug.Log($"对话画布: {(isActive ? "隐藏" : "显示")}");
        }
        else
        {
            Debug.LogWarning("对话画布未赋值！");
        }
    }
    #endregion
}