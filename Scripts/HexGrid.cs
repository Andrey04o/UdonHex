
using com.cyborgAssets.inspectorButtonPro;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HexGrid : MonoBehaviour
{
    [SerializeReference]
    GameObject hex;
    [SerializeReference]
    Material material;

    //public Canvas canvas;
    //[SerializeReference]
    //HexCreator hexCreator;
    [SerializeReference]
    float outerRadius = 10f;
    float innerRadius = 0f;


    public int width = 6;
    public int height = 6;
    public float padding = 15f;
    public Hex[,] cells;
    public HexGame hexGame;

    void createGrid() {
        cells = new Hex[height,width];
        for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(z, x);
			}
		}
    }
    void CreateGridHex() {
        cells = new Hex[height,width+height];
        for (int x = 0; x < height; x++) {
			for (int y = 0; y < width; y++) {
                int yh = y + x / 2;
                CreateCell(x, yh);
			}
		}
    }
    void CreateCell (int x, int z) {
        #if UNITY_EDITOR
		Vector3 position;
		position.x = x * (outerRadius * 1.5f); 
		position.y = 0;
        position.z = (z  + x * 0.5f - x / 2 ) * (innerRadius * 2f);
        GameObject objj = PrefabUtility.InstantiatePrefab(hex, transform) as GameObject;
        cells[x,z] = objj.GetComponent<Hex>();
        //cells[x,z] = Instantiate(hex, transform, false).GetComponent<Hex>();
        cells[x, z].transform.parent = transform;
        //cells[i].transform.SetParent(transform, false);
        cells[x,z].transform.localPosition = position;
        cells[x,z].position.x = x;
        cells[x,z].position.y = z;
        //HexCreator.CreateHex(cells[x, z].gameObject, material);
        #endif
	}
/*
    public List <Hex> GetNeighbours(int x, int y)
    {
        List <Hex> result = new List<Hex>();
        int x1 = x + 1;
        int x0 = x - 1;
        int y1 = y + 1;
        int y0 = y - 1;

        
        if (y1 < height)
        // ADD (x, y1)
            result.Add(cells[x, y1]);
        
        if (y0 >= 0)
        // ADD (x, y0)
            result.Add(cells[x, y0]);

        if (x1 < width)
        // ADD (x1, y)
            result.Add(cells[x1, y]);
        
        if (x0 >= 0)
        // ADD (x0, y)
            result.Add(cells[x0, y]);


        if (x%2 > 0)
        {
            if (x1 < width && y1 < height)
            // ADD (x1, y1)
                result.Add(cells[x1, y1]);
        
            if (x0 >= 0 && y1 < height)
            // ADD (x0, y1)
                result.Add(cells[x0, y1]);
        }
        else
        {
            if (x1 < width && y0 >= 0)
            // ADD (x1, y0)
                result.Add(cells[x1, y0]);
        
            if (x0 >= 0 && y0 >= 0)
            // ADD (x0, y0)
                result.Add(cells[x0, y0]);
        }
        return result;
    }
    */

    void Start()
    {
        
    }
    [ProButton]
    void Create() {
        innerRadius = outerRadius * 0.866025404f;
        CreateGridHex();
        //Instantiate(hex);
        //CreateCells();
    }

    void CreateCells() {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                HexCreator.CreateHex(cells[i, j].gameObject, material);
            }
        }
    }
    
    [ProButton] public void PushCellsToHexGame() {
        hexGame.hexs = new Hex[height][];
        for (int i = 0; i < height; i++)
        {
            hexGame.hexs[i] = new Hex[width + height];
            for (int j = 0; j < width+height; j++)
            {
                Debug.Log(cells[i, j]);
                hexGame.hexs[i][j] = cells[i, j];
            }
        }
    }

    public void SetNeightbours() {
    }
/*
    public MeshFilter meshFilter;
    public string path;
    [ProButton] public void SaveMesh() {

        AssetDatabase.CreateAsset(meshFilter.sharedMesh, path);
        AssetDatabase.SaveAssets();
    }
*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
