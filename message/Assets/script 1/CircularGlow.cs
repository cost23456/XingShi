using System;
using UnityEngine;

[ExecuteInEditMode]
public class CircularGlow : MonoBehaviour
{
    public Color glowColor = Color.white;
    public float radius = 2f;
    public float intensity = 1f;
    public int segments = 64;
    public bool fadeOut = true;
    public float fadeSpeed = 1f;

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Awake()
    {
        InitializeMesh();
    }

    void Start()
    {
        UpdateGlow();
    }

    void Update()
    {
        if (fadeOut)
        {
            glowColor.a = Mathf.PingPong(Time.time * fadeSpeed, 1f);
            UpdateGlow();
        }
    }

    void InitializeMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        var renderer = GetComponent<MeshRenderer>();
        if (renderer == null)
            renderer = gameObject.AddComponent<MeshRenderer>();
        
        // 使用自定义着色器实现发光效果
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        renderer.sharedMaterial = mat;
    }

    void UpdateGlow()
    {
        if (mesh == null) return;

        var vertices = new Vector3[segments + 2];
        var uvs = new Vector2[segments + 2];
        var colors = new Color[segments + 2];
        var normals = new Vector3[segments + 2];
        var indices = new int[segments * 3];

        // 中心顶点
        vertices[0] = Vector3.zero;
        uvs[0] = Vector2.zero;
        colors[0] = new Color(glowColor.r, glowColor.g, glowColor.b, glowColor.a * intensity);
        normals[0] = Vector3.forward;

        // 边缘顶点
        for (int i = 0; i < segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            vertices[i + 1] = new Vector3(x, y, 0);
            uvs[i + 1] = new Vector2(
                Mathf.Cos(angle) * 0.5f + 0.5f,
                Mathf.Sin(angle) * 0.5f + 0.5f
            );
            
            // 渐变透明度
            float dist = Vector2.Distance(Vector2.zero, new Vector2(x, y)) / radius;
            float alpha = 1f - dist;
            colors[i + 1] = new Color(
                glowColor.r, 
                glowColor.g, 
                glowColor.b, 
                alpha * glowColor.a * intensity
            );
            normals[i + 1] = Vector3.forward;
        }

        // 索引
        for (int i = 0; i < segments; i++)
        {
            indices[i * 3] = 0;
            indices[i * 3 + 1] = i + 1;
            indices[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.normals = new Vector3[vertices.Length];
        Array.Fill(mesh.normals, Vector3.forward);
        mesh.triangles = indices;
        mesh.RecalculateBounds();
    }
}
