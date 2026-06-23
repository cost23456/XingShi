using UnityEngine;

public class InputSystem_zjy : MonoBehaviour
{
    public float zoomSpeed = 5f;        // 滚轮灵敏度
    public float minZ = -10f;           // 最近（最大放大）限制
    public float maxZ = 10f;            // 最远（最大缩小）限制

    void Update()
    {
        // 获取鼠标滚轮输入（向上滚动为正，向下为负）
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // 计算沿着相机自身Z轴（前进方向）的移动量
            Vector3 moveDirection = transform.forward * scroll * zoomSpeed;

            // 应用移动
            transform.Translate(moveDirection, Space.World);

            // 【关键】限制相机在Z轴上的位置，防止穿墙或跑出边界
            Vector3 pos = transform.position;
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }
    }
}