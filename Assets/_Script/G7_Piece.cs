using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Piece : MonoBehaviour
{
    public int id;
    public List<G7_Tile> tiles = new List<G7_Tile>();

    public void Start()
    {
        foreach (G7_Tile tile in tiles)
        {
            tile.setSprite(G7_Resources.instance.tileSprites[id]);
        }
    }
}
