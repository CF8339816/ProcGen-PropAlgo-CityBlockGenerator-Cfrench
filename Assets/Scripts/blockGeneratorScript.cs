using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder; // Required for ProBuilder types

public class blockGeneratorScript : MonoBehaviour
{
  

    [Header("Ground Plate")]
    public float cubeWidth = 50f;
    public float cubeDepth= 50f;
    public float cubeHeight  = 1f;
    public int widthSegments = 20;
    public int  depthSegments = 20;
    public int heightSegments = 1;
    public Material Ground;
    [SerializeField] public int NumPlatesWideX;
    [SerializeField] public int NumPlatesLongY;
    private GameObject ground;

    [Header("Roadway")]
    public float RoadWidthMin = 1f;
    public float RoadDepthMin = 1f;
    public float RoadWidthMax = 6f;
    public float RoadDepthMax = 18f;
    public float RoadHeight = 1f;
    public int RoadwidthSegments;
    public int RoaddepthSegments;
    public int RoadheightSegments;
    public Material Road;

    private GameObject road; 

    [Header("Building")]
   
    public float BuildWidthMin=1f;
    public float BuildDepthMin=1f;
    public float BuildHeightMin=1f;
    public float BuildWidthMax=6f;
    public float BuildDepthMax=18f;   
    public float BuildHeightMax=6f;
    public int BuildwidthSegments;
    public int BuilddepthSegments;
    public int BuildheightSegments;
    public Material Build;

    private GameObject build;

    private ProBuilderMesh GroundPlate;

    void Start()
    {
        makeMap();
       
    }
    void makeMap()
    {
        int NumberOfPlatesNeeded = NumPlatesLongY * NumPlatesWideX;


        makeGround();



    }
    void makeGround()
    {
        Vector3 groundSize= new Vector3(cubeWidth, cubeHeight, cubeDepth );
        GroundPlate = ShapeGenerator.GenerateCube(  PivotLocation.Center, groundSize );

        GroundPlate.gameObject.name = "GroundPlate";
       
        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

        GroundPlate.ToMesh();
        GroundPlate.Refresh();
        GroundPlate.gameObject.AddComponent<MeshCollider>();
    }

    void makeRoads()
    {
        float RoadWidth = Random.Range(RoadWidthMin, RoadWidthMax);
        float RoadDepth = Random.Range(RoadDepthMin, RoadDepthMax);


        Vector3 RoadSize = new Vector3(RoadWidth, RoadDepth, RoadHeight);
        GroundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, RoadSize);

        GroundPlate.gameObject.name = "RoadWay";

        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = Road != null ? Road : BuiltinMaterials.defaultMaterial;

        GroundPlate.ToMesh();
        GroundPlate.Refresh();
        GroundPlate.gameObject.AddComponent<MeshCollider>();

    }

    void makeBuildings()
    {

        if (RoadWay = null)
        {
            float BuildWidth = Random.Range(BuildWidthMin, BuildWidthMax);
            float BuildHeight = Random.Range(BuildHeightMin, BuildHeightMax);
            float BuildDepth = Random.Range(BuildDepthMin, BuildDepthMax);
           
            Vector3 groundSize = new Vector3(BuildWidth, BuildHeight, BuildDepth);
            GroundPlate = ShapeGenerator.GenerateCube(PivotLocation.Center, groundSize);

            GroundPlate.gameObject.name = "Building";

            MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

            GroundPlate.ToMesh();
            GroundPlate.Refresh();
            GroundPlate.gameObject.AddComponent<MeshCollider>();
        }

    }
}