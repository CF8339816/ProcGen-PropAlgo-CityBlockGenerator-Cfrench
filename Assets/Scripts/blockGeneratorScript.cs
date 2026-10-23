using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class blockGeneratorScript : MonoBehaviour
{
    [Header("Ground Plate Settings")]
    public float tileWidthX = 50f;   // Horizontal size along X axis
    public float tileHeightY = 1f;   // Upward vertical thickness along Y axis
    public float tileLengthZ = 50f;   // Horizontal length along Z axis
    public float buildingBufferSpace = 1f;// spacing between buildings
    public Material Ground;
    public Material Road;
    public Material BuildingMat;
    [Header("Grid Counts")]
    [SerializeField] public int NumPlatesWideX = 1;
    [SerializeField] public int NumPlatesLongZ = 1;
    public TMP_InputField InputFieldWideInputField;
    public TMP_InputField InputFieldlongInputField;

    private GameObject groundGridContainer;
    private GameObject roadContainer;
    private GameObject buildingContainer;
    private bool[,] roadLocations;

    void Start()
    {
        onMakeCity();
    }
  public void SetCityBlocksWide(string inputValue)
    {
        string cleanInput = inputValue.Replace("\u200B", "").Trim();

        if (int.TryParse(cleanInput, out int result))
        {
            NumPlatesWideX = Mathf.Clamp(result, 1, 100);
            Debug.Log($"[SUCCESS] NumPlatesWideX changed in memory to: {NumPlatesWideX}");
        }
        else
        {
            Debug.LogWarning($"[FAIL] Unable to parse '{inputValue}'. Clean length: {cleanInput.Length}");
        }
    }

    public void SetCityBlockslong(string inputValue)
    {
        string cleanInput = inputValue.Replace("\u200B", "").Trim();

        if (int.TryParse(cleanInput, out int result))
        {
            NumPlatesLongZ = Mathf.Clamp(result, 1, 100);
            Debug.Log($"[SUCCESS] NumPlatesLongZ changed in memory to: {NumPlatesLongZ}");
        }
        else
        {
            Debug.LogWarning($"[FAIL] Unable to parse '{inputValue}'. Clean length: {cleanInput.Length}");
        }
    }
    public void onMakeCity()
    {
        if (groundGridContainer != null) { Destroy(groundGridContainer); }
        if (roadContainer != null) { Destroy(roadContainer); }
        if (buildingContainer != null) { Destroy(buildingContainer); }
        roadLocations = new bool[NumPlatesWideX, NumPlatesLongZ];
        makeGround();
        makeRoads();
       makeBuildings();
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
        roadContainer = new GameObject("Roadways");

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

                Vector3 roadPos = new Vector3(centerPositionX, 0.06f, centerPositionZ);

                float totalRoadLengthZ = NumPlatesLongZ * tileLengthZ;
                Vector3 roadScale = new Vector3(roadWidthUnits, tileHeightY * 1.1f, totalRoadLengthZ);

                SpawnRoadSegment(roadPos, roadScale, $"Lengthwise_Road_{roadWidthUnits:F1}_UnitsWide");

                int startTileX = Mathf.Clamp(Mathf.FloorToInt(currentX / tileWidthX), 0, NumPlatesWideX - 1);
                int endTileX = Mathf.Clamp(Mathf.FloorToInt((currentX + roadWidthUnits) / tileWidthX), 0, NumPlatesWideX - 1);

                for (int c = startTileX; c <= endTileX; c++)
                {
                    for (int r = 0; r < NumPlatesLongZ; r++)
                    {
                        roadLocations[c, r] = true; // Lock out the tile from building allocation
                    }
                }
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

                Vector3 roadPos = new Vector3(centerPositionX, 0.07f, centerPositionZ);

                float totalRoadWidthX = NumPlatesWideX * tileWidthX;
                Vector3 roadScale = new Vector3(totalRoadWidthX, tileHeightY * 1.1f, roadLengthUnits);

                SpawnRoadSegment(roadPos, roadScale, $"Widthwise_Road_{roadLengthUnits:F1}_UnitsLong");

                int startTileZ = Mathf.Clamp(Mathf.FloorToInt(currentZ / tileLengthZ), 0, NumPlatesLongZ - 1);
                int endTileZ = Mathf.Clamp(Mathf.FloorToInt((currentZ + roadLengthUnits) / tileLengthZ), 0, NumPlatesLongZ - 1);

                for (int r = startTileZ; r <= endTileZ; r++)
                {
                    for (int c = 0; c < NumPlatesWideX; c++)
                    {
                        roadLocations[c, r] = true; // Lock out the tile from building allocation
                    }
                }
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
    void makeBuildings()
    {
        buildingContainer = new GameObject("BuildingZone");// makes container for buildings
        buildingContainer.transform.position = Vector3.zero;
        float maxWorldWidthX = NumPlatesWideX * tileWidthX;
        float maxWorldLengthZ = NumPlatesLongZ * tileLengthZ;

        for (float checkX = buildingBufferSpace; checkX < maxWorldWidthX - buildingBufferSpace; checkX += 6f)
        {
            for (float checkZ = buildingBufferSpace; checkZ < maxWorldLengthZ - buildingBufferSpace; checkZ += 6f)
            {
                float buildWidthX = UnityEngine.Random.Range(1.5f, 4.5f);
                float buildLengthZ = UnityEngine.Random.Range(1.5f, 4.5f);

                float buildingCenterX = checkX + (buildWidthX / 2f);
                float buildingCenterZ = checkZ + (buildLengthZ / 2f);
                
                bool positionIsOnRoad = IsCoordinateOverlappingARoad(buildingCenterX, buildingCenterZ);

                if (!positionIsOnRoad)
                {
                    float footprintArea = buildWidthX * buildLengthZ;
                    float normalizedArea = (footprintArea - 1f) / (25f - 1f);
                    float buildHeightY = Mathf.Lerp(3f, 25f, normalizedArea);

                    float targetCenterY = (buildHeightY / 2f) + (tileHeightY / 2f);
                    Vector3 finalizedBuildingPosition = new Vector3(buildingCenterX, targetCenterY, buildingCenterZ);

                    GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    building.name = $"Building_At_({buildingCenterX:F1},{buildingCenterZ:F1})";
                    building.transform.position = finalizedBuildingPosition;
                    building.transform.localScale = new Vector3(buildWidthX, buildHeightY, buildLengthZ);
                    building.transform.SetParent(buildingContainer.transform);

                    if (building.GetComponent<BoxCollider>() != null)
                    {
                        Destroy(building.GetComponent<BoxCollider>());
                    }

                    MeshRenderer renderer = building.GetComponent<MeshRenderer>();
                    if (renderer != null && BuildingMat != null)
                    {
                        renderer.sharedMaterial = BuildingMat;
                    }
                }
                checkZ += buildLengthZ + buildingBufferSpace;
            }
        }
    }
    bool IsCoordinateOverlappingARoad(float x, float z)
    {
        if (roadContainer == null) return false;

        foreach (Transform roadSegment in roadContainer.transform)
        {
            Vector3 pos = roadSegment.position;
            Vector3 scale = roadSegment.localScale;

            float minX = pos.x - (scale.x / 2f) - buildingBufferSpace;
            float maxX = pos.x + (scale.x / 2f) + buildingBufferSpace;
            float minZ = pos.z - (scale.z / 2f) - buildingBufferSpace;
            float maxZ = pos.z + (scale.z / 2f) + buildingBufferSpace;

            if (x >= minX && x <= maxX && z >= minZ && z <= maxZ)
            {
                return true;
            }
        }
        return false; 
    }
 





    //    for (int r = 0; r < NumPlatesLongZ; r++)
    //    {
    //        for (int c = 0; c < NumPlatesWideX; c++)
    //        {                
    //            if (roadLocations[c, r]) continue;// skips tile if in a road zone

    //            float tileLeftEdgeX = c * tileWidthX;
    //            float tileBottomEdgeZ = r * tileLengthZ;

    //            float localStartX = tileLeftEdgeX + buildingBufferSpace;
    //            float localStartZ = tileBottomEdgeZ + buildingBufferSpace;
    //            float localEndX = tileLeftEdgeX + tileWidthX - buildingBufferSpace;
    //            float localEndZ = tileBottomEdgeZ + tileLengthZ - buildingBufferSpace;

    //            for (float checkX = localStartX; checkX < localEndX; checkX += 6f)// spawns  building clusters as it steps through the full base area map
    //            {
    //                for (float checkZ = localStartZ; checkZ < localEndZ; checkZ += 6f)
    //                {                       
    //                    float buildWidthX = UnityEngine.Random.Range(1f, 5f);// generates random building width
    //                    float buildLengthZ = UnityEngine.Random.Range(1f, 5f);// generates random building length

    //                    if (checkX + buildWidthX > localEndX || checkZ + buildLengthZ > localEndZ) continue;// ensures no overhanging buildings

    //                    float buildingCenterX = checkX + (buildWidthX / 2f);
    //                    float buildingCenterZ = checkZ + (buildLengthZ / 2f);

    //                    int evaluatedGridX = Mathf.Clamp(Mathf.FloorToInt((checkX + (buildWidthX / 2f)) / tileWidthX), 0, NumPlatesWideX - 1);//checks width for road 
    //                    int evaluatedGridZ = Mathf.Clamp(Mathf.FloorToInt((checkZ + (buildLengthZ / 2f)) / tileLengthZ), 0, NumPlatesLongZ - 1);// checks length for road

    //                    if (!roadLocations[evaluatedGridX, evaluatedGridZ])
    //                    {
    //                        float footprintArea = buildWidthX * buildLengthZ;           // these lines  sedt up  a randomized height that is soft capped based
    //                        float normalizedArea = (footprintArea - 1f) / (25f - 1f);   // on the dimensions of the building bases  smaller cap shorter
    //                        float buildHeightY = Mathf.Lerp(2f, 20f, normalizedArea);   // larger cap higher in their ranges

    //                        float targetCenterY = (buildHeightY / 2f) + (tileHeightY / 2f);
    //                        Vector3 finalizedBuildingPosition = new Vector3(buildingCenterX, targetCenterY, buildingCenterZ);

    //                        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                        building.name = $"Building_(Col_{c},Row_{r})_GridX{evaluatedGridX}_GridZ{evaluatedGridZ}";
    //                        building.transform.position = finalizedBuildingPosition;

    //                        building.transform.localScale = new Vector3(buildWidthX, buildHeightY, buildLengthZ);
    //                        building.transform.SetParent(buildingContainer.transform);

    //                        if (building.GetComponent<BoxCollider>() != null)
    //                        {
    //                            Destroy(building.GetComponent<BoxCollider>());
    //                        }

    //                        MeshRenderer renderer = building.GetComponent<MeshRenderer>();
    //                        if (renderer != null && BuildingMat != null)
    //                        {
    //                            renderer.sharedMaterial = BuildingMat;
    //                        }
    //                    }
    //                    checkZ += buildLengthZ + buildingBufferSpace;
    //                }
    //            }
    //        }
    //    }
    //}
}