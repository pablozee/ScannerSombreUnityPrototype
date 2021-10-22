using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the instancing of the shader
/// </summary>
public class ShaderControl : MonoBehaviour
{
   
    [SerializeField]
    private Color color;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float density;
    [SerializeField]
    private bool isVisable;
    [SerializeField]
    private float size;

    private Material material;

    //Properies of the shader
    private const string colorRefernce = "Color_381494d5b8a84c46a3408fdecd5e6049";
    private const string scaleRefernce = "Vector2_45412501e588472982e0fdc755b7d60d";
    private const string angleRefernce = "Vector1_2b9352adfb1047d290484cdc243cd534";
    private const string densityRefernce = "Vector1_6ec47fd4e6044714ba16d677c0e36e2b";
    private const string sizeRefernce = "Vector1_3bf0dbc98a034fb88eec7350a9341a91";
    private const string visablityRefernce = "Vector1_ed74f5af198a44d0ab6514cd0c63b21f";


    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetFloat(visablityRefernce,0);
    }

    private void Update()
    {
        material.SetColor(colorRefernce, color);
      material.SetVector(scaleRefernce, transform.lossyScale);
        material.SetFloat(angleRefernce, angle);
        material.SetFloat(densityRefernce, density);
        material.SetFloat(sizeRefernce, size);
    }

    public void Hitted()
    {
        material.SetFloat(visablityRefernce, 1);
    }
}

