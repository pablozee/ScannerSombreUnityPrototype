using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControl : MonoBehaviour
{
    private Material material;

    [SerializeField]
    private Color color;
    [SerializeField]
    private Vector2 scale;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        Debug.Log(material.GetColor("Color"));
    }

    private void Update()
    {
        material.SetColor("Color", color);
        material.SetVector("Scale", scale);
    }

}

