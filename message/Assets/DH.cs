using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 对话系统总管理器，控制UI显示、打字机动画、切换台词
/// 单例模式：所有NPC脚本直接 DialogueManager.Instance 调用
/// </summary>
public class DH : MonoBehaviour
{
    // 单例静态实例，全局唯一访问入口
    public static DH Instance;

    [Header("对话UI面板绑定")]
    [Tooltip("整个对话弹窗面板，默认隐藏")]
    public GameObject dialoguePanel;
    [Tooltip("显示对话正文的文本组件")]
    public Text txtContent;
    [Header("人物")]
    public Image RW1;
    public Image RW2;

    [Header("音效设置")]
    public AudioClip typingSound;              // 打字音效
    public float soundInterval = 0.04f;        // 音效播放间隔（秒）
    public float soundVolume = 0.5f;           // 音效音量

    private int a = 0;

    // 当前正在播放的对话资源
    private DHdata curDialogue;
    // 当前播放到第几句
    private int curIndex;
    // 标记：是否正在播放打字逐字动画
    private bool isTyping = false;
    // 标记：对话是否开启
    private bool isDialogueOpen = false;
    // 音效组件
    private AudioSource audioSource;
    private float lastSoundTime = 0f;

    private void Awake()
    {
        // 单例初始化，保证场景里只有一个对话管理器
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // 如果重复挂载，直接删除多余对象
            Destroy(gameObject);
        }

        // 初始化音效
        SetupAudioSource();
    }

    private void Start()
    {
        // 游戏开始时隐藏对话面板
        dialoguePanel.SetActive(false);
    }

    private void SetupAudioSource()
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

    private void Update()
    {
        // 对话开启时，检测鼠标点击
        if (isDialogueOpen && Input.GetMouseButtonDown(0))
        {
            ShowNextSentence();
        }
    }

    /// <summary>
    /// 外部NPC调用：开启一段新对话
    /// </summary>
    /// <param name="data">传入对应NPC的对话配置资源</param>
    public void StartDialogue(DHdata data)
    {
        // 赋值当前对话数据
        curDialogue = data;
        // 重置台词索引，从第一句开始
        curIndex = 0;
        // 显示对话弹窗
        dialoguePanel.SetActive(true);
        isDialogueOpen = true;

        if (a == 0)
        {
            RW1.color = new Color(255f, 255f, 255F, 255f);
            RW2.color = new Color(0f, 0f, 0f, 255f);
            a = 1;
        }
        else
        {
            RW2.color = new Color(255f, 255f, 255F, 255f);
            RW1.color = new Color(0f, 0f, 0f, 255f);
            a = 0;
        }

        // 渲染第一句台词
        ShowCurrentLine();
    }

    /// <summary>
    /// 渲染当前索引对应的台词，附带打字机动画
    /// </summary>
    private void ShowCurrentLine()
    {
        // 获取当前这句对话数据
        Sentence line = curDialogue.dialogueLines[curIndex];
        // 停止上一次未播放完的打字协程，防止文字重叠
        StopAllCoroutines();
        // 开启逐字打字动画
        StartCoroutine(TypeWriter(line.content));
    }

    /// <summary>
    /// 打字机逐字显示协程
    /// </summary>
    /// <param name="text">需要逐字打印的完整文本</param>
    private IEnumerator TypeWriter(string text)
    {
        isTyping = true;
        // 先清空文本框
        txtContent.text = "";
        lastSoundTime = 0f;

        // 循环每一个字符，逐个添加到文本
        foreach (char c in text)
        {
            txtContent.text += c;

            // 播放打字音效
            if (typingSound != null && Time.time - lastSoundTime >= soundInterval)
            {
                PlayTypingSound();
                lastSoundTime = Time.time;
            }

            // 每个字间隔0.04秒，控制打字速度
            yield return new WaitForSeconds(0.04f);
        }

        // 文字全部打印完成
        isTyping = false;
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

    /// <summary>
    /// 下一句执行逻辑
    /// </summary>
    public void ShowNextSentence()
    {
        // 如果对话未开启，不执行
        if (!isDialogueOpen) return;

        // 切换人物头像高亮
        if (a == 0)
        {
            RW1.color = new Color(1f, 1f, 1f, 1f);      // RW1 完全显示
            RW2.color = new Color(0.4f, 0.4f, 0.4f, 1f);    // RW2 透明度0.4
            a = 1;
        }
        else
        {
            RW2.color = new Color(1f, 1f, 1f, 1f);      // RW2 完全显示
            RW1.color = new Color(0.4f, 0.4f, 0.4f, 1f);    // RW1 透明度0.4
            a = 0;
        }

        // 如果还在打字，点击直接显示完整文字，跳过动画
        if (isTyping)
        {
            StopAllCoroutines();
            Sentence line = curDialogue.dialogueLines[curIndex];
            txtContent.text = line.content;
            isTyping = false;
            return;
        }

        // 索引+1，切换下一句
        curIndex++;

        // 判断是否还有剩余台词
        if (curIndex < curDialogue.dialogueLines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            // 所有台词播放完毕，关闭对话面板
            CloseDialogue();
        }
    }

    /// <summary>
    /// 强制关闭对话弹窗，清空动画
    /// </summary>
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueOpen = false;
        StopAllCoroutines();
    }
}