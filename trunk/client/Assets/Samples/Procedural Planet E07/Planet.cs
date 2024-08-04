using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum FaceRenderMask
    {
        All,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    [Range(2, 256)] public int resolution = 10;

    public bool autoUpdate = true;
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector] public bool shapeSettingsFoldout;

    [HideInInspector] public bool colourSettingsFoldout;

    [SerializeField] [HideInInspector] private MeshFilter[] meshFilters;

    private readonly ColourGenerator colourGenerator = new();

    private readonly ShapeGenerator shapeGenerator = new();
    private TerrainFace[] terrainFaces;


    private void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (meshFilters == null || meshFilters.Length == 0) meshFilters = new MeshFilter[6];
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        for (var i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                var meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            var renderFace = faceRenderMask == FaceRenderMask.All || (int) faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    private void GenerateMesh()
    {
        for (var i = 0; i < 6; i++)
            if (meshFilters[i].gameObject.activeSelf)
                terrainFaces[i].ConstructMesh();

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    private void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (var i = 0; i < 6; i++)
            if (meshFilters[i].gameObject.activeSelf)
                terrainFaces[i].UpdateUVs(colourGenerator);
    }
}