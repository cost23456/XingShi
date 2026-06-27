using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ColorScene : MonoBehaviour
{
    private CanvasGroup mMyCanvas;
    private CanvasGroup mBlackCanvas;
    private Sequence mSeq;
    private void Awake()
    {
        this.mMyCanvas = GetComponent<CanvasGroup>();
        this.mBlackCanvas = this.transform.Find("Black").GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        Destroy(this.gameObject, 7f);
        this.Fade();
    }

    public void Fade()
    {
        this.mSeq = DOTween.Sequence();
        this.mSeq.Append(mBlackCanvas.DOFade(0,5f));
        this.mSeq.AppendInterval(2f);
        this.mSeq.Append(mMyCanvas.DOFade(0, 5f));
    }
}
