using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenLetterMain : MonoBehaviour
{
    public List<RectTransform> LettersTrans;
    public ReadLetter Read;
    public List<ColorScene> ColScene;
    private Sequence mSeq;
    private int SceneIndex = 0;
    private void Start()
    {
        this.mSeq = DOTween.Sequence();
    }
    private void OnEnable()
    {
        this.mSeq.Append(LettersTrans[0].DORotate(new Vector3(0,0,3f),0.75f));
        //this.mSeq.Join(LettersTrans[1].DORotate(new Vector3(0, 0, 0), 1f));
        //this.mSeq.Join(LettersTrans[2].DORotate(new Vector3(0, 0, 0), 1f));
        this.mSeq.Append(LettersTrans[1].DORotate(new Vector3(0, 0, -15.5f), 1f));
        //this.mSeq.Join(LettersTrans[2].DORotate(new Vector3(0, 0, -90), 1f));
        this.mSeq.Append(LettersTrans[2].DORotate(new Vector3(0, 0, -34f), 1.25f));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.Read.gameObject.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            this.ColScene[SceneIndex].gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void OpenReadPage(int aIndex)
    { 
        this.SceneIndex = aIndex; 
        this.Read.gameObject.SetActive(true);
        this.Read.StartAnima(aIndex);
        this.LettersTrans[aIndex].GetComponent<CanvasGroup>().alpha = 0.5f;
    }
}
