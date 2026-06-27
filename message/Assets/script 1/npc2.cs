using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npc2 : MonoBehaviour
{
    public GameObject duihua1;
    public GameObject duihua2;
    // Start is called before the first frame update
    void Start()
    {
        duihua1.SetActive(false);
        duihua2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            duihua1.SetActive(true);
            duihua2.SetActive(true);
        }
    }
}
