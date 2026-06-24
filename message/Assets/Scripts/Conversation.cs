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
    public Image speakerAvatar1;          // 说话者1头像
    public Image speakerAvatar2;          // 说话者2头像
    public float activeBrightness = 1f;   // 说话时亮度（正常）
    public float inactiveBrightness = 0.4f; // 不说话时亮度（变暗）

    [Header("说话者标识")]
    public string speaker1Name = "主角";   // 说话者1名称
    public string speaker2Name = "NPC";    // 说话者2名称

    [Header("打字效果设置")]
    public float typingSpeed = 0.05f;

    public int index;

    List<string> textList = new List<string>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Awake()
    {
        GetTextFile(textFile);
    }

    private void OnEnable()
    {
        index = 0;

        if (textList.Count > 0 && index < textList.Count)
        {
            // 显示第一句前先更新头像
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
                // 显示下一句前更新头像
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

    /// <summary>
    /// 根据对话内容更新头像亮度
    /// </summary>
    private void UpdateAvatar(string dialogueLine)
    {
        // 判断说话者
        bool isSpeaker1 = dialogueLine.StartsWith(speaker1Name + ":");

        // 设置头像亮度
        if (speakerAvatar1 != null)
        {
            SetAvatarBrightness(speakerAvatar1, isSpeaker1 ? activeBrightness : inactiveBrightness);
        }

        if (speakerAvatar2 != null)
        {
            SetAvatarBrightness(speakerAvatar2, isSpeaker1 ? inactiveBrightness : activeBrightness);
        }
    }

    /// <summary>
    /// 设置单个头像亮度（颜色变暗，保留Alpha）
    /// </summary>
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

        // 如果文本包含说话者前缀，移除它再显示
        string displayText = RemoveSpeakerPrefix(text);

        foreach (char c in displayText)
        {
            textLabel.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    /// <summary>
    /// 移除对话中的说话者前缀
    /// </summary>
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