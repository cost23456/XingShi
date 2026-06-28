using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject _2DPmt;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            this._2DPmt.SetActive(true);
            Destroy(_2DPmt.gameObject,3f);
        }
    }
}
