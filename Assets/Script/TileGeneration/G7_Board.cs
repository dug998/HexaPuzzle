using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class G7_Board : G7_Block
{
    public List<G7_MoveBlock> games;
    public List<Tile> TilesSuggest;
    public bool CheckBoard(G7_MoveBlock moveBlock)
    {
        G7_Node[,] node = moveBlock.block.nodeBlock;
        for (int i = 0; i < node.GetLength(0); i++)
            for (int j = 0; j < node.GetLength(1); j++)
            {
                int row = i + moveBlock.posRow;
                int col = j + moveBlock.posCol;
                // col lẻ
                if (moveBlock.posCol % 2 == 1 && col % 2 == 0)
                {
                    row -= 1;
                }
                if (col < 0 || col >= this.col)
                {
                    return false;
                }

                //col chẵn 
                if (col < 0 || col >= this.col)
                {
                    return false;
                }
                if (row < 0 || row >= this.row)
                {
                    return false;
                }

                if (this.nodeBlock[row, col].statusNode == typeNode.full || this.nodeBlock[row, col].statusNode == typeNode.none)
                    return false;
            }

        return true;
    }
    public void CheckVisibleSuggest(G7_MoveBlock moveBlock)
    {
        HideVisibleSuggest();
        index = moveBlock.block.index;

        G7_Node[,] node = moveBlock.block.nodeBlock;
        for (int i = 0; i < node.GetLength(0); i++)
        {
            for (int j = 0; j < node.GetLength(1); j++)
            {
                int row = i + moveBlock.posRow;
                int col = j + moveBlock.posCol;
                // col lẻ 
                if (moveBlock.posCol % 2 == 1 && col % 2 == 0)
                {
                    row -= 1;
                }
                // col chăn
                if (col < 0 || col >= this.col)
                {
                    break;
                }
                if (row < 0 || row >= this.row)
                {
                    break;
                }

                if (this.nodeBlock[row, col].statusNode == typeNode.full || this.nodeBlock[row, col].statusNode == typeNode.none)
                {
                    break;
                }
                Debug.Log(row + "__" + col);
                AddTileSuggest(this.nodeBlock[row, col].obj, index);
            }
        }
        ShowVisibleSuggest(moveBlock.block.numberBlockChild);
    }

    //public int CheckCol(int col)
    //{
    //    if (col % 2 == 1)
    //    {

    //    }
    //}
    public void ShowVisibleSuggest(int numberBlock)
    {
        if (TilesSuggest.Count < 0)
        {
            return;
        }
        if (numberBlock != TilesSuggest.Count)
        {
            HideVisibleSuggest();
            return;
        }
        foreach (Tile tile in TilesSuggest)
        {
            tile.VisibleSuggest(true);
        }

    }
    public void HideVisibleSuggest()
    {
        if (TilesSuggest.Count < 0)
        {
            return;
        }
        foreach (Tile tile in TilesSuggest)
        {
            tile.VisibleSuggest(false);
        }
        TilesSuggest.Clear();

    }
    public void AddTileSuggest(Tile tile, int index)
    {
        tile.index = index;
        if (TilesSuggest.Contains(tile))
        {
            return;
        }
        TilesSuggest.Add(tile);
    }

    public void AddObject(G7_MoveBlock moveBlock)
    {
        G7_Node[,] node = moveBlock.block.nodeBlock;
        for (int i = 0; i < node.GetLength(0); i++)
            for (int j = 0; j < node.GetLength(1); j++)
            {
                this.nodeBlock[i + moveBlock.posRow, j + moveBlock.posCol].statusNode = typeNode.full;
            }
        moveBlock.transform.position -= G7_GameController.instance.offset;
        games.Add(moveBlock);
    }
    public void RemoveObject(G7_MoveBlock moveBlock)
    {
        G7_Node[,] node = moveBlock.block.nodeBlock;

        for (int i = 0; i < node.GetLength(0); i++)
            for (int j = 0; j < node.GetLength(1); j++)
            {

                this.nodeBlock[i + moveBlock.posRow, j + moveBlock.posCol].statusNode = typeNode.empty;

            }
        games.Remove(moveBlock);
    }
}
