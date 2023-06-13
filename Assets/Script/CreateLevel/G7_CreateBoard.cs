using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class G7_CreateBoard : MonoBehaviour
{
    GameObject parent;
    public GameObject tile;
    public Vector3 gridPosition;
    public int row, col;
    public G7_C_TileGenerator tg;
    private void Awake()
    {
        GenerateBoard();
    }
    public void CreateNewBoard()
    {
        parent = new GameObject("Grid");
        parent.transform.SetParent(transform);
    }

    public void GenerateBoard()
    {

        if (parent == null)
            CreateNewBoard();

        parent.transform.position = gridPosition;

        if (!parent.GetComponent<G7_C_TileGenerator>())
            tg = parent.AddComponent<G7_C_TileGenerator>();
        else
            tg = parent.GetComponent<G7_C_TileGenerator>();

        tg.GenerateGrid(tile, row, col);

        tg.AddComponent<G7_C_Block>();
    }
    public string SaveCoordinatesToString()
    {

        string coordinates = "";
        foreach (G7_C_Tile tile in tg.tiles)
        {
            if (tile.gameObject.activeInHierarchy)
            {
                coordinates += $"[{tile.row}, {tile.col}]: " + tile.name;
            }
        }
        return coordinates;
    }
    public bool checkActive()
    {
        foreach (G7_C_Tile tile in tg.tiles)
        {
            if (tile.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
