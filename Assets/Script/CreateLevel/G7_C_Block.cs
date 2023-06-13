using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_C_Block : MonoBehaviour
{
    protected G7_C_TileGenerator childBlock;
    public G7_Node[,] nodeBlock;
    protected int row, col;
    public int index = 1;
    public int numberBlockChild = 0;
    protected void Awake()
    {
        childBlock = GetComponent<G7_C_TileGenerator>();
    }

}
