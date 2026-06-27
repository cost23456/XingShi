using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openui : MonoBehaviour
{
    public GameObject UI1;
    public GameObject UI2;
    public GameObject player;
    public GameObject Ui3;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UI1.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.SetActive(false);
            UI2.SetActive(true);


        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            Ui3.SetActive(true);
        }
    }
}
