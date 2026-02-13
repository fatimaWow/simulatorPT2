using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ElectrostaticGrid : MonoBehaviour
{
    public int resolutionX = 50;
    public int resolutionY = 50;

    public float sizeX = 100f;
    public float sizeY = 100f;

    public float kConstant = 1f;
    public float softening = 0.5f;
    public float heightScale = 2f;

    private Mesh mesh;
    private Vector3[] vertices;
    private float[] oldVertices;
    private Vector2[] gridXZ;
    private float[,] field;
    private float[,] oldField;
    bool allChargesReachMax = false;
    
    public Material mat;

    public ChargeManager manager;

    float _interval = 2f;

    float _time;

    float animTime = 1f;

    // float deltaCharge = 0;


    private IsolineRenderer isolines;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        _time = 0f;
        isolines = new GameObject("Isolines").AddComponent<IsolineRenderer>();
        isolines.transform.SetParent(transform, false);

        Renderer isolineRend = isolines.GetComponent<Renderer>();

       isolineRend.material = mat;


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

    void repeat()
    {
        if (manager.run == true && allChargesReachMax == false)
        {
            
            UpdateField();
            Debug.Log("repeat called");
        }
       
    }

    public void reset()
    {
        for(int i = manager.charges.Count - 1; i > 1; i--)
        {
            manager.charges[i].destroySelf();
        }
        BuildGrid();
       // manager.charges
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

                    pot += kConstant * c.charge / r;
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

            //if (c.charge > 0)
            //{
            //    if (c.deltacharge < c.charge)
            //    {
            //        c.deltacharge += 0.2f;
            //    }
            //    else
            //    {
            //        c.maxReached = true;
            //    }

            //}
            //else
            //{
            //    if (c.deltacharge > c.charge)
            //    {
            //        c.deltacharge -= 0.2f;
            //    }
            //    else
            //    {
            //        c.maxReached = true;
            //    }
            //}
           // Debug.Log(c.deltacharge);
        }

       

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        isolines.Render(field, resolutionX, resolutionY, gridXZ, heightScale);
    }
}
