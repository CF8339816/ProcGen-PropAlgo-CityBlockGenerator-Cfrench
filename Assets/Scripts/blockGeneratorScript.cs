using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder; // Required for ProBuilder types
using UnityEngine.VFX;


public class blockGeneratorScript : MonoBehaviour


{
    [Header("Ground Plate")]
    public float cubeWidth = 50f; //x axis width
    public float cubeHeight = 1f;  // y axis height (cause it felt akward previously referring to it as depth in the maths)
    public float cubeDepth = 50f; // z axis depth(lenght)
    public int widthSegments = 20;
    public int heightSegments = 1;
    public int depthSegments = 20;
    public Material Ground;

    [SerializeField] public int NumPlatesWideX = 4;
    [SerializeField] public int NumPlatesLongY = 3;

    private GameObject groundGridContainer;

    void Start()
    {
        makeGround();
    }

    void makeGround()
    {
        float GroundArea = NumPlatesWideX * NumPlatesLongY;// defines total plates required for requested surface area
        groundGridContainer = new GameObject("GroundGrid"); // creates the 'Parent container object the tiles will be spawned in 
        groundGridContainer.transform.position = Vector3.zero;// gives this container a location for the loop to generate from

        Vector3 groundSize = new Vector3(cubeWidth, cubeDepth, cubeHeight);

        for (int i = 0; i < GroundArea; i++)
        //for (int x = 0; x < NumPlatesWideX; x++) // loops and spawns the plates needed for the area requested
        //{
        //    for (int z = 0; z < NumPlatesLongY; z++)  // calculates  local positions for plates being generated per plate
        //                                              // placing them  from the centeral plate outward in a raidius around
        //                                              // it to fill the area
        {

            int gridX = i % NumPlatesWideX;// provides tile's position in the column
            int gridZ = i / NumPlatesWideX;// provided tile's current posiotion in the row

            float posX = gridX * cubeWidth; // provides location coordinates based on colume position
            float posZ = gridZ * cubeDepth;// provides location position based on row position
            //float posX = x * cubeWidth;
            //    float posZ = z * cubeDepth;
            Vector3 platePosition = new Vector3(posX, 0f, posZ);

                ProBuilderMesh groundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, groundSize);// generates tile

                groundPlate.gameObject.name = $"GroundPlate_Index_{i}_({gridX},{gridZ})"; // provides unique name for each tile 
                groundPlate.gameObject.transform.position = platePosition;// places  tile in world
                groundPlate.gameObject.transform.SetParent(groundGridContainer.transform);// Sets tiles to parent

            MeshRenderer renderer = groundPlate.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

            groundPlate.ToMesh();
                groundPlate.Refresh();
                groundPlate.gameObject.AddComponent<MeshCollider>();
            }
        //}
    }
}


#region old code
//public class blockGeneratorScript : MonoBehaviour
//{


//    [Header("Ground Plate")]
//    public float cubeWidth = 50f;
//    public float cubeDepth = 50f;
//    public float cubeHeight = 1f;
//    public int widthSegments = 20;
//    public int depthSegments = 20;
//    public int heightSegments = 1;
//    public Material Ground;
//    [SerializeField] public int NumPlatesWideX;
//    [SerializeField] public int NumPlatesLongY;
//    private GameObject ground;

//    [Header("Roadway")]
//    public float RoadWidthMin = 1f;
//    public float RoadDepthMin = 1f;
//    public float RoadWidthMax = 6f;
//    public float RoadDepthMax = 18f;
//    public float RoadHeight = 1f;
//    public int RoadwidthSegments;
//    public int RoaddepthSegments;
//    public int RoadheightSegments;
//    public Material Road;

//    private GameObject road;

//    [Header("Building")]

//    public float BuildWidthMin = 1f;
//    public float BuildDepthMin = 1f;
//    public float BuildHeightMin = 1f;
//    public float BuildWidthMax = 6f;
//    public float BuildDepthMax = 18f;
//    public float BuildHeightMax = 6f;
//    public int BuildwidthSegments;
//    public int BuilddepthSegments;
//    public int BuildheightSegments;
//    public Material Build;

//    private GameObject build;

//    private ProBuilderMesh GroundPlate;

//    void Start()
//    {
//        makeMap();

//    }
//    void makeMap()
//    {
//        int NumberOfPlatesNeeded = NumPlatesLongY * NumPlatesWideX;


//        makeGround();



//    }
//    void makeGround()
//    {
//        Vector3 groundSize = new Vector3(cubeWidth, cubeHeight, cubeDepth);
//        GroundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, groundSize);

//        GroundPlate.gameObject.name = "GroundPlate";

//        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
//        renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

//        GroundPlate.ToMesh();
//        GroundPlate.Refresh();
//        GroundPlate.gameObject.AddComponent<MeshCollider>();
//    }

//    void makeRoads()
//    {
//        float RoadWidth = Random.Range(RoadWidthMin, RoadWidthMax);
//        float RoadDepth = Random.Range(RoadDepthMin, RoadDepthMax);


//        Vector3 RoadSize = new Vector3(RoadWidth, RoadDepth, RoadHeight);
//        GroundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, RoadSize);

//        GroundPlate.gameObject.name = "RoadWay";

//        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
//        renderer.sharedMaterial = Road != null ? Road : BuiltinMaterials.defaultMaterial;

//        GroundPlate.ToMesh();
//        GroundPlate.Refresh();
//        GroundPlate.gameObject.AddComponent<MeshCollider>();

//    }

//    void makeBuildings()
//    {

//        //if (RoadWay = null)
//        //{
//        float BuildWidth = Random.Range(BuildWidthMin, BuildWidthMax);
//        float BuildHeight = Random.Range(BuildHeightMin, BuildHeightMax);
//        float BuildDepth = Random.Range(BuildDepthMin, BuildDepthMax);

//        Vector3 groundSize = new Vector3(BuildWidth, BuildHeight, BuildDepth);
//        GroundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, groundSize);

//        GroundPlate.gameObject.name = "Building";

//        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
//        renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

//        GroundPlate.ToMesh();
//        GroundPlate.Refresh();
//        GroundPlate.gameObject.AddComponent<MeshCollider>();
//        //}

//    }
//}
#endregion