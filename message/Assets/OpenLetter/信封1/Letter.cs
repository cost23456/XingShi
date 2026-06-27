using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Letter : MonoBehaviour
{
    private RectTransform mIconTrans;
    private Sequence mSeq;
    private void Awake()
    {
        this.mSeq = DOTween.Sequence();
        this.mIconTrans = this.transform.Find("Icon").gameObject.GetComponent<RectTransform>();
    }
    public void PointerEnter()
    {
        this.mSeq?.Kill(true);
        this.mSeq.Append(mIconTrans.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.3f));
    }

    public void PointerExit()
    {
        this.mSeq?.Kill(true);
        this.mSeq.Append(mIconTrans.DOScale(new Vector3(1f, 1f, 1f), 0.3f));
    }
}
