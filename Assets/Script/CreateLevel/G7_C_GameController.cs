using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class G7_C_GameController : MonoBehaviour
{
    public static G7_C_GameController instance;
    public G7_CreateBoard G7_CreateBoard;
    public bool canCreateBoard;
    public bool canCreatePieces;
    public List<G7_C_Tile> PiecesTiles;
    public G7_SpawnPieces SpawnPieces;
    private void Awake()
    {
        instance = this;
        canCreateBoard = false;
        canCreatePieces = false;
    }
    public void OnStart()
    {
        canCreateBoard = true;
    }
    public void OnAdjut()
    {

    }
    string coordinates = "";
    public void OnPieces()
    {
        if (PiecesTiles.Count < 0)
        {
            return;
        }
        string pieces = "";
        foreach (G7_C_Tile tile in PiecesTiles)
        {
            pieces += $"[{tile.row}, {tile.col}]: " + tile.name;

        }
        pieces += "|";
        coordinates += pieces;
        SpawnPieces.Create(PiecesTiles, true);
        PiecesTiles.Clear();
        if (G7_CreateBoard.checkActive())
        {
            CreatePieces();
        }
    }
    public void CreatePieces()
    {
        SpawnPieces.Show();
    }
    public void OnSaveBoard()
    {
        Debug.Log(G7_CreateBoard.SaveCoordinatesToString());
        canCreateBoard = false;
        canCreatePieces = true;
    }
}
