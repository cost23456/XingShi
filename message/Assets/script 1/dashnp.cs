using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dashnp : MonoBehaviour
{
    public Text dashnpText;
    public GameObject chuf;
    public Image dashnpImage;
    public GameObject dash;
    public GameObject player;
    private bool duihua = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!duihua)
        {
            player.GetComponent<move>().enabled = true;
            player.GetComponent<move>().Dash();
            duihua = true;
        }
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            dash.SetActive(true);
            player.GetComponent<move>().enabled = false;
            duihua = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            dash.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
