using UnityEngine;
using UnityEngine.UI;

public class weixiu : MonoBehaviour
{
    public GameObject player;
    public Text weixiuText;
    public Image weixiuImage;
    public GameObject weixiugong;
    public GameObject luzhang;
    public Button weixiuBtn;
    public GameObject yanwuper;
    private GameObject yanwu;
    // Start is called before the first frame update
    void Start()
    {
        weixiugong.SetActive(false);
        weixiuBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            weixiuBtn.gameObject.SetActive(true);
            weixiuText.text = "召唤维修虫";
        }
    }
    
    public void Weixiu()
    {
        weixiugong.SetActive(true);
        yanwu = Instantiate(yanwuper,luzhang.transform.position,transform.rotation);
        Destroy(luzhang,2f);
        Destroy(yanwu,2f);
        Destroy(weixiugong,2.5f);
    }
}
