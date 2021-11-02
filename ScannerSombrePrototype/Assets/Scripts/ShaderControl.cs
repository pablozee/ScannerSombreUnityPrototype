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
    private const string  minLimitsRefernce = "MinLimits";
    private const string maxLimitsRefernce = "MaxLimits";

    #endregion
    private Vector3 minLimits;
    private Vector3 maxLimits;
    private Vector3 colliderSize;
    private List<Grid> gridLayout;

    [SerializeField]
    private float gridSize = 5;

    [SerializeField]
    private GameObject cube;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        float angle = Random.Range(ANGLE_RANGE_MIN, ANGLE_RANGE_MAX);

       minLimits = new Vector3(1, 1,1);
       maxLimits = new Vector3(0,0,0);

        material.SetVector(minLimitsRefernce, minLimits);
        material.SetVector(maxLimitsRefernce, maxLimits);

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
    public void Hitted(Vector3 pos)
    {
        CheckGrid(pos);

        material.SetFloat(visablityRefernce, 1);

    }

    /// <summary>
    /// Determiens which partition of the model to display
    /// </summary>
    /// <param name="pos"></param>
    private void CheckGrid(Vector3 pos)
    {
        foreach (Grid square in gridLayout)
        {
            //Check bounds
            //X
            if (pos.x >= square.GetMin().x && pos.x <= square.GetMax().x)
            {
                //Y
                if (pos.y >= square.GetMin().y && pos.y <= square.GetMax().y)
                {
                    //Z
                    if (pos.z >= square.GetMin().z && pos.z <= square.GetMax().z)
                    {
                        UpdateVisable(square);
                    }
                }


            }

        }
    }

    private void UpdateVisable(Grid square)
    {
        bool changed = false;
        //Check min
        Vector3 min = ConvertToUV(square.GetMax());


        if (min.x < minLimits.x)
        {
            minLimits.x = min.x;
            changed = true;
        }
        if (min.y < minLimits.y)
        {
            minLimits.y = min.y;
            changed = true;
        }
        if (min.z < minLimits.y)
        {
            minLimits.y = min.z;
            changed = true;
        }

        //Check min
        Vector3 max = ConvertToUV(square.GetMin());
        if (max.x > maxLimits.x)
        {
            maxLimits.x = max.x;
            changed = true;
        }
        if (max.y > maxLimits.y)
        {
            maxLimits.y = max.y;
            changed = true;
        }
        if (max.z > maxLimits.x)
        {
            maxLimits.y = max.z;
            changed = true;
        }


        if (changed)
        {
            material.SetVector(minLimitsRefernce, minLimits);
            material.SetVector(maxLimitsRefernce, maxLimits);
        }
    }

    private Vector3 ConvertToUV(Vector3 pos)
    {
        Vector3 uv = pos;
        uv += colliderSize / 2;
        
        uv.x /= colliderSize.x;
        uv.y /= colliderSize.y;
        uv.z /= colliderSize.z;

        uv.x = 1 - uv.x;
        uv.y = 1 - uv.y;
        uv.z = 1 - uv.z;
        Debug.Log(uv);


        return uv;
    }

    /// <summary>
    /// Create the grid
    /// </summary>
    private void CreateGrid()
    {
        gridLayout = new List<Grid>();
        //Get min pos
        BoxCollider box = GetComponent<BoxCollider>();
        colliderSize = box.size;
        Vector3 center = box.center;

        Vector3 min = center - (colliderSize / 2);
        Vector3 max = center + (colliderSize / 2);
   
        min = transform.TransformPoint(min);
        max = transform.TransformPoint(max);

        //Get world size
        colliderSize = max - min;

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
      //  if (gridLayout == null)
       // {
            CreateGrid();
      //  }

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
