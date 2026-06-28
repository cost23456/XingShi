using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameTitle : MonoBehaviour
{
    private CanvasGroup mMyCanvasGroup;
    private Sequence mSeq;
    private void Awake()
    {
        this.mMyCanvasGroup = GetComponent<CanvasGroup>();
        this.mSeq = DOTween.Sequence();
    }
    private void OnEnable()
    {
        this.mSeq.Append(mMyCanvasGroup.DOFade(1f,1f));
        this.mSeq.AppendInterval(5f);
        this.mSeq.Append(mMyCanvasGroup.DOFade(0, 5f));
        this.mSeq.Join(mMyCanvasGroup.gameObject.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 5f));
        Destroy(this.gameObject,15f);
    }
}
