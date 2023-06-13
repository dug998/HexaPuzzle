using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G7_C_CanvasController : MonoBehaviour
{
    public Button btnSaveBoard;
    public Button btnPieces;
    public Button btnAdjut;
    public Button btnStart;
    public void Awake()
    {
        btnSaveBoard.onClick.AddListener(OnSaveBoard);
        btnPieces.onClick.AddListener(OnPieces);
        btnAdjut.onClick.AddListener(OnAdjut);
        btnStart.onClick.AddListener(OnStart);

    }
    private void OnStart()
    {
        G7_C_GameController.instance.OnStart();
    }
    private void OnAdjut()
    {
        G7_C_GameController.instance.OnAdjut();
    }

    private void OnPieces()
    {
        G7_C_GameController.instance.OnPieces();
    }

    private void OnSaveBoard()
    {
        G7_C_GameController.instance.OnSaveBoard();
    }
}
