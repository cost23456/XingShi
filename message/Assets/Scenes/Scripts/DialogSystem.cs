using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogSystem : MonoBehaviour
{
    [Header("UI绑定")]
    public GameObject dialogPanel;
    public Text dialogText;

    [Header("人物父物体")]
    public GameObject grandfatherObj;
    public GameObject granddaughterObj;
    [Header("人物图片组件")]
    public RawImage grandpaImg;
    public RawImage girlImg;

    [Header("打字设置")]
    public float typeSpeed = 0.05f;

    [Header("音效设置")]
    public AudioClip typingSound;              // 打字音效
    public float soundInterval = 0.05f;        // 音效播放间隔（秒）
    public float soundVolume = 0.5f;           // 音效音量

    [System.Serializable]
    public struct SentenceData
    {
        [TextArea] public string content;
        // 0=爷爷说话  1=孙女说话
        public int speaker;
    }
    public SentenceData[] dialogList;

    private int currentIndex;
    private bool dialogIsOpen = false;

    // 打字新增变量
    private int charShowIndex;
    private float typeTimer;
    private bool isTypingNow;
    private AudioSource audioSource;
    private float lastSoundTime = 0f;

    void Start()
    {
        // 初始化音效
        SetupAudioSource();

        SetAllDark();
        dialogPanel.SetActive(false);
        grandfatherObj.SetActive(false);
        granddaughterObj.SetActive(false);
        dialogIsOpen = false;
        currentIndex = 0;
        charShowIndex = 0;
        typeTimer = 0;
        isTypingNow = false;
        RefreshSentence();
    }

    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && typingSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = soundVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
        else if (audioSource != null)
        {
            audioSource.volume = soundVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        if (typingSound == null)
        {
            Debug.LogWarning("typingSound 未赋值！请在 Inspector 中拖入音效文件");
        }
    }

    // 点击尺子触发对话
    public void StartNewDialog()
    {
        grandfatherObj.SetActive(true);
        granddaughterObj.SetActive(true);
        dialogPanel.SetActive(true);
        dialogIsOpen = true;

        currentIndex = 0;
        PlayTypeWriter();
        Canvas.ForceUpdateCanvases();
    }

    // 刷新当前句子基础内容（人物明暗）
    void RefreshSentence()
    {
        if (dialogList == null || dialogList.Length == 0) return;
        SentenceData data = dialogList[currentIndex];

        SetAllDark();
        if (data.speaker == 0)
        {
            grandpaImg.color = Color.white;
        }
        else if (data.speaker == 1)
        {
            girlImg.color = Color.white;
        }
    }

    // 开始逐字打印当前句子
    void PlayTypeWriter()
    {
        RefreshSentence();
        charShowIndex = 0;
        typeTimer = 0;
        isTypingNow = true;
        lastSoundTime = 0f;
        dialogText.text = "";
    }

    void SetAllDark()
    {
        grandpaImg.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
        girlImg.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        grandfatherObj.SetActive(false);
        granddaughterObj.SetActive(false);
        dialogIsOpen = false;
        Canvas.ForceUpdateCanvases();
    }

    void Update()
    {
        // 逐字打字逻辑
        if (dialogIsOpen && isTypingNow)
        {
            typeTimer += Time.deltaTime;
            if (typeTimer >= typeSpeed)
            {
                typeTimer = 0;
                SentenceData data = dialogList[currentIndex];
                charShowIndex++;
                dialogText.text = data.content.Substring(0, charShowIndex);

                // 播放打字音效
                if (typingSound != null && Time.time - lastSoundTime >= soundInterval)
                {
                    PlayTypingSound();
                    lastSoundTime = Time.time;
                }

                // 当前句子全部打完，停止打字
                if (charShowIndex >= data.content.Length)
                {
                    isTypingNow = false;
                }
            }
        }

        // 对话开启 + 鼠标左键按下
        if (dialogIsOpen && Input.GetMouseButtonDown(0))
        {
            // 判断点击落在对话框UI上
            if (EventSystem.current.IsPointerOverGameObject())
            {
                NextSentence();
            }
        }
    }

    void NextSentence()
    {
        // 如果还在打字，直接显示完整句子，不跳转
        if (isTypingNow)
        {
            SentenceData data = dialogList[currentIndex];
            dialogText.text = data.content;
            isTypingNow = false;
            return;
        }

        // 打字结束，切换下一句
        if (currentIndex < dialogList.Length - 1)
        {
            currentIndex++;
            PlayTypeWriter();
        }
        else
        {
            CloseDialog();
        }
    }

    /// <summary>
    /// 播放打字音效
    /// </summary>
    private void PlayTypingSound()
    {
        if (audioSource != null && typingSound != null)
        {
            audioSource.PlayOneShot(typingSound, soundVolume);
        }
        else if (typingSound != null)
        {
            // 备用方案：静态播放
            AudioSource.PlayClipAtPoint(typingSound, transform.position, soundVolume);
        }
    }
}