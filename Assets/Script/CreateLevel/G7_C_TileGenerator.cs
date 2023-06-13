using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_C_TileGenerator : MonoBehaviour
{
    public List<G7_C_Tile> tiles;
    public int row;
    public int col;
    void ClearGrid()
    {
        for (int i = transform.childCount; i >= transform.childCount; i--)
        {
            if (transform.childCount == 0)
                break;

            int c = Mathf.Clamp(i - 1, 0, transform.childCount);
            DestroyImmediate(transform.GetChild(c).gameObject);
        }
    }

    Vector2 DetermineTileSize(Bounds tileBounds)
    {
        return new Vector2((tileBounds.extents.x * 2) * 0.75f, (tileBounds.extents.z * 2));
    }

    public void GenerateGrid(GameObject tile, int row, int col)
    {
        tiles = new List<G7_C_Tile>();
        this.row = row;
        this.col = col;

        ClearGrid();
        Vector2 tileSize = DetermineTileSize(tile.GetComponent<MeshFilter>().sharedMesh.bounds);
        Vector3 position = transform.position;

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                position.x = transform.position.x + tileSize.x * x;
                position.z = transform.position.z + tileSize.y * y;

                position.z += UnevenRowOffset(x, tileSize.y);

                CreateTile(tile, position, new Vector2Int(x, y));
            }
        }
    }

    float UnevenRowOffset(float x, float y)
    {
        return x % 2 == 0 ? y / 2 : 0f;
    }

    void CreateTile(GameObject t, Vector3 pos, Vector2Int id)
    {
        GameObject newTile = Instantiate(t.gameObject, pos, Quaternion.identity, transform);
        newTile.name = "Tile " + id.y + "_" + id.x;
        newTile.GetComponent<G7_C_Tile>().row = id.y;
        newTile.GetComponent<G7_C_Tile>().col = id.x;
        tiles.Add(newTile.GetComponent<G7_C_Tile>());
      //  Debug.Log("Created a tile!");
    }
}
