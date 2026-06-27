using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float rotateSpeed = 400f;

    public Vector3 offset; // 固定偏移，永远不变

    void Start()
    {
        // 一开始相机距离玩家多远，就永远保持这个距离
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Alt 切换鼠标锁定
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }

        // 左右旋转：绕玩家 Y 轴
        if (Input.GetAxis("Mouse X") != 0)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(player.position, Vector3.up, mouseX * rotateSpeed * Time.deltaTime);
        }

        // 上下旋转：绕玩家右侧轴旋转
        if (Input.GetAxis("Mouse Y") != 0)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            transform.RotateAround(player.position, transform.right, -mouseY * rotateSpeed * Time.deltaTime);
        }

        // 关键：强制保持初始距离，永远不会拉近拉远
        transform.position = player.position + offset.normalized * offset.magnitude;

        // 相机始终看玩家
        transform.LookAt(player);
    }
}