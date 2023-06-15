using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class G7_TileRegion2 : MonoBehaviour
{
    private Dictionary<Vector2, G7_Tile> slots = new Dictionary<Vector2, G7_Tile>();
    private Dictionary<Vector2, G7_Tile> bottomSlots = new Dictionary<Vector2, G7_Tile>();
    public List<G7_Piece> pieces = new List<G7_Piece>();

    public static G7_TileRegion2 instance;
    private G7_GameLevel gameLevel;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadBottomBackground()
    {
        Color color = Color.white;
        color.a = 0.4f;
        for (int col = 0; col < 17; col++)
        {
            for (int row = 0; row < 7; row++)
            {
                G7_Tile tile = Instantiate(G7_Resources.instance.tile_background);
                tile.transform.SetParent(G7_Resources.instance.bottomRegion);
                tile.transform.localScale = Vector3.one * G7_Const.SCALED_TILE;
                Vector3 position = GetLocalPosition(col, row);
                tile.transform.localPosition = position * G7_Const.SCALED_TILE;
                tile.position = new Vector2(col, row);
                tile.transform.GetComponent<Image>().color = color;
                tile.type = G7_Tile.Type.Background;
                bottomSlots.Add(tile.position, tile);
            }
        }
    }
    public void LoadBoardBackground()
    {
        Transform root = G7_Resources.instance.backgroundTilesTransform;

        foreach (Transform child in root)
        {
            DestroyImmediate(child.gameObject);
        }

        slots.Clear();

        gameLevel = G7_MakeLevelManager.instance.gameLevel;
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

        if (string.IsNullOrEmpty(gameLevel.positions))
        {
            for (int col = 0; col < G7_MakeLevelManager.instance.numCol; col++)
                for (int row = 0; row < G7_MakeLevelManager.instance.numRow; row++)
                {
                    G7_Tile tile = Instantiate(G7_Resources.instance.tile_background);
                    tile.transform.SetParent(G7_Resources.instance.backgroundTilesTransform);
                    tile.transform.localScale = Vector3.one;
                    Vector3 pos = GetLocalPosition(col, row);
                    tile.transform.localPosition = pos;

                    tile.position = new Vector2(col, row);
                    tile.transform.GetChild(0).GetComponent<TMP_Text>().text = col + "," + row;
                    tile.type = G7_Tile.Type.Background;

                    if (pos.x < minX) minX = pos.x;
                    if (pos.x > maxX) maxX = pos.x;
                    if (pos.y < minY) minY = pos.y;
                    if (pos.y > maxY) maxY = pos.y;
                }
        }
        else
        {
            List<string> positions = G7_Utils.BuildListFromString<string>(gameLevel.positions);
            foreach (var value in positions)
            {
                string[] values = value.Split(',');
                int col = int.Parse(values[0]);
                int row = int.Parse(values[1]);

                G7_Tile tile = Instantiate(values.Length == 2 ? G7_Resources.instance.tile_background : G7_Resources.instance.tile_stone);
                tile.transform.SetParent(G7_Resources.instance.backgroundTilesTransform);
                tile.transform.localScale = Vector3.one;
                Vector3 position = GetLocalPosition(col, row);
                tile.transform.localPosition = position;
                tile.position = new Vector2(col, row);
                //tile.type = Tile.Type.Background;

                if (values.Length == 2)
                {
                    tile.transform.GetChild(0).GetComponent<TMP_Text>().text = col + "," + row;
                    slots.Add(tile.position, tile);
                }

                if (position.x < minX) minX = position.x;
                if (position.x > maxX) maxX = position.x;
                if (position.y < minY) minY = position.y;
                if (position.y > maxY) maxY = position.y;

            }
        }

        float regionWidth = maxX - minX + G7_Tile.WIDTH + (minX - G7_Tile.WIDTH / 2) * 2; ;
        float regionHeight = maxY - minY + G7_Tile.HEIGHT + (minY - G7_Tile.HEIGHT / 2) * 2;

        transform.localPosition = new Vector3(0, 163) - new Vector3(regionWidth / 2, regionHeight / 2);
        GetComponent<RectTransform>().sizeDelta = new Vector2(regionWidth, regionHeight);
    }
    public void LoadPieces(G7_GameLevel gameLevel)
    {
        ClearPieces();

        List<string> data = G7_Utils.BuildListFromString<string>(gameLevel.pieces);
        int id = 0;
        foreach (var oneData in data)
        {
            List<string> positions = G7_Utils.BuildListFromString<string>(oneData, '-');
            Vector2 bottomPosition = Vector2.zero;

            List<Vector2> defaultPositions = new List<Vector2>();

            bool isExtra = positions[positions.Count - 1] == "r";

            if (isExtra)
                positions.RemoveAt(positions.Count - 1);

            for (int i = 0; i < positions.Count; i++)
            {
                string[] values = positions[i].Split(',');
                int col = int.Parse(values[0]);
                int row = int.Parse(values[1]);
                Vector2 position = new Vector2(col, row);

                if (i == positions.Count - 1)
                {
                    bottomPosition = position;
                }

                if (i != positions.Count - 1)
                {
                    defaultPositions.Add(position);
                }
            }


            float scaleFactor = G7_Const.SCALED_TILE;
            Transform parent = G7_Resources.instance.pieceRegion;

            G7_Piece piece = CreatePiece(defaultPositions, parent);
            piece.boardPositions = GetMatchPositions(piece, bottomPosition);
            piece.isExtra = isExtra;
            pieces.Add(piece);

            piece.id = id++;
            piece.bottomPosition = bottomPosition;

            piece.transform.localScale = Vector3.one * scaleFactor;
            piece.transform.localPosition = GetLocalPosition(bottomPosition) * G7_Const.SCALED_TILE;
        }
    }
    private Vector3 GetLocalPosition(Vector2 position)
    {
        return GetLocalPosition((int)position.x, (int)position.y);
    }
    private G7_Piece CreatePiece(List<Vector2> positions, Transform parent)
    {
        G7_Piece2 piece = Instantiate(G7_Resources.instance.piece2Prefab);
        piece.center = positions[0];
        piece.transform.SetParent(parent);
        piece.defaultPositions.AddRange(positions);

        Vector3 translateVector = GetLocalPosition(piece.center);

        foreach (var position in piece.defaultPositions)
        {
            int col = (int)position.x;
            int row = (int)position.y;

            G7_Tile tile = Instantiate(G7_Resources.instance.tile_normal);
            tile.transform.SetParent(piece.transform);
            tile.transform.localScale = Vector3.one;
            Vector3 localPosition = GetLocalPosition(col, row);
            tile.transform.localPosition = localPosition - translateVector;
            tile.position = new Vector2(col, row);
            tile.piece2 = piece;
            tile.type = G7_Tile.Type.Normal;

            piece.tiles.Add(tile);
        }

        foreach (var pos in piece.defaultPositions)
        {
            piece.tilePositions.Add(pos - positions[0]);
        }

        return piece;
    }
    public void ClearPieces()
    {
        pieces.Clear();
        foreach (Transform child in G7_Resources.instance.pieceRegion)
        {
            Destroy(child.gameObject);
        }
    }

    private Vector3 GetLocalPosition(int col, int row)
    {
        return col % 2 == 0 ? new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 0.5f) * G7_Tile.HEIGHT) :
                        new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 1) * G7_Tile.HEIGHT);
    }
    private List<Vector2> GetMatchPositions(G7_Piece piece, Vector2 centerPosition)
    {
        List<Vector2> matchPositions = new List<Vector2>();
        int deltaX = (int)(centerPosition.x - piece.tileCenter.position.x);
        if (deltaX % 2 == 0)
        {
            matchPositions.AddRange(piece.tilePositions);
        }
        else
        {
            int modifier = centerPosition.x % 2 == 0 ? -1 : 1;
            foreach (var pos in piece.tilePositions)
            {
                float newY = pos.y;
                if (((int)pos.x) % 2 == 1)
                {
                    newY += modifier;
                }
                matchPositions.Add(new Vector2(pos.x, newY));
            }
        }

        for (int i = 0; i < matchPositions.Count; i++) matchPositions[i] += centerPosition;

        return matchPositions;
    }
    public bool CheckMatch(G7_Piece2 piece)
    {
        float minDistance = float.MaxValue;
        G7_Tile matchSlot = null;

        foreach (G7_Tile slot in bottomSlots.Values)
        {
            if (slot.hasCover) continue;
            float distance = Vector3.Distance(piece.tileCenter.transform.position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                matchSlot = slot;
            }
        }
      
        piece.matches = new List<G7_Tile>();
        if (minDistance < 0.2f)
        {
            bool isMatch = true;
            List<Vector2> matchPos = GetMatchPositions(piece, matchSlot.position);

            foreach (var math in matchPos)
            {
                if (!bottomSlots.ContainsKey(math) || bottomSlots[math].hasCover)
                {
                    isMatch = false;
                    break;
                }
                else
                {
                    piece.matches.Add(bottomSlots[math]);
                }
            }
            
            return isMatch;
        }
        return false;
    }
}
