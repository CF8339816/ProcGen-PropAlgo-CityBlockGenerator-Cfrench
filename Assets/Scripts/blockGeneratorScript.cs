using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder; // Required for ProBuilder types

public class blockGeneratorScript : MonoBehaviour
{
    [Header("Ground Plate")]
    public float cubeWidth = 50f;
    public float cubeHeight = 50f;
    public float cubeDepth = 1f;
    public int widthSegments = 20;
    public int heightSegments = 20;
    public int depthSegments = 1;
    public Material Ground;



    private ProBuilderMesh GroundPlate;

    void Start()
    {
        makeGround();
       
    }

    void makeGround()
    {
       
        GroundPlate = ShapeGenerator.GenerateCube();

        GroundPlate.gameObject.name = "GroundPlate";

       
        MeshRenderer renderer = GroundPlate.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = Ground != null ? Ground : BuiltinMaterials.defaultMaterial;

        GroundPlate.ToMesh();
        GroundPlate.Refresh();
        GroundPlate.gameObject.AddComponent<MeshCollider>();
    }

   

  
}