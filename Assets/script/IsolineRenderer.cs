using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IsolineRenderer : MonoBehaviour
{
    public float minLevel = -10f;
    public float maxLevel = 10f;
    public int lineCount = 20;


    private Mesh mesh;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

       // if (!IsolineLabelManager.Instance)
          //  new GameObject("IsolineLabelManager").AddComponent<IsolineLabelManager>();
    }

    public void Render(
        float[,] field,
        int resX,
        int resY,
        Vector2[] gridXZ,
        float heightScale
    )
    {
        List<Vector3> verts = new();
        List<int> inds = new();

    //    IsolineLabelManager.Instance.ClearLabels();

        for (int l = 0; l < lineCount; l++)
        {
            float iso = Mathf.Lerp(minLevel, maxLevel, l / (float)(lineCount - 1));
            bool labelPlaced = false;

            for (int y = 0; y < resY; y++)
                for (int x = 0; x < resX; x++)
                {
                    float c0 = field[x, y];
                    float c1 = field[x + 1, y];
                    float c2 = field[x + 1, y + 1];
                    float c3 = field[x, y + 1];

                    int caseID =
                        (c0 > iso ? 1 : 0) |
                        (c1 > iso ? 2 : 0) |
                        (c2 > iso ? 4 : 0) |
                        (c3 > iso ? 8 : 0);

                    if (caseID == 0 || caseID == 15) continue;

                    int id(int xx, int yy) => yy * (resX + 1) + xx;

                    Vector3 p0 = new(gridXZ[id(x, y)].x, c0 * heightScale, gridXZ[id(x, y)].y);
                    Vector3 p1 = new(gridXZ[id(x + 1, y)].x, c1 * heightScale, gridXZ[id(x + 1, y)].y);
                    Vector3 p2 = new(gridXZ[id(x + 1, y + 1)].x, c2 * heightScale, gridXZ[id(x + 1, y + 1)].y);
                    Vector3 p3 = new(gridXZ[id(x, y + 1)].x, c3 * heightScale, gridXZ[id(x, y + 1)].y);

                    Vector3 L(float a, float b, Vector3 A, Vector3 B)
                    {
                        float t = Mathf.Abs(b - a) < 1e-6f ? 0.5f : (iso - a) / (b - a);
                        return Vector3.Lerp(A, B, t);
                    }

                    Vector3 e0 = L(c0, c1, p0, p1);
                    Vector3 e1 = L(c1, c2, p1, p2);
                    Vector3 e2 = L(c3, c2, p3, p2);
                    Vector3 e3 = L(c0, c3, p0, p3);

                    Vector3 A = e0, B = e1;
                    if (caseID == 1 || caseID == 14) { A = e0; B = e3; }
                    else if (caseID == 3 || caseID == 12) { A = e3; B = e1; }
                    else if (caseID == 4 || caseID == 11) { A = e1; B = e2; }
                    else if (caseID == 7 || caseID == 8) { A = e3; B = e2; }

                    int i = verts.Count;
                    verts.Add(A);
                    verts.Add(B);
                    inds.Add(i);
                    inds.Add(i + 1);

                    if (!labelPlaced)
                    {
                     //   IsolineLabelManager.Instance.AddLabel((A + B) * 0.5f, iso);
                        labelPlaced = true;
                    }
                }
        }

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetIndices(inds, MeshTopology.Lines, 0);
    }
}