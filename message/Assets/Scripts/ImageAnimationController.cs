using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageAnimationController : MonoBehaviour
{
    public CanvasGroup BgCvsGroup;
    public GameObject Promote;
    public Button closeButton;
    public Button nextButton;

    [Header("主角控制")]
    public MonoBehaviour playerController;
    public MonoBehaviour playerInput;

    [Header("主对话")]
    public TextAsset textFile;
    public Text textLabel;

    [Header("第二个弹窗对话")]
    public TextAsset secondTextFile;
    public GameObject secondDialogPanel;
    public Text secondTextLabel;

    [Header("【新增】第三个结尾对话面板")]
    public TextAsset thirdTextFile;
    public GameObject thirdDialogPanel;
    public Text thirdTextLabel;

    [Header("打字效果设置")]
    public float typingSpeed = 0.5f;

    private Sequence mSeq;
    private bool isOpen = false;
    private List<string> dialogueList = new List<string>();
    private List<string> secondDialogueList = new List<string>();
    // 第三对话数据
    private List<string> thirdDialogueList = new List<string>();

    private int currentIndex = 0;
    private int secondCurrentIndex = 0;
    private int thirdCurrentIndex = 0;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isDialogueFinished = false;
    private bool isSecondDialogueActive = false;
    private bool isSecondDialogueCompleted = false;

    // 第三对话状态标记
    private bool isThirdDialogueActive = false;
    private bool isThirdDialogueCompleted = false;

    private void Awake()
    {
        this.mSeq = DOTween.Sequence();
        LoadDialogueFromFile(textFile);
        LoadSecondDialogueFromFile(secondTextFile);
        // 加载第三份文本
        LoadThirdDialogueFromFile(thirdTextFile);
    }

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);

        if (secondDialogPanel != null)
            secondDialogPanel.SetActive(false);
        // 初始化隐藏第三面板
        if (thirdDialogPanel != null)
            thirdDialogPanel.SetActive(false);

        this.BgCvsGroup.alpha = 0;
        this.mSeq.Append(BgCvsGroup.DOFade(1, 0.8f));
        this.Promote.transform.localScale = Vector3.zero;
        this.mSeq.Join(Promote.transform.DOScale(Vector3.one, 1.0f));

        this.mSeq.OnComplete(() =>
        {
            StartDialogue();
        });

       // DisablePlayer();
        isOpen = true;
    }

    private void Update()
    {
        if (isOpen && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)))
        {
            NextLine();
        }
    }

    #region 文本加载
    private void LoadDialogueFromFile(TextAsset file)
    {
        dialogueList.Clear();
        if (file == null)
        {
            Debug.LogError("请拖入主对话文本文件！");
            return;
        }
        string[] paragraphs = file.text.Split(new char[] { '@' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string paragraph in paragraphs)
        {
            string trimmed = paragraph.Trim();
            if (!string.IsNullOrWhiteSpace(trimmed))
                dialogueList.Add(trimmed);
        }
        Debug.Log($"加载主对话成功，共 {dialogueList.Count} 段");
    }

    private void LoadSecondDialogueFromFile(TextAsset file)
    {
        secondDialogueList.Clear();
        if (file == null)
        {
            Debug.LogWarning("第二个文本文件为空！");
            return;
        }
        string[] paragraphs = file.text.Split(new char[] { '@' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string paragraph in paragraphs)
        {
            string trimmed = paragraph.Trim();
            if (!string.IsNullOrWhiteSpace(trimmed))
                secondDialogueList.Add(trimmed);
        }
        Debug.Log($"加载第二个对话成功，共 {secondDialogueList.Count} 段");
    }

    /// <summary>新增：加载第三段对话文本</summary>
    private void LoadThirdDialogueFromFile(TextAsset file)
    {
        thirdDialogueList.Clear();
        if (file == null)
        {
            Debug.LogWarning("第三个文本文件为空！");
            return;
        }
        string[] paragraphs = file.text.Split(new char[] { '@' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string paragraph in paragraphs)
        {
            string trimmed = paragraph.Trim();
            if (!string.IsNullOrWhiteSpace(trimmed))
                thirdDialogueList.Add(trimmed);
        }
        Debug.Log($"加载第三个结尾对话成功，共 {thirdDialogueList.Count} 段");
    }
    #endregion

    private void StartDialogue()
    {
        if (dialogueList.Count == 0)
        {
            Debug.LogWarning("主对话列表为空！");
            return;
        }
        currentIndex = 0;
        isDialogueFinished = false;
        isSecondDialogueActive = false;
        isSecondDialogueCompleted = false;
        // 重置第三对话标记
        isThirdDialogueActive = false;
        isThirdDialogueCompleted = false;

        ShowParagraph(currentIndex);
        Debug.Log("开始主对话");
    }

    public void NextLine()
    {
        if (!isOpen) return;

        // 优先级1：当前正在第三对话
        if (isThirdDialogueActive)
        {
            NextThirdLine();
            return;
        }

        // 优先级2：当前正在第二对话
        if (isSecondDialogueActive)
        {
            NextSecondLine();
            return;
        }

        // 优先级3：第二对话完成，回到主对话
        if (isSecondDialogueCompleted)
        {
            ContinueMainDialogue();
            return;
        }

        if (dialogueList.Count == 0) return;
        if (isDialogueFinished) return;

        // 快速跳过打字
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            textLabel.text = dialogueList[currentIndex];
            isTyping = false;
            return;
        }

        currentIndex++;

        // 主对话读到第4段（索引3）弹出第二弹窗
        if (currentIndex == 3 && secondDialogueList.Count > 0)
        {
            StartSecondDialogue();
            return;
        }

        // 主对话未读完，继续显示
        if (currentIndex < dialogueList.Count)
        {
            ShowParagraph(currentIndex);
        }
        else
        {
            // ========== 主对话全部读完，开启第三段结尾对话 ==========
            isDialogueFinished = true;
            Debug.Log("全部主对话结束，开启第三结尾对话");
            StartThirdDialogue();
        }
    }

    #region 第二对话逻辑
    private void ContinueMainDialogue()
    {
        isSecondDialogueCompleted = false;
        isDialogueFinished = false;

        if (secondDialogPanel != null)
            secondDialogPanel.SetActive(false);

        textLabel.gameObject.SetActive(true);

        currentIndex++;
        if (currentIndex < dialogueList.Count)
        {
            ShowParagraph(currentIndex);
        }
        else
        {
            isDialogueFinished = true;
            Debug.Log("全部主对话结束，开启第三结尾对话");
            StartThirdDialogue();
        }
    }

    private void StartSecondDialogue()
    {
        isSecondDialogueActive = true;
        secondCurrentIndex = 0;
        isSecondDialogueCompleted = false;

        if (secondDialogPanel != null)
            secondDialogPanel.SetActive(true);

        if (secondTextLabel == null && secondDialogPanel != null)
            secondTextLabel = secondDialogPanel.GetComponentInChildren<Text>();

        ShowSecondParagraph(secondCurrentIndex);
        Debug.Log("开始第二个弹窗对话");
    }

    private void NextSecondLine()
    {
        if (secondDialogueList.Count == 0) return;

        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            if (secondTextLabel != null)
                secondTextLabel.text = secondDialogueList[secondCurrentIndex];
            isTyping = false;
            return;
        }

        secondCurrentIndex++;

        if (secondCurrentIndex < secondDialogueList.Count)
        {
            ShowSecondParagraph(secondCurrentIndex);
        }
        else
        {
            isSecondDialogueActive = false;
            isSecondDialogueCompleted = true;
            Debug.Log("第二个对话结束，点击继续主对话");
        }
    }
    #endregion

    #region 【新增】第三结尾对话完整逻辑
    private void StartThirdDialogue()
    {
        if (thirdDialogueList.Count == 0)
        {
            Debug.LogWarning("第三对话文本为空，直接结束对话");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        isThirdDialogueActive = true;
        isThirdDialogueCompleted = false;
        thirdCurrentIndex = 0;

        // 已删除隐藏主面板、第二面板的代码，主文本会保留显示
        // if (secondDialogPanel != null)
        //     secondDialogPanel.SetActive(false);
        // textLabel.gameObject.SetActive(false);

        // 只显示第三面板，原有UI全部保留
        if (thirdDialogPanel != null)
            thirdDialogPanel.SetActive(true);

        // 自动获取文本组件
        if (thirdTextLabel == null && thirdDialogPanel != null)
            thirdTextLabel = thirdDialogPanel.GetComponentInChildren<Text>();

        ShowThirdParagraph(thirdCurrentIndex);
        Debug.Log("开启第三段结尾对话面板，主面板文本保留");
    }

    private void NextThirdLine()
    {
        if (thirdDialogueList.Count == 0) return;

        // 快速显示全部文字
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            if (thirdTextLabel != null)
                thirdTextLabel.text = thirdDialogueList[thirdCurrentIndex];
            isTyping = false;
            return;
        }

        thirdCurrentIndex++;

        if (thirdCurrentIndex < thirdDialogueList.Count)
        {
            ShowThirdParagraph(thirdCurrentIndex);
        }
        else
        {
            // 第三对话全部读完，标记完成
            isThirdDialogueActive = false;
            isThirdDialogueCompleted = true;
            Debug.Log("所有对话全部结束，可关闭面板");
        }
    }
    #endregion

    #region 打字显示
    private void ShowParagraph(int index)
    {
        if (index >= dialogueList.Count) return;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeParagraph(dialogueList[index], textLabel));
    }

    private void ShowSecondParagraph(int index)
    {
        if (index >= secondDialogueList.Count) return;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        if (secondTextLabel != null)
            typingCoroutine = StartCoroutine(TypeParagraph(secondDialogueList[index], secondTextLabel));
    }

    /// <summary>第三段文本显示</summary>
    private void ShowThirdParagraph(int index)
    {
        if (index >= thirdDialogueList.Count) return;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        if (thirdTextLabel != null)
            typingCoroutine = StartCoroutine(TypeParagraph(thirdDialogueList[index], thirdTextLabel));
    }

    private IEnumerator TypeParagraph(string text, Text targetText)
    {
        isTyping = true;
        targetText.text = "";

        foreach (char c in text)
        {
            targetText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    #endregion

    #region 玩家开关与关闭面板
    //private void DisablePlayer()
    //{
    //    if (playerController != null)
    //        playerController.enabled = false;

    //    if (playerInput != null)
    //        playerInput.enabled = false;

    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;

    //    Debug.Log("主角已禁用");
    //}

    //private void EnablePlayer(bool lockCursor = true)
    //{
    //    if (playerController != null)
    //        playerController.enabled = true;

    //    if (playerInput != null)
    //        playerInput.enabled = true;

    //    if (lockCursor)
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //        Debug.Log("主角已启用，鼠标已锁定");
    //    }
    //    else
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //        Debug.Log("主角已启用，鼠标已解锁");
    //    }
    //}

    public void ClosePanel()
    {
        Sequence closeSeq = DOTween.Sequence();

        closeSeq.Join(Promote.transform.DOScale(Vector3.zero, 0.5f));
        closeSeq.Join(BgCvsGroup.DOFade(0, 0.5f));

        closeSeq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            if (secondDialogPanel != null)
                secondDialogPanel.SetActive(false);
            // 关闭第三面板
            if (thirdDialogPanel != null)
                thirdDialogPanel.SetActive(false);
           // EnablePlayer(true);
        });

        Debug.Log("关闭全部对话面板");
    }
    #endregion

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextLine);
    }
}