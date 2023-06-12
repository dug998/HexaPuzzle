using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Pieces : MonoBehaviour
{
    [Header("Block Move")]
    public GameObject BlockMove;
    public G7_MoveBlock G7_MoveBlock;
    [Header("Block BackGround")]
    public G7_BlockBG G7_BlockBg;

    public List<Tile> TileBoard = new List<Tile>();

    public void addTileBoard(List<Tile> tile)
    {
        TileBoard.Clear();
        TileBoard.AddRange(tile);
        if (TileBoard.Count < 0)
        {
            return;
        }
        foreach (Tile x in TileBoard)
        {
            x.VisibleSuggest(true);
        }
        BlockMove.SetActive(false);
    }
    public void removeTileBoard()
    {
        if (TileBoard.Count < 0)
        {
            return;
        }
        foreach (Tile tile in TileBoard)
        {
            tile.VisibleSuggest(false);
        }
        TileBoard.Clear();
        BlockMove.SetActive(true);
    }

}
