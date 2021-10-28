using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the instancing of the shader
/// </summary>
public class ShaderControl : MonoBehaviour
{
    //Describes the shape of the cirles
    [SerializeField]
    private float density;
    [SerializeField]
    private float size;
    private Material material;
  //Randomizes the pattern
    private const float ANGLE_RANGE_MIN = 20;
    private const float ANGLE_RANGE_MAX = 1000;

    //Properies of the shader
    private const string densityRefernce = "CellDensity";
    private const string angleRefernce = "CellAngle";
    private const string scaleRefernce = "Scale";
    private const string visablityRefernce = "Visablity";
    private const string sizeRefernce = "Size";

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        float angle = Random.Range(ANGLE_RANGE_MIN, ANGLE_RANGE_MAX);

        //Set instance properties
        material.SetVector(scaleRefernce, transform.lossyScale);
        material.SetFloat(angleRefernce, angle);
        material.SetFloat(densityRefernce, density);
        material.SetFloat(visablityRefernce, 0);
        material.SetFloat(sizeRefernce, size);
    }

    /// <summary>
    /// Used to update distance
    /// </summary>
    private void Update()
    {
    }

    /// <summary>
    /// Displays the object in it's entirity
    /// </summary>
    public void Hitted()
    {
        material.SetFloat(visablityRefernce, 1);
    }
}

