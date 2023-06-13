using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public List<Tilesss> tiles;
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

    public void GenerateGrid(GameObject tile, Vector2Int gridsize)
    {
        tiles = new List<Tilesss>();
        row = gridsize.y;
        col = gridsize.x;

        ClearGrid();
        Vector2 tileSize = DetermineTileSize(tile.GetComponent<MeshFilter>().sharedMesh.bounds);
        Vector3 position = transform.position;

        for (int x = 0; x < gridsize.x; x++)
        {
            for (int y = 0; y < gridsize.y; y++)
            {
                position.x = transform.position.x + tileSize.x * x;
                position.z = transform.position.z + tileSize.y * y;

                position.z += UnevenRowOffset(x, tileSize.y);

                CreateTile(tile, position, new Vector2Int(x, y));
            }
        }
    }
    public void GenerateGrid(GameObject tile, int row, int col, bool[,] action)
    {
        tiles = new List<Tilesss>();
        this.row =row;
        this.col = col;

        ClearGrid();
        Vector2 tileSize = DetermineTileSize(tile.GetComponent<MeshFilter>().sharedMesh.bounds);
        Vector3 position = transform.position;

        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                position.x = transform.position.x + tileSize.x * c;
                position.z = transform.position.z + tileSize.y * r;

                position.z += UnevenRowOffset(c, tileSize.y);

                CreateTile(tile, position, new Vector2Int(c, r), action[r, c]);
            }
        }
    }
    float UnevenRowOffset(float x, float y)
    {
        return x % 2 == 0 ? y / 2 : 0f;
    }

    void CreateTile(GameObject t, Vector3 pos, Vector2Int id, bool action = true)
    {
        GameObject newTile = Instantiate(t.gameObject, pos, Quaternion.identity, transform);
        newTile.name = "Tile " + id.y + "_" + id.x;
        newTile.GetComponent<Tilesss>().row = id.y;
        newTile.GetComponent<Tilesss>().col = id.x;
        tiles.Add(newTile.GetComponent<Tilesss>());
        newTile.SetActive(action);
        Debug.Log("Created a tile!");
    }
}
