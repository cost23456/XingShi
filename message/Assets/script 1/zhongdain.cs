using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zhongdain : MonoBehaviour
{
    public GameObject playerxin;
    public GameObject xinfPrefab;  // 改为命名为 Prefab
    public GameObject zhongdian;
    private bool isSuccess = false;
    private bool isLoad = false;
    private GameObject currentXinf;  // 当前生成的 xinf 对象引用

    void Start()
    {
        
    }

    void Update()
    {
        // 只有当存在当前生成的对象时才移动
        if (currentXinf != null)
        {
            currentXinf.transform.position = Vector2.MoveTowards(
                currentXinf.transform.position, 
                zhongdian.transform.position, 
                8f * Time.deltaTime
            );
            
            // 检查是否到达目标位置
            if (Vector2.Distance(currentXinf.transform.position, zhongdian.transform.position) < 0.1f)
            {
                Debug.Log("xinf 已到达终点");
                // 到达终点后可以选择销毁对象或做其他处理
                // Destroy(currentXinf);
                // currentXinf = null;
                this.isSuccess = true;
            }
        }
        if (isSuccess && !isLoad)
        {
            this.isLoad = true;
            ScenesManager.Instance.LoadScene("遗憾回廊");
            ScenesManager.Instance.Loadscenes.SetActive(true);
            gameobjects.Instance._2DisSuccess = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // 保存生成的对象引用
            currentXinf = Instantiate(xinfPrefab, playerxin.transform.position, other.transform.rotation);
            Debug.Log("生成了新的 xinf 对象");
        }
    }
}