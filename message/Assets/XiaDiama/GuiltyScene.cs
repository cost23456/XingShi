using UnityEngine;

public class GuiltyScene : MonoBehaviour
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
            ScenesManager.Instance.LoadScene("À¢¾ÎÖ®»·");
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