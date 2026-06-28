using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StartPmt : MonoBehaviour
{
    private CanvasGroup mMyCanvasGroup;
    private Sequence mSeq;
    private void Awake()
    {
        this.mMyCanvasGroup = GetComponent<CanvasGroup>();
        this.mSeq = DOTween.Sequence();
    }
    private void Start()
    {
        if (gameobjects.Instance._2DisSuccess) Destroy(gameObject);
        this.transform.localScale = Vector3.zero;
        this.mMyCanvasGroup.alpha = 0f;
        this.mSeq.Append(mMyCanvasGroup.DOFade(1f, 1f));
        this.mSeq.Join(mMyCanvasGroup.gameObject.transform.DOScale(new Vector3(2f, 2f, 2f), 1f));
        this.mSeq.AppendInterval(1f);
        this.mSeq.Append(mMyCanvasGroup.DOFade(0, 1f));
        this.mSeq.Join(mMyCanvasGroup.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 1f));
        Destroy(this.gameObject, 5f);
    }
}
