using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class G7_Piece2 : G7_Piece
{
    private void Start()
    {
        foreach (G7_Tile tile in tiles)
        {
            tile.GetComponent<Image>().sprite = G7_Resources.instance.tileSprites[id % G7_Resources.instance.tileSprites.Length];
        }
    }
    public new void OnDrag(PointerEventData eventData)
    {
        if (status != Status.Dragging) return;

        Vector3 moveDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - beginTouchPosition;
        transform.position = beginPosition + moveDelta;

        G7_TileRegion2.instance.CheckMatch(this);
    }
    public new void OnPointerDown(PointerEventData eventData)
    {
        beginPosition = transform.position;
        beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.SetParent(G7_Resources.instance.dragRegion);

        status = Status.Dragging;

        matches.Clear();
    }
    public new void OnEndDrag(PointerEventData eventData)
    {
        if (matches.Count == tiles.Count)
        {
            transform.DOMove(matches[0].transform.position, 10f).SetSpeedBased().OnComplete(() =>
            {
                CompleteMoveToBoard();
            });
           
        }
        else
        {
            transform.DOScale(Vector3.one * G7_Const.SCALED_TILE, 4).SetSpeedBased();

            transform.DOMove(beginPosition, 15).SetSpeedBased().OnComplete(() =>
            {
                CompleteMoveToBottom();
            });
        }

        status = Status.OnTween;
    }
    private void CompleteMoveToBoard()
    {
        status = Status.OnBottom;
        transform.SetParent(G7_Resources.instance.pieceRegion);
        if (matches.Count != 0) boardPositions.Clear();
        foreach (var tile in matches)
        {
            boardPositions.Add(tile.position);
        }

        G7_MakeLevelManager.instance.AdjustPieces();
    }

    private void CompleteMoveToBottom()
    {
        status = Status.OnBottom;
        transform.SetParent(G7_Resources.instance.pieceRegion);
    }
}
