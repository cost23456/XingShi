using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReadLetter : MonoBehaviour
{
    public List<Sprite> Anima;
    [TextArea(3, 10)]
    public List<string> Context;

    private Image mImg;
    private GameObject LetterText;
    private CanvasGroup mCanvasGroup;
    private Sequence mSequence;
    public Text mText;
    private Coroutine _currentLetterCoroutine;

    private void Awake()
    {
        mImg = GetComponent<Image>();
        LetterText = transform.Find("LetterCOntext").gameObject;
        mCanvasGroup = LetterText.GetComponent<CanvasGroup>();

        // 初始化文字默认隐藏缩小
        mCanvasGroup.alpha = 0;
        LetterText.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void StartAnima(int aIndex)
    {
        // 防越界
        if (aIndex < 0 || aIndex >= Context.Count)
            return;
        if (Anima == null || Anima.Count == 0)
            return;

        // 停止正在运行的旧协程
        if (_currentLetterCoroutine != null)
        {
            StopCoroutine(_currentLetterCoroutine);
            _currentLetterCoroutine = null;
        }

        // 杀死上一轮文字动画
        if (mSequence != null && mSequence.IsActive())
        {
            mSequence.Kill();
        }

        // 重置文字到初始隐藏状态
        mCanvasGroup.alpha = 0;
        LetterText.GetComponent<RectTransform>().localScale = Vector3.zero;

        // 启动新协程并保存实例
        _currentLetterCoroutine = StartCoroutine(ReadLetters(aIndex));
    }

    IEnumerator ReadLetters(int aIndex)
    {
        // 帧动画从头播放
        mImg.sprite = Anima[0];
        foreach (Sprite sp in Anima)
        {
            mImg.sprite = sp;
            yield return new WaitForSeconds(0.2f);
        }

        // 创建新动画序列
        mSequence = DOTween.Sequence();
        mSequence.Append(mCanvasGroup.DOFade(1f, 1f));
        mSequence.Join(LetterText.GetComponent<RectTransform>().DOScale(Vector3.one, 1f));

        // 设置文本
        mText.text = Context[aIndex];

        // 等待文字动画播放完毕
        yield return mSequence.WaitForCompletion();

        // 动画全部结束，清空协程标记
        _currentLetterCoroutine = null;
    }

    private void OnDisable()
    {
        // 停止协程
        if (_currentLetterCoroutine != null)
        {
            StopCoroutine(_currentLetterCoroutine);
            _currentLetterCoroutine = null;
        }

        // 销毁动画
        if (mSequence != null)
        {
            mSequence.Kill();
        }
    }
}