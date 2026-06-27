using UnityEngine;
using UnityEngine.UI;

public class ClickTrigger : MonoBehaviour
{
    public DialogSystem dialogSystem;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickItem);
    }

    void ClickItem()
    {
        if (dialogSystem != null)
        {
            dialogSystem.StartNewDialog();
        }
    }
}