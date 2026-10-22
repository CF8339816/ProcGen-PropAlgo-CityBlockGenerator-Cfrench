using System;
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
        roadContainer.transform.position = Vector3.zero;

        int currentColumn = UnityEngine.Random.Range(0, 3); // randomize starting column edge offset

        while (currentColumn < NumPlatesWideX)
        {
            int roadWidthInTiles = UnityEngine.Random.Range(1, 5); // 1 to 4 tiles wide

           
            if (currentColumn + roadWidthInTiles <= NumPlatesWideX) // checks for fit inside grid boundries
            {
                
                for (int c = currentColumn; c < currentColumn + roadWidthInTiles; c++)// maps out paths along the z axis orientation for directional alignment
                {
                    for (int r = 0; r < NumPlatesLongZ; r++)
                    {
                        gridOccupation[c, r] = true; // marks tile cell for roads
                    }
                }

                
                SpawnRoadVisuals(currentColumn, 0, roadWidthInTiles, NumPlatesLongZ, "Length_Road");// renders the road block
            }

         
            currentColumn += roadWidthInTiles + UnityEngine.Random.Range(2, 5);// steps to next rando location
        }

    
        int currentRow = UnityEngine.Random.Range(0, 3); // randomises starting row edge offset

        while (currentRow < NumPlatesLongZ)
        {
            int roadLengthInTiles = UnityEngine.Random.Range(1, 5); 

            if (currentRow + roadLengthInTiles <= NumPlatesLongZ)
            {
              
                for (int r = currentRow; r < currentRow + roadLengthInTiles; r++)  // maps road paths on x axis
                {
                    for (int c = 0; c < NumPlatesWideX; c++)
                    {
                        gridOccupation[c, r] = true; // reserved for roads
                    }
                }

                
                SpawnRoadVisuals(0, currentRow, NumPlatesWideX, roadLengthInTiles, "Width_Road");// renders roads
            }

            
            currentRow += roadLengthInTiles + UnityEngine.Random.Range(2, 5);// steps to next with ofsets

        }
    }

    void SpawnRoadVisuals(int startCol, int startRow, int colSpan, int rowSpan, string roadLabel)
    {
       
        float startX = startCol * tileWidthX;
        float startZ = startRow * tileLengthZ;

        float totalWidthX = colSpan * tileWidthX;
        float totalLengthZ = rowSpan * tileLengthZ;

      
        float midX = startX + (totalWidthX / 2f) - (tileWidthX / 2f);
        float midZ = startZ + (totalLengthZ / 2f) - (tileLengthZ / 2f);

       
        Vector3 roadPosition = new Vector3(midX, 0.05f, midZ);
        Vector3 roadScale = new Vector3(totalWidthX, tileHeightY, totalLengthZ);

        GameObject roadSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roadSegment.name = $"{roadLabel}_(Col_{startCol}_to_{startCol + colSpan - 1},Row_{startRow}_to_{startRow + rowSpan - 1})";
        roadSegment.transform.position = roadPosition;
        roadSegment.transform.localScale = roadScale;
        roadSegment.transform.SetParent(roadContainer.transform);

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
