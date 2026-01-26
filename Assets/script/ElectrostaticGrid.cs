using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ElectrostaticGrid : MonoBehaviour
{
    public int resolutionX = 200;
    public int resolutionY = 200;

    public float sizeX = 1000f;
    public float sizeY = 1000f;

    public float kConstant = 1f;
    public float softening = 0.5f;
    public float heightScale = 2f;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] gridXZ;
    private float[,] field;

    private IsolineRenderer isolines;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        isolines = new GameObject("Isolines").AddComponent<IsolineRenderer>();
        isolines.transform.SetParent(transform, false);

        BuildGrid();
    }

    void Update()
    {
        UpdateField();
    }

    void BuildGrid()
    {
        int vx = resolutionX + 1;
        int vy = resolutionY + 1;

        vertices = new Vector3[vx * vy];
        gridXZ = new Vector2[vx * vy];
        field = new float[vx, vy];

        int[] tris = new int[resolutionX * resolutionY * 6];

        int i = 0;
        int t = 0;

        for (int y = 0; y <= resolutionY; y++)
        {
            for (int x = 0; x <= resolutionX; x++)
            {
                float px = Mathf.Lerp(
                    -sizeX * 0.5f,
                     sizeX * 0.5f,
                     x / (float)resolutionX
                );

                float pz = Mathf.Lerp(
                    -sizeY * 0.5f,
                     sizeY * 0.5f,
                     y / (float)resolutionY
                );

                gridXZ[i] = new Vector2(px, pz);
                vertices[i] = new Vector3(px, 0f, pz);

                if (x < resolutionX && y < resolutionY)
                {
                    tris[t++] = i;
                    tris[t++] = i + vx;
                    tris[t++] = i + 1;

                    tris[t++] = i + 1;
                    tris[t++] = i + vx;
                    tris[t++] = i + vx + 1;
                }

                i++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

    void UpdateField()
    {
        var charges = ChargeManager.Instance.Charges;
        int i = 0;

        for (int y = 0; y <= resolutionY; y++)
        {
            for (int x = 0; x <= resolutionX; x++)
            {
                float pot = 0f;
                Vector2 p = gridXZ[i];

                foreach (var c in charges)
                {
                    if (!c) continue;

                    Vector2 cp = c.GetLocalXZ(transform);
                    float r = Mathf.Max(Vector2.Distance(p, cp), softening);

                    pot += kConstant * c.charge / r;
                }

                field[x, y] = pot;
                vertices[i].y = pot * heightScale;

                i++;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        isolines.Render(field, resolutionX, resolutionY, gridXZ, heightScale);
    }
}
