using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    private CharacterController Controller;

    [Header("输入设置")]
    private float horizontal;
    private float vertical;
    private Vector3 direction;

    [Header("旋转设置")]
    [SerializeField] private float turnSpeed;//角色旋转速度
    [SerializeField] private Camera mainCamera;

    [Header("跳跃设置")]
    [SerializeField] private float jumpHeight;//跳跃高度
    [SerializeField] private float gravity;//重力加速度
    private Vector3 velocityGravity;//速度
    private bool IsGround;

    [Header("移动设置")]
    [SerializeField] private float moveSpeed;
    private Vector3 moveDirection;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        SetPlayerMove();
        SetPlayerRotation();
        SetPlayerJump();
        SetPlayerGravity();
    }

    private void SetPlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        direction = new Vector3(horizontal, 0, vertical);

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 【这里修复了！】正确的相机视角移动公式
        moveDirection = cameraForward * vertical + cameraRight * horizontal;

        Controller.Move(moveSpeed * Time.deltaTime * moveDirection.normalized);
    }

    private void SetPlayerRotation()
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetrotation = Quaternion.LookRotation(moveDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, turnSpeed * Time.deltaTime);
        }
    }

    private void SetPlayerJump()
    {
        if (IsGround && Input.GetButtonDown("Jump"))
        {
            velocityGravity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private void SetPlayerGravity()
    {
        Controller.Move(velocityGravity * Time.deltaTime);

        IsGround = Controller.isGrounded;
        if (IsGround)
        {
            velocityGravity.y = -2f;
        }
        else
        {
            velocityGravity.y += gravity * Time.deltaTime;
        }
    }
}