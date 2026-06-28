using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColorScene : MonoBehaviour
{
    private CanvasGroup mMyCanvas;
    private CanvasGroup mColorfulCanvas;
    private CanvasGroup mBlackCanvas;
    private Sequence mSeq;
    private void Awake()
    {
        this.mMyCanvas = GetComponent<CanvasGroup>();
        this.mBlackCanvas = this.transform.Find("Black").GetComponent<CanvasGroup>();
        this.mColorfulCanvas = this.transform.Find("Colorful").GetComponent<CanvasGroup>();
        this.mMyCanvas.alpha = 0;
        this.mBlackCanvas.alpha = 1;
        this.mColorfulCanvas.alpha = 0;
    }
    private void OnEnable()
    {
        this.mSeq?.Kill(true);
        this.Fade();
    }
    public void Fade()
    {
        this.mSeq = DOTween.Sequence();
        Tween tempIn = this.mMyCanvas.DOFade(1f, 2f);
        tempIn.OnComplete(() =>
        {
            this.mColorfulCanvas.alpha = 1f;
        });
        this.mSeq.Append(tempIn);
        this.mSeq.Append(this.mBlackCanvas.DOFade(0, 5f));
        this.mSeq.AppendInterval(5f);
        this.mSeq.Append(this.mMyCanvas.DOFade(0, 2f));
        this.mSeq.OnComplete(() =>
        {
            Destroy(this.gameObject, 1f);
        });
    }
    private void OnDestroy()
    {
        this.mSeq?.Kill(true);
    }
}