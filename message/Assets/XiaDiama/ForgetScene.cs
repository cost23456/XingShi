using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgetScene : MonoBehaviour
{
    private bool playerInZone;
    private bool isLoading = false;

    void Update()
    {

        if (this == null || isLoading)
            return;

        if (ScenesManager.Instance == null)
            return;

        if (playerInZone && Input.GetKeyDown(KeyCode.F))
        {
            isLoading = true;
            ScenesManager.Instance.Loadscenes.SetActive(true);
            ScenesManager.Instance.LoadScene("ÒÅÍüÖ®Ëø");
            gameObject.SetActive(false);
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLoading || this == null) return;
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

        }
    }

}
