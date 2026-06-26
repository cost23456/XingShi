using UnityEngine;
using DG.Tweening;

public class DT : MonoBehaviour
{
    [Header("旋转一圈耗时（秒），数值越小转越快")]
    public float rotateDuration = 3f;
    private Tweener rotateTween;

    void Start()
    {
        // DOLocalRotate：局部旋转，UI用Z轴旋转
        // Vector3(0,0,360)：2D平面顺时针转一圈
        // RotateMode.FastBeyond360：支持超过360度连续旋转，不会卡顿回弹
        rotateTween = transform.DOLocalRotate(
            new Vector3(0, 0, 360),
            rotateDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(Ease.Linear) // Linear 匀速旋转，无加速减速
        .SetLoops(-1, LoopType.Restart); // -1 = 无限循环；Restart 转完一圈立刻再来
       
        
    }

   
}