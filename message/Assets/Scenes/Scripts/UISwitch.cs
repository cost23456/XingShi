using UnityEngine;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{
    // 原有切换分组（你已经赋值好，不用改）
    public GameObject YardGroup;
    public GameObject HospitalGroup;

    [Header("院落对话配置")]
    public Text yardText;
    public Image yardDialogBg;
    [TextArea(2, 5)] public string[] yardDialogList;
    public float yardTypeSpeed = 0.05f;

    [Header("医院对话配置")]
    public Text hospitalText;
    public Image hospitalDialogBg;
    [TextArea(2, 5)] public string[] hospitalDialogList;
    public float hospitalTypeSpeed = 0.05f;

    // 院落打字缓存
    private int yardLineIndex;
    private int yardCharIndex;
    private float yardTimer;
    private bool yardIsTyping;

    // 医院打字缓存
    private int hospitalLineIndex;
    private int hospitalCharIndex;
    private float hospitalTimer;
    private bool hospitalIsTyping;

    void Update()
    {
        // 院落逐字打印逻辑
        if (yardIsTyping)
        {
            yardTimer += Time.deltaTime;
            if (yardTimer >= yardTypeSpeed)
            {
                yardTimer = 0;
                yardCharIndex++;
                yardText.text = yardDialogList[yardLineIndex].Substring(0, yardCharIndex);
                if (yardCharIndex >= yardDialogList[yardLineIndex].Length)
                {
                    yardIsTyping = false;
                }
            }
        }

        // 医院逐字打印逻辑
        if (hospitalIsTyping)
        {
            hospitalTimer += Time.deltaTime;
            if (hospitalTimer >= hospitalTypeSpeed)
            {
                hospitalTimer = 0;
                hospitalCharIndex++;
                hospitalText.text = hospitalDialogList[hospitalLineIndex].Substring(0, hospitalCharIndex);
                if (hospitalCharIndex >= hospitalDialogList[hospitalLineIndex].Length)
                {
                    hospitalIsTyping = false;
                }
            }
        }
    }

    // 原有切换函数，保留不动
    public void GoHospital()
    {
        YardGroup.SetActive(false);
        HospitalGroup.SetActive(true);
        // 切场景清空院落对话
        yardText.text = "";
        yardDialogBg.transform.parent.gameObject.SetActive(false);
    }

    public void GoYard()
    {
        HospitalGroup.SetActive(false);
        YardGroup.SetActive(true);
        // 切场景清空医院对话
        hospitalText.text = "";
        hospitalDialogBg.transform.parent.gameObject.SetActive(false);
    }

    #region 院落对话调用方法
    // 尺子Ruler点击：打开院落对话从头播放
    public void OpenYardDialog()
    {
        yardLineIndex = 0;
        PlayYardSingleSentence();
        yardDialogBg.transform.parent.gameObject.SetActive(true);
    }

    // 点击院落对话框底板：跳完整文字 / 切换下一句
    public void ClickYardDialog()
    {
        if (yardIsTyping)
        {
            yardText.text = yardDialogList[yardLineIndex];
            yardIsTyping = false;
            return;
        }

        yardLineIndex++;
        if (yardLineIndex >= yardDialogList.Length)
        {
            yardText.text = "";
            yardDialogBg.transform.parent.gameObject.SetActive(false);
            return;
        }
        PlayYardSingleSentence();
    }

    void PlayYardSingleSentence()
    {
        yardCharIndex = 0;
        yardTimer = 0;
        yardText.text = "";
        yardIsTyping = true;
    }
    #endregion

    #region 医院对话调用方法
    // 医院Click道具点击：打开医院对话
    public void OpenHospitalDialog()
    {
        hospitalLineIndex = 0;
        PlayHospitalSingleSentence();
        hospitalDialogBg.transform.parent.gameObject.SetActive(true);
    }

    // 点击医院对话框底板
    public void ClickHospitalDialog()
    {
        if (hospitalIsTyping)
        {
            hospitalText.text = hospitalDialogList[hospitalLineIndex];
            hospitalIsTyping = false;
            return;
        }

        hospitalLineIndex++;
        if (hospitalLineIndex >= hospitalDialogList.Length)
        {
            hospitalText.text = "";
            hospitalDialogBg.transform.parent.gameObject.SetActive(false);
            return;
        }
        PlayHospitalSingleSentence();
    }

    void PlayHospitalSingleSentence()
    {
        hospitalCharIndex = 0;
        hospitalTimer = 0;
        hospitalText.text = "";
        hospitalIsTyping = true;
    }
    #endregion
}