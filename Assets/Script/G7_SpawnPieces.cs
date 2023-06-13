using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_SpawnPieces : MonoBehaviour
{
    public GameObject PrefPieces;
    public List<Node> listNode = new List<Node>();
    public void Show()
    {
        gameObject.SetActive(true);

    }
    public void Create(List<G7_C_Tile> PiecesTiles, bool values)
    {


        foreach (G7_C_Tile tile in PiecesTiles)
        {
            Node node = new Node((int)tile.row, (int)tile.col, values);
            listNode.Add(node);
        }
        GameObject obj = Instantiate(PrefPieces);
        obj.transform.SetParent(transform);
        bool[,] matrix = matrixBlock(listNode);
        obj.GetComponent<G7_Pieces>().Generator(matrix.GetLength(0), matrix.GetLength(1), matrix);
    }

    public bool[,] matrixBlock(List<Node> nodes)
    {
        int maxRow = MaxRow(nodes);
        int maxCol = MaxCol(nodes);
        int minRow = MinRow(nodes);
        int minCol = MinCol(nodes);

        bool[,] matrix = new bool[7, 7];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = false;

        }
        foreach (Node node in nodes)
        {
            matrix[node.row, node.col] = node.values;
        }
        listNode.Clear();
        return matrix;
    }
    public int MaxRow(List<Node> nodes)
    {
        int max = int.MinValue;
        foreach (Node node in nodes)
        {
            if (node.row > max)
            {
                max = node.row;
            }
        }
        return max;
    }
    public int MinRow(List<Node> nodes)
    {
        int min = int.MaxValue;
        foreach (Node node in nodes)
        {
            if (node.row < min)
            {
                min = node.row;
            }
        }
        return min;
    }
    public int MaxCol(List<Node> nodes)
    {
        int max = int.MinValue;
        foreach (Node node in nodes)
        {
            if (node.col > max)
            {
                max = node.col;
            }
        }
        return max;
    }
    public int MinCol(List<Node> nodes)
    {
        int min = int.MaxValue;
        foreach (Node node in nodes)
        {
            if (node.col < min)
            {
                min = node.col;
            }
        }
        return min;
    }


}
[System.Serializable]
public class Node
{
    public int row;
    public int col;
    public bool values;
    public Node(int row, int col, bool values)
    {
        this.row = row;
        this.col = col;
        this.values = values;
    }
}
