using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("缩放设置")]
    public float zoomSpeed = 5f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("UI引用")]
    public Image game2;
    public GameObject game1;
    public Image game4;
    public GameObject game3;

    [Header("对话数据")]
    public DHdata dh;
    public DHdata dh2;

    [Header("音效设置")]
    public AudioClip clickSound;
    public float clickVolume = 0.7f;

    private AudioSource audioSource;
    private bool isInitialized = false;  // 防止重复初始化

    void Start()
    {
        if (!isInitialized)
        {
            SetupAudioSource();
            isInitialized = true;
        }

        // ✅ 检查是否有错误的调用
        Debug.Log("UImanager Start - 没有自动播放音效");
    }

    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = clickVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
        else if (audioSource != null)
        {
            audioSource.volume = clickVolume;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 moveDirection = transform.forward * scroll * zoomSpeed;
            transform.Translate(moveDirection, Space.World);
            Vector3 pos = transform.position;
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }
    }

    private void PlayClickSound()
    {
        // ✅ 添加安全检查：音效未设置时不播放
        if (clickSound == null)
        {
            Debug.LogWarning("点击音效未设置！");
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, clickVolume);
        }
        else
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position, clickVolume);
        }
    }

    public void Huan1()
    {
        Debug.Log("点击 Huan1");  // 调试信息
        PlayClickSound();
        DH.Instance.StartDialogue(dh);
    }

    public void Sence1()
    {
        Debug.Log("点击 Sence1");  // 调试信息
        PlayClickSound();
        game1.SetActive(false);
        game3.SetActive(true);
        game2.rectTransform.localPosition = new Vector3(800, 0, 0);
    }

    public void Sence2()
    {
        Debug.Log("点击 Sence2");  // 调试信息
        PlayClickSound();
        game3.SetActive(false);
        game1.SetActive(true);
        game4.rectTransform.localPosition = new Vector3(-800, 0, 0);
    }

    public void Huan2()
    {
        Debug.Log("点击 Huan2");  // 调试信息
        PlayClickSound();
        DH.Instance.StartDialogue(dh2);
    }
}