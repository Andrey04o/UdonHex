
using System.Collections.Generic;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HexCreator : MonoBehaviour
{
    [SerializeReference]
    Material material;
    [SerializeReference]
    public const float outerRadius = 10f;
    [SerializeReference]
	public const float innerRadius = outerRadius * 0.866025404f;
    Mesh mesh;
    MeshCollider meshCollider;
    //Vector3[] vertices = new Vector3[4];
    //Vector2[] uv = new Vector2[4];
    int[] triangles = new int[12];

    public static Vector3[] corners = {
		new Vector3( outerRadius, 0f, 0f),
		new Vector3(0.5f * outerRadius, 0f, innerRadius),
		new Vector3(-0.5f * outerRadius, 0f, innerRadius),
		new Vector3(-outerRadius, 0f, 0f),
		new Vector3(-0.5f * outerRadius, 0f, -innerRadius),
		new Vector3(0.5f * outerRadius, 0f, -innerRadius)
	};

    public static Vector2[] uv = {
		new Vector2(outerRadius, 0f),
		new Vector2( 0.5f * outerRadius, innerRadius),
		new Vector2(-0.5f * outerRadius, innerRadius),
		new Vector2(-outerRadius, 0f),
		new Vector2(-0.5f * outerRadius, -innerRadius),
		new Vector2(0.5f * outerRadius, -innerRadius)
	};
    void Start()
    {

    }

    static public void CreateHex(GameObject gameObject, Material material) {
        Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        mesh.Clear();

        int[] triangles = new int[12];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        triangles[3] = 2;
        triangles[4] = 4;
        triangles[5] = 3;

        triangles[6] = 4;
        triangles[7] = 0;
        triangles[8] = 5;

        triangles[9] = 0;
        triangles[10] = 4;
        triangles[11] = 2; 

        mesh.vertices = corners;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshCollider.sharedMesh = mesh;
        //meshCollider.convex = true;
    } 
    /*
    public static List <Hex> GetNeighbours(Hex hex, Hex[][] grid)
    {
        List <Hex> result = new List<Hex>();
        int x = (int)hex.position.x;
        int y = (int)hex.position.y;
        int height = grid.Length -1;
        int width = grid[0].Length -1;
        int x1 = x + 1;
        int x0 = x - 1;
        int y1 = y + 1;
        int y0 = y - 1;

        void AddHex(Hex hex1) {
            if (hex1 != null) {
                result.Add(hex1);
            }
        }

        if (y1 < height)
        // ADD (x, y1)
            AddHex(grid[x][y1]);
        
        if (y0 >= 0)
        // ADD (x, y0)
            AddHex(grid[x][y0]);

        if (x1 < width)
        // ADD (x1, y)
            AddHex(grid[x1][y]);
        
        if (x0 >= 0)
        // ADD (x0, y)
            AddHex(grid[x0][y]);


        if (x%2 > 0)
        {
            if (x1 < width && y1 < height)
            // ADD (x1, y1)
                AddHex(grid[x1][y1]);
        
            if (x0 >= 0 && y1 < height)
            // ADD (x0, y1)
                AddHex(grid[x0][y1]);
        }
        else
        {
            if (x1 < width && y0 >= 0)
            // ADD (x1, y0)
                AddHex(grid[x1][y0]);
        
            if (x0 >= 0 && y0 >= 0)
            // ADD (x0, y0)
                AddHex(grid[x0][y0]);
        }
        return result;
    }
    */

    public static Vector3 GetPosition(int x, int z) {
        Vector3 position;
		position.x = x * (outerRadius * 1.5f); 
		position.y = 0;
        position.z = (z  + x * 0.5f - x / 2 ) * (innerRadius * 2f);
        return position;
    }

    public static Vector3 GetPositionRelative(Hex hex, Vector3 position) {
        return hex.transform.position + position * hex.transform.lossyScale.x;
    }

    public static Hex[] GetNeighbours(Hex hex) {
        //0 1, 0 2, 1 0, 1 1 - center, 1 2, 2 1, 2 2
        

        Hex GetHex(int x, int y) {
            Vector3 v = GetPosition(x, y);
            Vector3 vRel = GetPositionRelative(hex, v);
            //Handles.DrawLine(vRel, vRel + (Vector3.down * 100.5f), 0.2f);
        
            RaycastHit raycastHit;
            vRel.y = 0.4f;
            if (Physics.Raycast(vRel, Vector3.down, out raycastHit, 0.8f)) {
                Debug.Log("touch " + raycastHit.transform.gameObject.name + " pos " + raycastHit.point + " distance " + raycastHit.distance);
                
                Hex hex1;
                if (raycastHit.transform.TryGetComponent<Hex>(out hex1)) {
                    Debug.Log("found, pos is, " + hex.position);
                    return hex1;
                }
            }
            Debug.DrawLine(vRel, vRel + (Vector3.down * 30f), Color.green, 2f);
            return null;
        }
        List<Hex> hexes = new List<Hex>(6);
        Hex h = null;
        void AddHex(int x, int y) {
            h = GetHex(x, y);
            if (h != null) {
                hexes.Add(h);
            }
        }

        AddHex(-1, 0);
        AddHex(-1, 1);
        AddHex(0, -1);
        AddHex(0, 1);
        AddHex(1, -1);
        AddHex(1, 0);
        
        return hexes.ToArray();

        //for (int i = 0; i < 6; i++) {
        //    
        //}
        //    Physics.Raycast(hex.transform.position, Vector3.down, 0.2f);
    }
    
}
