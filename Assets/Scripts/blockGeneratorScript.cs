using System;
using System.Collections.Generic;
using UnityEngine;


public class blockGeneratorScript : MonoBehaviour
{
    [Header("Ground Plate Settings")]
    public float tileWidthX = 50f;   // Horizontal size along X axis
    public float tileHeightY = 1f;   // Upward vertical thickness along Y axis
    public float tileLengthZ = 50f;   // Horizontal length along Z axis
    public Material Ground;
    public Material Road;

     [Header("Grid Counts")]
    [SerializeField] public int NumPlatesWideX = 10;
    [SerializeField] public int NumPlatesLongZ = 15;

    private GameObject groundGridContainer;
    private GameObject roadContainer;

    private bool[,] gridOccupation;

    void Start()
    {
        makeCity();
    }
    void makeCity()
    {
        gridOccupation = new bool[NumPlatesWideX, NumPlatesLongZ];
        makeGround();
        makeRoads();
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

                float posX = C * tileWidthX;
                float posY = 0f;
                float posZ = R * tileLengthZ;

                Vector3 platePosition = new Vector3(posX, posY, posZ);
       
                GameObject groundPlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
          
                groundPlate.name = $"GroundPlate_Index_{tileIndex}_(Col_{C},Row_{R})";
                groundPlate.transform.position = platePosition;
                groundPlate.transform.SetParent(groundGridContainer.transform);
                groundPlate.transform.localScale = new Vector3(tileWidthX, tileHeightY, tileLengthZ);
             
                MeshRenderer renderer = groundPlate.GetComponent<MeshRenderer>();
                if (renderer != null && Ground != null)
                {
                    renderer.sharedMaterial = Ground;
                }

                tileIndex++;
            }
        }




    }
    void makeRoads()
    {

        roadContainer = new GameObject("RoadNetwork");

        //float startOffsetValuesX = -(tileWidthX / 2f);
        //float startOffsetValuesZ = -(tileLengthZ / 2f);

        //roadContainer.transform.position = new Vector3(startOffsetValuesX, 0f, startOffsetValuesZ);
        roadContainer.transform.position = Vector3.zero;

        float maxWorldWidthX = (NumPlatesWideX * tileWidthX); 
        float maxWorldLengthZ = (NumPlatesLongZ * tileLengthZ);
        float currentX = 0f;

    

        while (currentX < maxWorldWidthX)
        {
            float roadWidthUnits = UnityEngine.Random.Range(1f, 4f);



            if (currentX + roadWidthUnits <= maxWorldWidthX)// checks for fit inside grid boundries
            {

                float centerPositionX = currentX + (roadWidthUnits / 2f) - (tileWidthX / 2f);
                float centerPositionZ = (maxWorldLengthZ / 2f) - (tileLengthZ / 2f);


                //float centerPositionX = currentX + (roadWidthUnits / 2f);
                //float centerPositionZ = maxWorldLengthZ / 2f;
                Vector3 roadPos = new Vector3(centerPositionX, 0.06f, centerPositionZ);

                float totalRoadLengthZ = NumPlatesLongZ * tileLengthZ;
                Vector3 roadScale = new Vector3(roadWidthUnits, tileHeightY * 1.1f, totalRoadLengthZ);


                SpawnRoadSegment(roadPos, roadScale, $"Lengthwise_Road_{roadWidthUnits:F1}_UnitsWide");
            }


            float spacingBufferX = UnityEngine.Random.Range(20f, 35f);
            currentX += roadWidthUnits + spacingBufferX;
        }



        float currentZ = 0f;

        while (currentZ < maxWorldLengthZ)
        {
            float roadLengthUnits = UnityEngine.Random.Range(1f, 4f);

            if (currentZ + roadLengthUnits <= maxWorldLengthZ)
            {
                float centerPositionX = (maxWorldWidthX / 2f) - (tileWidthX / 2f);
                float centerPositionZ = currentZ + (roadLengthUnits / 2f) - (tileLengthZ / 2f);


                //float centerPositionX = maxWorldWidthX / 2f;
                //float centerPositionZ = currentZ + (roadLengthUnits / 2f);
                
                
                Vector3 roadPos = new Vector3(centerPositionX, 0.07f, centerPositionZ);

                float totalRoadWidthX = NumPlatesWideX * tileWidthX;
                Vector3 roadScale = new Vector3(totalRoadWidthX, tileHeightY * 1.1f, roadLengthUnits);

                SpawnRoadSegment(roadPos, roadScale, $"Widthwise_Road_{roadLengthUnits:F1}_UnitsLong");
            }

            float spacingBufferZ = UnityEngine.Random.Range(15f, 30f);
            currentZ += roadLengthUnits + spacingBufferZ;
        }
    }


    void SpawnRoadSegment(Vector3 position, Vector3 scale, string segmentName)
    {
        GameObject roadSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roadSegment.name = segmentName;
        roadSegment.transform.position = position;
        roadSegment.transform.localScale = scale;
        roadSegment.transform.SetParent(roadContainer.transform);

        if (roadSegment.GetComponent<BoxCollider>() != null)
        {
            Destroy(roadSegment.GetComponent<BoxCollider>());
        }

        MeshRenderer renderer = roadSegment.GetComponent<MeshRenderer>();
        if (renderer != null && Road != null)
        {
            renderer.sharedMaterial = Road;
        }
    }



    public bool IsTileOccupiedByRoad(int c, int r) // checks if tile cell is used for roads  for building layouts
    {
        if (c >= 0 && c < NumPlatesWideX && r >= 0 && r < NumPlatesLongZ)
        {
            return gridOccupation[c, r];
        }
        return false;

    }
}
