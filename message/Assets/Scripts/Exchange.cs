using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange : MonoBehaviour
{
    [Header("滑动设置")]
    public float stepDistance = 1900f;    // 每步距离
    public float yPosition = -800f;        // Y轴固定位置
    public float startX = -900f;           // 起始X位置（第1页）

    // 三个场景的位置（索引0=第1页，索引1=第2页，索引2=第3页）
    private float[] positions = new float[3];
    private int currentIndex = 0;          // 当前页面索引（初始为第1页）

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform 组件未找到！");
            return;
        }

        // 以 startX 为第3页，间隔1900，向左排列
        // 第3页在 -900（右边），第2页在中间，第1页在最左边
        positions[2] = startX;                      // 第3页：-900（右边）
        positions[1] = startX - stepDistance;       // 第2页：-2800（中间）
        positions[0] = startX - stepDistance * 2;   // 第1页：-4700（最左边）

        // 初始化到第1页（索引0）
        currentIndex = 0;
        rectTransform.anchoredPosition = new Vector2(positions[currentIndex], yPosition);
        Debug.Log($"当前页面: {currentIndex + 1}/3，位置: {positions[currentIndex]}");
    }

    public void NextChange()
    {
        if (rectTransform == null) return;

        // NextChange：图片向左移动（索引增加，位置减小）
        if (currentIndex < positions.Length - 1)
        {
            currentIndex++;
            rectTransform.anchoredPosition = new Vector2(positions[currentIndex], yPosition);
            Debug.Log($"切换到第 {currentIndex + 1} 页，位置: {positions[currentIndex]}");
        }
        else
        {
            Debug.Log("已经是最后一页了！");
        }
    }

    public void FrontChange()
    {
        if (rectTransform == null) return;

        // FrontChange：图片向右移动（索引减少，位置增大）
        if (currentIndex > 0)
        {
            currentIndex--;
            rectTransform.anchoredPosition = new Vector2(positions[currentIndex], yPosition);
            Debug.Log($"切换到第 {currentIndex + 1} 页，位置: {positions[currentIndex]}");
        }
        else
        {
            Debug.Log("已经是第一页了！");
        }
    }

    /// <summary>
    /// 跳转到指定页面（0=第1页，1=第2页，2=第3页）
    /// </summary>
    public void GoToPage(int pageIndex)
    {
        if (rectTransform == null) return;
        if (pageIndex < 0 || pageIndex >= positions.Length)
        {
            Debug.LogWarning($"页面索引 {pageIndex} 无效，有效范围: 0-{positions.Length - 1}");
            return;
        }

        currentIndex = pageIndex;
        rectTransform.anchoredPosition = new Vector2(positions[currentIndex], yPosition);
        Debug.Log($"跳转到第 {currentIndex + 1} 页");
    }

    /// <summary>
    /// 重置到第一页
    /// </summary>
    public void ResetToStart()
    {
        if (rectTransform == null) return;
        currentIndex = 0;
        rectTransform.anchoredPosition = new Vector2(positions[currentIndex], yPosition);
        Debug.Log($"重置到第一页，位置: {positions[currentIndex]}");
    }
}