using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    private CharacterController Controller;

    [Header("输入设置")]
    private float horizontal;
    private float vertical;
    private Vector3 inputDir;

    [Header("旋转设置")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private Camera mainCamera;

    [Header("移动设置")]
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        this.Controller = this.GetComponent<CharacterController>();

        if (gameobjects.Instance != null)
        {
            gameobjects.Instance.player = this.transform;
        }
    }
    private void Start()
    {
        if (gameobjects.Instance != null && gameobjects.Instance.player != null)
        {
            this.transform.position = gameobjects.Instance.player.position;
        }
    }

    private void Update()
    {
        if (gameobjects.Instance._2DisSuccess) return;
        this.SetPlayerMove();
    }

    private void SetPlayerMove()
    {
        if (gameobjects.Instance._2DisSuccess) return;
        this.horizontal = Input.GetAxis("Horizontal");
        this.vertical = Input.GetAxis("Vertical");
        this.inputDir = new Vector3(this.horizontal, 0, this.vertical).normalized;

        // 获取相机水平前后、左右（去掉Y，只水平面）
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // WASD 相对于相机方向移动
        Vector3 moveDir = (camForward * this.vertical + camRight * this.horizontal).normalized;

        // 有移动输入时，人物面朝相机前方
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRot, this.turnSpeed * Time.deltaTime);
        }

        this.Controller.Move(moveDir * this.moveSpeed * Time.deltaTime);
    }
}