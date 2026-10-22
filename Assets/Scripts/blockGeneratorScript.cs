using System;
using UnityEngine;


public class blockGeneratorScript : MonoBehaviour
{
    [Header("Ground Plate Settings")]
    public float cubeWidth = 50f;   // Horizontal size along X axis
    public float cubeHeight = 1f;   // Upward vertical thickness along Y axis
    public float cubeDepth = 50f;   // Horizontal length along Z axis
    public Material Ground;

    [SerializeField] public int NumPlatesWideX = 4;
    [SerializeField] public int NumPlatesLongZ = 3;

    private GameObject groundGridContainer;

    void Start()
    {
        makeGround();
    }

    void makeGround()
    {
       
        groundGridContainer = new GameObject("GroundGrid");
        groundGridContainer.transform.position = Vector3.zero;

        int tileIndex = 0;

        
        for (int R = 0; R < NumPlatesLongZ; R++)
        {
            for (int C = 0; C < NumPlatesWideX; C++)
            {
                
                float posX = C * cubeWidth;
                float posY = 0f;
                float posZ = R * cubeDepth;
                Vector3 platePosition = new Vector3(posX, posY, posZ);

              
                GameObject groundPlate = GameObject.CreatePrimitive(PrimitiveType.Cube);

               
                groundPlate.name = $"GroundPlate_Index_{tileIndex}_(Col_{C},Row_{R})";
                groundPlate.transform.position = platePosition;
                groundPlate.transform.SetParent(groundGridContainer.transform);

                groundPlate.transform.localScale = new Vector3(cubeWidth, cubeHeight, cubeDepth);

               
                MeshRenderer renderer = groundPlate.GetComponent<MeshRenderer>();
                if (renderer != null && Ground != null)
                {
                    renderer.sharedMaterial = Ground;
                }

                tileIndex++;
            }
        }
    }
}
