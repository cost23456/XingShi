using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float lookSensitivity = 2f;

    [Header("鼠标设置")]
    public float mouseSensitivity = 2f;
    public float minYAngle = -80f;
    public float maxYAngle = 80f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("组件引用")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckGrounded();
        HandleMouseLook();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }

    void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // 简单地面检测：从脚底向下射线检测
            RaycastHit hit;
            float distance = 0.2f;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, distance + 0.1f, groundLayer);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        if (cameraTransform != null)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // 计算移动方向（相对于角色朝向）
        Vector3 move = transform.forward * vertical + transform.right * horizontal;

        if (move.magnitude > 1f)
            move.Normalize();

        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        // 使用 Rigidbody 移动
        Vector3 targetVelocity = move * currentSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity = targetVelocity;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void ApplyGravity()
    {
        // 重力由物理引擎自动处理，不需要手动添加
        // 如果需要在空中限制下落速度，可以在这里处理
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}