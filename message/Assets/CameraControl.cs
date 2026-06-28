using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float rotateSpeed = 400f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private Vector3 originalOffset;
    private float pitchAngle;
    private float yawAngle;

    void Start()
    {
        if (gameobjects.Instance._2DisSuccess) return;
        // 开机自动锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalOffset = transform.position - player.position;
        yawAngle = transform.eulerAngles.y;
        pitchAngle = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (gameobjects.Instance._2DisSuccess) return;
        // Alt 解锁/重新锁定鼠标
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // 鼠标锁定时才允许旋转视角
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // 水平左右旋转（Yaw）
        float mouseX = Input.GetAxis("Mouse X");
        yawAngle += mouseX * rotateSpeed * Time.deltaTime;

        // 上下俯仰旋转（Pitch，限制角度防止翻转）
        float mouseY = Input.GetAxis("Mouse Y");
        pitchAngle -= mouseY * rotateSpeed * Time.deltaTime;
        pitchAngle = Mathf.Clamp(pitchAngle, minPitch, maxPitch);

        // 根据俯仰+水平角度计算相机位置
        Quaternion rot = Quaternion.Euler(pitchAngle, yawAngle, 0);
        transform.position = player.position + rot * originalOffset;
        transform.LookAt(player);
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}