using UnityEngine;
using UnityEngine.UI;

public class HideRoomOnClick : MonoBehaviour
{
    [Header("承载病房图的RawImage物体(Hospital Scene NoColor)")]
    public RawImage roomRawImage;

    // 给Button OnClick面板调用的公共方法，无参数
    public void HideRoomPicture()
    {
        if (roomRawImage != null)
        {
            roomRawImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("请赋值roomRawImage，拖拽Hospital Scene NoColor物体！");
        }
    }

    // 可选：恢复显示
    public void ShowRoomPicture()
    {
        if (roomRawImage != null)
        {
            roomRawImage.gameObject.SetActive(true);
        }
    }
}