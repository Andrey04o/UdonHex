using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
#endif
namespace Hex04o {
    public class HexGrid : MonoBehaviour {
        [SerializeReference] GameObject hex;
        [SerializeReference] Material material;
        [SerializeReference] float outerRadius = 10f;
        float innerRadius = 0f;
        public int width = 6;
        public int height = 6;
        public float padding = 15f;
        public Hex[,] cells;
        public HexGame hexGame;
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
            #if !COMPILER_UDONSHARP && UNITY_EDITOR
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

        public void Create() {
            innerRadius = outerRadius * 0.866025404f;
            CreateGridHex();
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
        
        public void PushCellsToHexGame() {
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
    }
}