using UnityEngine;

/// <summary>
/// 单条对话结构：包含说话人、对话文本
/// </summary>
[System.Serializable]
public struct Sentence
{
   

    [Header("对话内容")]
    [TextArea(2, 5)] // 编辑器多行输入框，最少2行最多5行
    public string content;
}

/// <summary>
/// 对话资源载体，ScriptableObject，用来存一整段NPC对话
/// Project面板右键可创建配置文件，方便统一管理所有NPC台词
/// </summary>
[CreateAssetMenu(fileName = "NPC对话_老师", menuName = "NPC对话/新建对话集")]
public class DHdata : ScriptableObject
{
    [Header("该NPC的全部对话句子")]
    public Sentence[] dialogueLines;
}