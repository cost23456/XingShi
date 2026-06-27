using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float moveSpeed = 5f;          // 移动速度
    public float jiashuspeed=50f;         // 加速速度
    public float jumpForce = 7f;          // 跳跃力
    public float flyDuration = 2f;        // 最大飞行时间
    public float flyForce = 3f;           // 飞行时的上升力
    public float dashForce = 10f;         // 跳跃时的上升力
    private Rigidbody2D rb;
    public GameObject fs;
    private bool isGrounded = false;      // 是否着地
    private bool isFlying = false;        // 是否正在飞行
    private float flyTimer = 0f;          // 飞行计时器
    private RaycastHit2D hit;             // 射线检测结果
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 冻结旋转，防止碰撞后角色旋转
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 检测是否着地（使用射线检测）
        CheckGrounded();
        //Debug.Log(hit.collider.tag);
        // 左右移动
        float horizontal = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.K))
        {
            Dash();
        }

        // 使用Rigidbody2D实现2D移动
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        //Debug.Log(horizontal*moveSpeed);
        //rb.AddForce(new Vector2(horizontal * moveSpeed, 0f), ForceMode2D.Impulse);
        
        // 设置移动动画参数
        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontal));
        }
        
        // 转向：按A向左转，按D向右转
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1.5f,1.5f,1);
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }

        // 跳跃和飞行逻辑
        HandleJumpAndFly();
    }

    // 射线检测是否着地
    void CheckGrounded()
    {
        // 从角色底部向下发射射线，检测是否接触地面
        hit = Physics2D.Raycast(fs.transform.position, Vector2.down, 1f);
        
        // 检查：1.检测到碰撞体 2.碰撞体标签是"road"
        isGrounded = hit.collider != null && hit.collider.tag == "road";

        // 调试用：显示射线
        Debug.DrawRay(fs.transform.position, Vector2.down * 1f, Color.red);
    }
    public void Dash()
{
    animator.SetBool("jiashu", true);
    // 开始协程，以jiashuspeed速度移动指定距离(5个单位)
    StartCoroutine(DashCoroutine(40f));
}

IEnumerator DashCoroutine(float dashDistance)
{
    float startX = transform.position.x;
    float targetX = startX + dashDistance * Mathf.Sign(transform.localScale.x);
    
    // 设置冲刺速度
    rb.velocity = new Vector2(jiashuspeed * Mathf.Sign(transform.localScale.x), rb.velocity.y);
    float temp = moveSpeed;
    moveSpeed = jiashuspeed;
    // 等待移动到目标位置
    while (Mathf.Abs(transform.position.x - startX) < dashDistance)
    {
        yield return null;
    }
    
    // 移动完成后将jiashu改为false
    animator.SetBool("jiashu", false);
    // 重置水平速度
    moveSpeed = temp;
    //rb.velocity = new Vector2(0, rb.velocity.y);
}

    // 处理跳跃和飞行
    void HandleJumpAndFly()
    {
        // 长按空格飞行
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // 刚按空格时跳跃
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
                animator.SetTrigger("jump");
                
            }
        }
    }
}