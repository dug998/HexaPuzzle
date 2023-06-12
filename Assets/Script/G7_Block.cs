using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class G7_Block : MonoBehaviour
{
    private TileGenerator childBlock;
    public G7_Node[,] nodeBlock;
    protected int row, col;
    public int index = 1;
    public int numberBlockChild = 0;
    private void Awake()
    {
        childBlock = GetComponent<TileGenerator>();
        Create();
    }
    public void Create()
    {
        row = childBlock.row;
        col = childBlock.col;
        nodeBlock = new G7_Node[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {

                SetValuesNode(i, j);
            }
        }
    }
    public void SetValuesNode(int row, int col)
    {

        List<Tile> tile = childBlock.tiles.Where(x => x.row == row && x.col == col).ToList();
        if (tile.Count > 0 && tile != null && tile[0].gameObject.activeInHierarchy)
        {
            nodeBlock[row, col] = new G7_Node(typeNode.empty, tile[0]);
            numberBlockChild++;
            tile.Clear();
            return;
        }

        nodeBlock[row, col] = new G7_Node(typeNode.none, null);
    }

}
