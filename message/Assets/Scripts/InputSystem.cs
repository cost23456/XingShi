using UnityEngine;
using UnityEngine.UI;
using System;

public class InputSystem : MonoBehaviour
{
    [Header("滚轮缩放")]
    public float zoomSpeed = 5f;        // 滚轮灵敏度
    public float minZ = -10f;           // 最近（最大放大）限制
    public float maxZ = 10f;            // 最远（最大缩小）限制

    [Header("鼠标拖拽")]
    public float dragSpeed = 0.5f;      // 拖拽灵敏度
    private Vector3 dragOrigin;          // 拖拽起始点
    private bool isDragging = false;     // 是否正在拖拽

    void Start()
    {
        
    }


    void Update()
    {
        // ========== 1. 滚轮缩放 ==========
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // 计算沿着相机自身Z轴（前进方向）的移动量
            Vector3 moveDirection = transform.forward * scroll * zoomSpeed;

            // 应用移动
            transform.Translate(moveDirection, Space.World);

            // 限制相机在Z轴上的位置
            Vector3 pos = transform.position;
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }

        // ========== 2. 鼠标右键拖拽 ==========
        // 按下鼠标右键：记录拖拽起点
        if (Input.GetMouseButtonDown(1))  // 1 = 鼠标右键
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        // 按住鼠标右键：执行拖拽
        if (Input.GetMouseButton(1) && isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 delta = currentMousePos - dragOrigin;

            // 将屏幕坐标增量转换为世界坐标增量
            // 注意：这里是在XY平面上移动，Z保持不变
            Vector3 moveDelta = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * Time.deltaTime;

            // 应用移动（保持Z轴不变）
            transform.Translate(moveDelta, Space.World);

            // 更新拖拽起点，防止跳跃
            dragOrigin = currentMousePos;
        }

        // 释放鼠标右键：结束拖拽
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }


    }

}