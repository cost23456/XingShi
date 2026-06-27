using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Conversation : MonoBehaviour
{
    [Header("文本文件")]
    public TextAsset textFile;

    [Header("UI文件")]
    public Text textLabel;

    [Header("头像变暗设置")]
    public Image speakerAvatar1;
    public Image speakerAvatar2;
    public float activeBrightness = 1f;
    public float inactiveBrightness = 0.4f;

    [Header("说话者标识")]
    public string speaker1Name = "主角";
    public string speaker2Name = "NPC";

    [Header("打字效果设置")]
    public float typingSpeed = 0.05f;

    [Header("音效设置")]
    public AudioClip typingSound;              // 打字音效
    public float soundInterval = 0.1f;         // 音效播放间隔（秒）
    public float soundVolume = 0.5f;           // 音效音量

    public int index;

    List<string> textList = new List<string>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private AudioSource audioSource;
    private float lastSoundTime = 0f;

    void Awake()
    {
        GetTextFile(textFile);

        // 获取或添加 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = soundVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D 音效
            Debug.Log("已创建 AudioSource");
        }
        else
        {
            audioSource.volume = soundVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    private void OnEnable()
    {
        index = 0;

        if (textList.Count > 0 && index < textList.Count)
        {
            UpdateAvatar(textList[index]);
            StartTyping(textList[index]);
            index++;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && index >= textList.Count && !isTyping)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                textLabel.text = textList[index - 1];
                isTyping = false;
                return;
            }

            if (index < textList.Count)
            {
                UpdateAvatar(textList[index]);
                StartTyping(textList[index]);
                index++;
            }
            else
            {
                gameObject.SetActive(false);
                index = 0;
            }
        }
    }

    private void UpdateAvatar(string dialogueLine)
    {
        bool isSpeaker1 = dialogueLine.StartsWith(speaker1Name + ":");

        if (speakerAvatar1 != null)
        {
            SetAvatarBrightness(speakerAvatar1, isSpeaker1 ? activeBrightness : inactiveBrightness);
        }

        if (speakerAvatar2 != null)
        {
            SetAvatarBrightness(speakerAvatar2, isSpeaker1 ? inactiveBrightness : activeBrightness);
        }
    }

    private void SetAvatarBrightness(Image avatar, float brightness)
    {
        if (avatar == null) return;

        Color color = avatar.color;
        color.r = brightness;
        color.g = brightness;
        color.b = brightness;
        avatar.color = color;
    }

    private void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textLabel.text = "";

        string displayText = RemoveSpeakerPrefix(text);
        lastSoundTime = 0f;

        foreach (char c in displayText)
        {
            textLabel.text += c;

            // 播放打字音效（按间隔控制）
            if (typingSound != null && Time.time - lastSoundTime >= soundInterval)
            {
                PlayTypingSound();
                lastSoundTime = Time.time;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

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
            // 如果没有 AudioSource，用静态方法播放
            AudioSource.PlayClipAtPoint(typingSound, transform.position, soundVolume);
        }
    }

    private string RemoveSpeakerPrefix(string text)
    {
        if (text.Contains(":"))
        {
            int colonIndex = text.IndexOf(':');
            return text.Substring(colonIndex + 1).TrimStart();
        }
        return text;
    }

    void GetTextFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        if (file == null)
        {
            Debug.LogError("请拖入对话文本文件！");
            return;
        }

        var lineDate = file.text.Split('\n');

        foreach (var line in lineDate)
        {
            if (!string.IsNullOrWhiteSpace(line))
                textList.Add(line.Trim());
        }
    }
}