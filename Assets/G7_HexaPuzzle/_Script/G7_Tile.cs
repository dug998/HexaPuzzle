using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class G7_Tile : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public const float EDGE_SIZE = 42;
    public const float WIDTH = 2f * EDGE_SIZE;
    public const float HEIGHT = WIDTH * 0.866f;

    public enum Type { Background, Normal, Stone };
    public Type type = Type.Normal;
    public bool isActive = true;
    public Image icon;
    public Vector2 position, boardPosition;

    public G7_Piece piece;
    public G7_Piece2 piece2;
    public bool hasCover;
    public void setSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }
    private void Start()
    {
        if (G7_Utils.IsMakingLevel())
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (G7_Utils.IsMakingLevel() && type == Type.Background)
        {
            isActive = !isActive;
            SetActive(isActive);
        }
    }
    public void SetActive(bool isActive)
    {
        Color color = Color.white;
        color.a = isActive ? 1 : 0;
        GetComponent<Image>().color = color;
        transform.GetChild(0).GetComponent<TMP_Text>().color = color;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (piece != null) piece.OnDrag(eventData);
        if (piece2 != null) piece2.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        if (piece != null) piece.OnPointerDown(eventData);
        if (piece2 != null) piece2.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (piece != null) piece.OnEndDrag(eventData);
        if (piece2 != null) piece2.OnEndDrag(eventData);
    }
}
