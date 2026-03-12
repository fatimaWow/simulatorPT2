
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ElectrostaticGrid : MonoBehaviour
{
    public int resolutionX = 50;
    public int resolutionY = 50;

    public float sizeX = 100f;
    public float sizeY = 100f;

    public float kConstant = 1f;
    public float softening = 1.5f;
    public float heightScale = 2f;

    private Mesh mesh;
    private Vector3[] vertices;
    private float[] oldVertices;
    private Vector2[] gridXZ;
    private float[,] field;
    private float[,] oldField;
    bool allChargesReachMax = false;
    
    public Material isolinematerial;

    public ChargeManager manager;

  

    float animTime = 1f;


    // float deltaCharge = 0;


    private IsolineRenderer isolines;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        isolines = new GameObject("Isolines").AddComponent<IsolineRenderer>();
        isolines.transform.SetParent(transform, false);

        Renderer isolineRend = isolines.GetComponent<Renderer>();

       isolineRend.material = isolinematerial;


        //InvokeRepeating("repeat", 0, .2f);

        BuildGrid();

      
    }

    void Update()
    {

    }

    void FixedUpdate()   // animation
    {
        if (ChargeManager.detectChange)
        {
          
            ChargeManager.detectChange = false;
            animTime = 0;
            for (int y = 0; y <= resolutionY; y++)
            {
                for (int x = 0; x <= resolutionX; x++)
                {
                    oldField[x, y] = field[x, y];
                }
            }
            for (int i = 0; i < vertices.Length; i++) oldVertices[i] = vertices[i].y;
        }       
            if (animTime >= 1f) return;
            animTime = Mathf.Min(1f, animTime + Time.fixedDeltaTime * 0.5f);
        
     
        UpdateField();
    }

    //void repeat()
    //{
    //    if (manager.run == true && allChargesReachMax == false)
    //    {
            
    //        UpdateField();
    //        Debug.Log("repeat called");
    //    }
       
    //}

    public void reset()
    {
        for (int i = ChargeManager.charges.Count - 1; i > 1; i--)
        {
            ChargeManager.charges[i].destroySelf();
        }

        // isolines.mesh.Clear(); // reset isoline

        //BuildGrid();

        ChargeManager.detectChange = true;
    }
   
    void BuildGrid()
    {
        int vx = resolutionX + 1;
        int vy = resolutionY + 1;

        vertices = new Vector3[vx * vy];
        oldVertices = new float[vx * vy];
        gridXZ = new Vector2[vx * vy];
        field = new float[vx, vy];
        oldField = new float[vx, vy];
        
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

        MeshCollider meshCol = GetComponent<MeshCollider>();
        if (meshCol)
        {
            meshCol.sharedMesh = null;       // Clear first to force update
            meshCol.sharedMesh = mesh;       // Assign the updated mesh
        }
    }

    public void UpdateField()
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

                    pot += kConstant * ((c.charge)) / r;
                }

                field[x, y] = Mathf.Lerp(oldField[x, y], pot, 1 - (1 - animTime) * (1 - animTime));
                vertices[i].y = Mathf.Lerp(oldVertices[i], pot * heightScale, 1 - (1 - animTime) * (1 - animTime));

                i++;
            }
        }

        foreach (var c in charges)
        {
            if (c.maxReached == true)
            {
                allChargesReachMax = true;
            }
            else{
                allChargesReachMax = false;
            }

            if(allChargesReachMax == true)
            {
                manager.run = false;
                allChargesReachMax = false;
                Debug.Log("run state: "+ manager.run);
                Debug.Log("chargesMax state:" + allChargesReachMax);
            }

          

        }

       

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        MeshCollider meshCol = GetComponent<MeshCollider>();
        if (meshCol)
        {
            meshCol.sharedMesh = null;       // Clear first to force update
            meshCol.sharedMesh = mesh;       // Assign the updated mesh
        }

        isolines.Render(field, resolutionX, resolutionY, gridXZ, heightScale);
    }


    // Samples the electric potential V at any point on the grid
    public bool TrySamplePotential(Vector3 worldPoint, out float V)
    {
        V = 0f;
        if (field == null) return false; // Return false if the field hasn't been initialized

        // Convert world position to local grid coordinates
        Vector3 local = transform.InverseTransformPoint(worldPoint);

        // Normalize local coordinates to [0,1] based on grid size
        float u = (local.x + sizeX * 0.5f) / sizeX; // X as fraction across grid
        float v = (local.z + sizeY * 0.5f) / sizeY; // Z as fraction across grid

        // Return false if point is outside the grid bounds
        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return false;

        // Convert normalized coordinates to float grid indices
        float gx = u * resolutionX;
        float gy = v * resolutionY;

        // Find the four grid vertices surrounding the point
        int x0 = Mathf.Clamp(Mathf.FloorToInt(gx), 0, resolutionX);
        int y0 = Mathf.Clamp(Mathf.FloorToInt(gy), 0, resolutionY);
        int x1 = Mathf.Clamp(x0 + 1, 0, resolutionX);
        int y1 = Mathf.Clamp(y0 + 1, 0, resolutionY);

        // Fractional position inside the cell for interpolation
        float tx = gx - x0;
        float ty = gy - y0;

        // Get the potential values at the four surrounding vertices
        float v00 = field[x0, y0]; // bottom-left
        float v10 = field[x1, y0]; // bottom-right
        float v01 = field[x0, y1]; // top-left
        float v11 = field[x1, y1]; // top-right

        // Bilinear interpolation
        float vx0 = Mathf.Lerp(v00, v10, tx); // interpolate along X at bottom
        float vx1 = Mathf.Lerp(v01, v11, tx); // interpolate along X at top
        V = Mathf.Lerp(vx0, vx1, ty);         // interpolate along Y between bottom & top

        return true; // Successfully sampled potential
    }

    // Samples the electric field vector E at any point on the grid
    public bool TrySampleField(Vector3 worldPoint, out Vector2 E)
    {
        E = Vector2.zero;
        if (field == null) return false; // Return false if field not initialized

        // Convert world position to local grid coordinates
        Vector3 local = transform.InverseTransformPoint(worldPoint);

        // Normalize local coordinates to [0,1]
        float u = (local.x + sizeX * 0.5f) / sizeX;
        float v = (local.z + sizeY * 0.5f) / sizeY;

        // Return false if outside grid
        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return false;

        // Convert normalized coordinates to nearest grid index
        float gx = u * resolutionX;
        float gy = v * resolutionY;

        // Pick a vertex but avoid edges because we need neighbors for derivative
        int x = Mathf.Clamp(Mathf.RoundToInt(gx), 1, resolutionX - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(gy), 1, resolutionY - 1);

        // Compute grid spacing
        float dx = sizeX / resolutionX;
        float dz = sizeY / resolutionY;

        // Approximate partial derivatives using central differences
        float dVdx = (field[x + 1, y] - field[x - 1, y]) / (2f * dx); // ∂V/∂x
        float dVdz = (field[x, y + 1] - field[x, y - 1]) / (2f * dz); // ∂V/∂z

        // Electric field is negative gradient
        float Ex = -dVdx;
        float Ez = -dVdz;

        E = new Vector2(Ex, Ez); // Return 2D electric field on XZ plane
        return true; // Successfully sampled electric field
    }
}
