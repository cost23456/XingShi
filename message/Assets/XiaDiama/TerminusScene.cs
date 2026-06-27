using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminusScene : MonoBehaviour
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
            ScenesManager.Instance.LoadScene("3D³¡¾°");
            gameObject.SetActive(false);

        }
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLoading || this == null) return;
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;

        }
    }

}
