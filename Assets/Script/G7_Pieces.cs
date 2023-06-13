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

    public List<Tilesss> TileBoard = new List<Tilesss>();
    [Header("Generator")]
    public TileGenerator _TileGenerator;
    public GameObject tile;
    public void addTileBoard(List<Tilesss> tile)
    {
        TileBoard.Clear();
        TileBoard.AddRange(tile);
        if (TileBoard.Count < 0)
        {
            return;
        }
        foreach (Tilesss x in TileBoard)
        {
            x.VisibleSuggest(true);
        }

        G7_MoveBlock.hideBlock();
    }
    public void removeTileBoard()
    {
        if (TileBoard.Count < 0)
        {
            return;
        }
        foreach (Tilesss tile in TileBoard)
        {
            tile.VisibleSuggest(false);
        }
        TileBoard.Clear();
        G7_MoveBlock.showBlock();
    }
    public void Generator(int row, int col, bool[,] action)
    {
        _TileGenerator.GenerateGrid(tile, row, col, action);
    }

}
