using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Tile : MonoBehaviour
{
    public const float EDGE_SIZE = 42;
    public const float WIDTH = 2f * EDGE_SIZE;
    public const float HEIGHT = WIDTH * 0.866f;

    public enum Type { Background, Normal, Stone };
    public Type type = Type.Normal;
    public bool isActive = true;
    public SpriteRenderer icon;
    public Vector2 position, boardPosition;

    public G7_Piece piece;

    public void setSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
