using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_C_Tile : Tilesss
{
    private void OnMouseDown()
    {
        if (G7_C_GameController.instance.canCreateBoard)
        {
            gameObject.SetActive(false);
        }
        else if (G7_C_GameController.instance.canCreatePieces)
        {
            G7_C_GameController.instance.PiecesTiles.Add(this);

            gameObject.SetActive(false);
        }

    }
}
