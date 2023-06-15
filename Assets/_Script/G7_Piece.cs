using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class G7_Piece : MonoBehaviour
{
    public int id;
    public Vector2 center;
    public List<Vector2> defaultPositions = new List<Vector2>();
    public List<Vector2> boardPositions = new List<Vector2>();
    public List<Vector2> tilePositions = new List<Vector2>();
    public List<G7_Tile> matches = new List<G7_Tile>();


    public List<G7_Tile> tiles = new List<G7_Tile>();


    public Vector2 bottomPosition;
    public G7_Tile bottomBackground;
    public bool isExtra = false;
    public GameObject shadows;

    public enum Status { Dragging, OnBoard, OnBottom, OnTween };
    public Status status = Status.OnBottom;
    private Vector3 upDelta;
    protected Vector3 beginPosition, beginTouchPosition;
    public G7_Tile tileCenter
    {
        get { return tiles[0]; }
    }
    private void Start()
    {
        foreach (G7_Tile tile in tiles)
        {
            tile.setSprite(G7_Resources.instance.tileSprites[id]);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (status != Status.Dragging) return;
        Vector3 moveDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - beginTouchPosition;
        transform.position = beginPosition + moveDelta + upDelta;

        G7_TileRegion.instance.CheckMatch(this);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (status != Status.OnBoard && status != Status.OnBottom || !G7_GameState.canPlay) return;

        beginPosition = transform.position;
        beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        upDelta = GetUpDelta();

        transform.DOScale(Vector3.one, 0.06f);
        transform.DOMove(beginPosition + upDelta, 0.06f);
        transform.SetParent(G7_Resources.instance.dragRegion);

        if (status == Status.OnBoard)
        {
            G7_TileRegion.instance.ClearCovers(this);
        }
        status = Status.Dragging;

        matches.Clear();
        boardPositions.Clear();
        UpdatePiece();

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (status != Status.Dragging) return;
        // n?u v?a ?? trên board
        if (matches.Count == tiles.Count)
        {
            transform.DOMove(matches[0].transform.position, 10).SetSpeedBased().OnComplete(() =>
            {
                CompleteMoveToBoard();
            });
        }
        else
        {
            transform.DOScale(Vector3.one * G7_Const.SCALED_TILE, 4).SetSpeedBased();

            transform.DOMove(bottomBackground.transform.position, 15).SetSpeedBased().OnComplete(() =>
            {
                CompleteMoveToBottom_EndDrag();
            });
        }
        status = Status.OnTween;
        ResetMatchColor();
    }
    public void MoveToBottom()
    {
        // reset l?i 
        if (status == Status.OnBoard)
        {
            transform.SetParent(G7_Resources.instance.dragRegion);
            G7_TileRegion.instance.ClearCovers(this);
            transform.DOScale(Vector3.one * G7_Const.SCALED_TILE, 4).SetSpeedBased();
            transform.DOMove(bottomBackground.transform.position, 20f).SetSpeedBased().OnComplete(() =>
            {
                CompleteMoveToBottom();
            });
            status = Status.OnTween;
        }
    }
    private void CompleteMoveToBoard()
    {
        status = Status.OnBoard;

        transform.SetParent(G7_Resources.instance.piecesTransform);

        foreach (var tile in matches)
        {
            tile.hasCover = true;
            boardPositions.Add(tile.position);
        }
        UpdateTileBoardPosition();

        G7_TileRegion.instance.CheckGameComplete();

        UpdatePiece();
    }
    public void UpdateTileBoardPosition()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].boardPosition = boardPositions[i];
        }
    }

    private Vector3 GetUpDelta()
    {
        float minY = float.MaxValue;
        foreach (var tile in tiles)
        {
            minY = Mathf.Min(minY, tile.transform.position.y);
        }

        return Vector3.up * (beginTouchPosition.y + 0.3f - minY);
    }
    private void CompleteMoveToBottom_EndDrag()
    {
        CompleteMoveToBottom();
        //  Sound.instance.Play(Sound.Others.OnBottom);
    }
    private void CompleteMoveToBottom()
    {
        status = Status.OnBottom;
        transform.SetParent(G7_Resources.instance.piecesBottomTransform);
        UpdatePiece();
        FadeIn();
    }
    public void FadeIn()
    {
        foreach (G7_Tile tile in tiles) tile.GetComponent<Image>().canvasRenderer.SetAlpha(1);
    }
    public void UpdatePiece()
    {
        shadows.SetActive(status == Status.OnBottom);
    }
    public void ResetMatchColor()
    {
        foreach (Transform highlight in G7_Resources.instance.highlightsTransform)
        {
            if (highlight.gameObject.activeSelf)
                G7_GameManager.instance.GetComponent<G7_Pooler>().Push(highlight.gameObject);
        }
    }
    public void HighlightMatchColor()
    {
        foreach (G7_Tile tile in matches)
        {
            GameObject highlight = G7_GameManager.instance.GetComponent<G7_Pooler>().GetPooledObject();
            highlight.transform.SetParent(G7_Resources.instance.highlightsTransform);
            highlight.transform.localScale = Vector3.one;
            highlight.transform.position = tile.transform.position;
        }
    }
}

