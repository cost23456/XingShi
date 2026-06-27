using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weixiudhua : MonoBehaviour
{
    public GameObject jiao;
    private RaycastHit2D hit;
    private bool isGrounded;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        if (isGrounded)
        {
            animator.SetTrigger("weixiu");
        }
        Debug.Log(hit.collider.name);
    }
    void CheckGrounded()
    {
        // 从角色底部向下发射射线，检测是否接触地面
        hit = Physics2D.Raycast(jiao.transform.position, Vector2.down, 1f);
        
        // 检查：1.检测到碰撞体 2.碰撞体标签是"road"
        isGrounded = hit.collider != null && hit.collider.tag == "road";

        // 调试用：显示射线
        Debug.DrawRay(jiao.transform.position, Vector2.down * 1f, Color.red);
    }
}
