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

    #region Shader Properties
    private const string densityRefernce = "CellDensity";
    private const string angleRefernce = "CellAngle";
    private const string scaleRefernce = "Scale";
    private const string visablityRefernce = "Visablity";
    private const string sizeRefernce = "Size";
    private const string  minLimits = "MinLimits";
    private const string maxLimits = "MaxLimits";

    #endregion
    private Vector2 xCurrentLimits;
    private Vector2 yCurrentLimits;
    private List<Grid> gridLayout;

    [SerializeField]
    private float gridSize = 5;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        float angle = Random.Range(ANGLE_RANGE_MIN, ANGLE_RANGE_MAX);

        xCurrentLimits = new Vector2(1, 0);
        yCurrentLimits = new Vector2(0, 1);
        CreateGrid();
     
        //Set instance properties
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
    public void Hitted(Vector2 pos)
    {
        Vector2 uv = transform.InverseTransformPoint(pos);
        material.SetFloat(visablityRefernce, 1);

        //    CheckGrid(uv);
    }

    /// <summary>
    /// Determiens which partition of the model to display
    /// </summary>
    /// <param name="pos"></param>
    private void CheckGrid(Vector2 pos)
    {
        Debug.Log(pos);
        pos += new Vector2(5f, 5f);
        pos /= 10;
        Debug.Log(pos);

        //Check x
        if (pos.x < xCurrentLimits.x)
        {
            xCurrentLimits.x = pos.x;
        }
        if (pos.x > xCurrentLimits.y)
        {
            xCurrentLimits.y = pos.x;
        }


        Vector2 Xlimts = new Vector2(xCurrentLimits.x, xCurrentLimits.y);
        Vector2 yLimtis = new Vector2(0, 1);
        material.SetVector(xlimits0, Xlimts);
        material.SetVector(ylimits0, yLimtis);
    }

    /// <summary>
    /// Create the grid
    /// </summary>
    private void CreateGrid()
    {
        gridLayout = new List<Grid>();

        //Get min pos
        BoxCollider box = GetComponent<BoxCollider>();
        Vector3 size = box.size;
        Vector3 center = box.center;

        Vector3 min = center - (size / 2);
        Vector3 max = center + (size / 2);

        min = transform.TransformPoint(min);
        max = transform.TransformPoint(max);

        float xStep = (max.x - min.x) /gridSize ;
        float yStep = (max.y - min.y) / gridSize;
        float zStep = (max.z - min.z) / gridSize;

        for (float x = min.x; x < max.x;)
        {
            for (float y = min.y; y < max.y;)
            {
                for (float z = min.z; z < max.z;)
                {
                    Vector3 minGrid = new Vector3(x, y, z);
                    Vector3 maxGrid = new Vector3(x + xStep, y + yStep, z + zStep);
                    Grid newGrid = new Grid(minGrid, maxGrid);
                    gridLayout.Add(newGrid);
                    z += zStep;
                }
                y += yStep;
            }
            x += xStep;
        }
    }

    /// <summary>
    /// Draws the grid for visaul
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (gridLayout == null)
        {
            CreateGrid();
        }

        foreach (Grid grid in gridLayout)
        {
            Vector3 min = grid.GetMin();
            Vector3 max = grid.GetMax();
            Vector3 size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
            Gizmos.DrawWireCube(min + (size / 2), size);
        }
    }

    private struct Grid
    {
        private Vector3 min;
        private Vector3 max;

        public Grid(Vector3 a_min, Vector3 a_max)
        {
            min = a_min;
            max = a_max;
        }

        public Vector3 GetMin() { return min; }
        public Vector3 GetMax() { return max; }
    }
}
