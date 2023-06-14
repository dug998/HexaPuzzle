using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Resources : MonoBehaviour
{
    public G7_Tile tile_background, tile_background2, tile_background_bottom, tile_shadow, tile_stone;
    public G7_Tile tile_normal;
  //  public Piece piecePrefab;
  //  public Piece2 piece2Prefab;
    public Transform dragRegion, bottomRegion, pieceRegion;
    public Sprite[] tileSprites;
    public Transform hintPiecesTransform, backgroundTilesTransform, piecesTransform, piecesBottomTransform,
        bottomRegionBGTransform, highlightsTransform;

    public static G7_Resources instance;

    private void Awake()
    {
        instance = this;
    }
}
