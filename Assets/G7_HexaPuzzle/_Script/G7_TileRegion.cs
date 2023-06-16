using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Purchasing;

public class G7_TileRegion : MonoBehaviour
{
    public static G7_TileRegion instance;
    public Vector2 size;
    public G7_HintPieces hint;
    public Dictionary<Vector2, G7_Tile> slots = new Dictionary<Vector2, G7_Tile>();
    public List<G7_Piece> pieces = new List<G7_Piece>();

    public Stack<G7_Piece> stackPiece = new Stack<G7_Piece>() { };
    private G7_LevelPref levelPrefs;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }
    public void LoadBoard(G7_GameLevel gameLevel)
    {
        levelPrefs = G7_GameManager.instance.levelPrefs;

        List<string> positions = G7_Utils.BuildListFromString<string>(gameLevel.positions);
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

        // Create backGround 2
        foreach (var values in positions)
        {
            string[] value = values.Split(',');
            int col = int.Parse(value[0]);
            int row = int.Parse(value[1]);

            G7_Tile tile = Instantiate(G7_Resources.instance.tile_background2);
            tile.transform.SetParent(G7_Resources.instance.backgroundTilesTransform);
            tile.transform.localScale = Vector3.one;
            Vector3 pos = GetLocalPosition(col, row);
            tile.transform.localPosition = pos;

            tile.position = new Vector2(col, row);
        }
        // Create background 1
        foreach (var values in positions)
        {
            string[] value = values.Split(',');
            int col = int.Parse(value[0]);
            int row = int.Parse(value[1]);


            //  G7_Tile tile = Instantiate(values.Length == 2 ? G7_Resources.instance.tile_background : G7_Resources.instance.tile_stone);
            G7_Tile tile = Instantiate(G7_Resources.instance.tile_background);
            tile.transform.SetParent(G7_Resources.instance.backgroundTilesTransform);
            tile.transform.localScale = Vector3.one;
            Vector3 pos = GetLocalPosition(col, row);
            tile.transform.localPosition = pos;
            tile.position = new Vector2(col, row);

            if (value.Length == 2)
            {
                slots.Add(tile.position, tile);
            }
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;


        }
        float regionWidth = maxX - minX + G7_Tile.WIDTH + (minX - G7_Tile.WIDTH / 2) * 2;
        float regionHeight = maxY - minY + G7_Tile.HEIGHT + (minY - G7_Tile.HEIGHT / 2) * 2;

        transform.localPosition = GetComponent<RectTransform>().localPosition - new Vector3(regionWidth / 2, regionHeight / 2);
        GetComponent<RectTransform>().sizeDelta = new Vector2(regionWidth, regionHeight);
        LoadPieces(gameLevel);


    }
    public void LoadPieces(G7_GameLevel gameLevel)
    {
        List<string> data = G7_Utils.BuildListFromString<string>(gameLevel.pieces);
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

        int id = 0;
        foreach (var one in data)
        {
            // t?o ra Piece

            List<string> positions = G7_Utils.BuildListFromString<string>(one, '-');
            Vector2 bottomPos = Vector2.zero;

            List<Vector2> defaultPos = new List<Vector2>();
            bool isRedundant = positions[positions.Count - 1] == "r";// d? phòng

            if (isRedundant)
            {
                positions.RemoveAt(positions.Count - 1);
            }
            int i = 0;
            for (i = 0; i < positions.Count; i++)
            {
                string[] values = positions[i].Split(',');
                int col = int.Parse(values[0]);
                int row = int.Parse(values[1]);

                Vector2 pos = new Vector2(col, row);

                if (i == positions.Count - 1)
                {
                    bottomPos = pos;
                }
                if (i != positions.Count - 1)
                {
                    defaultPos.Add(pos);
                }

            }
            // check Piece trên hay d??i
            //   G7_PiecePrefs piecePrefs = levelPrefs.piecesPrefs.Find(x => x.id == id);
            G7_PiecePrefs piecePrefs = null;

            bool isOnBoard = piecePrefs != null;
            float scaleFavtor = isOnBoard ? 1 : G7_Const.SCALED_TILE;
            Transform parent = isOnBoard ? G7_Resources.instance.piecesTransform : G7_Resources.instance.piecesBottomTransform;

            G7_Piece piece = CreatePiece(defaultPos, parent);
            piece.isExtra = isRedundant;
            pieces.Add(piece);

          
            piece.setId(id++);
            piece.setSpriteTiles(null);
            piece.bottomPosition = bottomPos;
            piece.transform.localScale = Vector3.one * scaleFavtor;

            if (isOnBoard)
            {

            }
            else
            {
                piece.transform.localPosition = GetLocalPosition(bottomPos) * G7_Const.SCALED_TILE;
            }

            // set v? trí trên board 

            i = 0;
            foreach (var pos in GetMatchPositions(piece, piece.bottomPosition))
            {
                G7_Tile tile = Instantiate(G7_Resources.instance.tile_background_bottom);
                tile.transform.SetParent(G7_Resources.instance.bottomRegionBGTransform);
                tile.transform.localScale = Vector3.one * G7_Const.SCALED_TILE;
                tile.transform.localPosition = GetLocalPosition(pos) * G7_Const.SCALED_TILE;
                tile.position = pos;

                var localPos = tile.transform.localPosition;
                if (localPos.x < minX) minX = localPos.x;
                if (localPos.x > maxX) maxX = localPos.x;
                if (localPos.y < minY) minY = localPos.y;
                if (localPos.y > maxY) maxY = localPos.y;

                if (i == 0) piece.bottomBackground = tile;
                i++;
            }
            

        }
        float regionWidth = maxX - minX + G7_Tile.WIDTH + (minX - G7_Tile.WIDTH / 2) * 2;
        float regionHeight = maxY - minY + G7_Tile.HEIGHT + (minY - G7_Tile.HEIGHT / 2) * 2;

        var bgRT = G7_Resources.instance.bottomRegionBGTransform.GetComponent<RectTransform>();

        bgRT.localPosition = new Vector3(0, 13.7f) - new Vector3(regionWidth / 2, regionHeight / 2);
        bgRT.sizeDelta = new Vector2(regionWidth, regionHeight);

        var piecesRT = G7_Resources.instance.piecesBottomTransform.GetComponent<RectTransform>();
        piecesRT.localPosition = bgRT.localPosition;
        piecesRT.sizeDelta = bgRT.sizeDelta;
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

    private G7_Piece CreatePiece(List<Vector2> positions, Transform parent)
    {
        G7_Piece piece = Instantiate(G7_Resources.instance.piecePrefab);

        piece.center = positions[0];
        piece.transform.SetParent(parent);
        piece.defaultPositions.AddRange(positions);

        Vector3 translaterVector = GetLocalPosition(piece.center);

        foreach (var position in piece.defaultPositions)
        {
            int col = (int)position.x;
            int row = (int)position.y;

            G7_Tile tile = Instantiate(G7_Resources.instance.tile_normal);
            tile.transform.SetParent(piece.transform);
            tile.transform.localScale = Vector3.one;
            Vector3 localPosition = GetLocalPosition(col, row);
            tile.transform.localPosition = localPosition - translaterVector;
            tile.position = new Vector2(col, row);
            tile.piece = piece;

            // Add shadows
            G7_Tile tileS = Instantiate(G7_Resources.instance.tile_shadow);
            tileS.transform.SetParent(piece.shadows.transform);
            tileS.transform.localScale = Vector3.one;
            tileS.transform.localPosition = tile.transform.localPosition + new Vector3(5, -5);

            piece.tiles.Add(tile);
        }
        foreach (var pos in piece.defaultPositions)
        {
            piece.tilePositions.Add(pos - positions[0]);
        }

        return piece;
    }
    private Vector3 GetLocalPosition(Vector2 position)
    {
        return GetLocalPosition((int)position.x, (int)position.y);
    }
    private Vector3 GetLocalPosition(int col, int row)
    {
        return col % 2 == 0 ? new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 0.5f) * G7_Tile.HEIGHT) :
                        new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 1) * G7_Tile.HEIGHT);
    }
    // clear values node in board 
    public void ClearCovers(G7_Piece piece)
    {
        foreach (var pos in piece.boardPositions)
        {
            slots[pos].hasCover = false;
        }
    }


    //
    public void CheckGameComplete()
    {
        bool isComplete = true;
        foreach (var slot in slots.Values)
        {
            if (!slot.hasCover)
            {
                isComplete = false;
                break;
            }
        }
        if (!isComplete)
        {
            return;
        }
        print("win game");

    }
    // ki?m tra có th?a mãn board
    public bool CheckMatch(G7_Piece piece)
    {
        float minDistance = float.MaxValue;
        G7_Tile matchSlot = null;

        foreach (G7_Tile slot in slots.Values)
        {
            if (slot.hasCover) continue;
            float distance = Vector3.Distance(piece.tileCenter.transform.position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                matchSlot = slot;
            }
        }
        piece.ResetMatchColor();
        piece.matches = new List<G7_Tile>();
        if (minDistance < 0.4f)
        {
            bool isMatch = true;
            List<Vector2> matchPos = GetMatchPositions(piece, matchSlot.position);

            foreach (var math in matchPos)
            {
                if (!slots.ContainsKey(math) || slots[math].hasCover)
                {
                    isMatch = false;
                    break;
                }
                else
                {
                    piece.matches.Add(slots[math]);
                }
            }
            if (isMatch)
            {
                piece.HighlightMatchColor();
            }
            return isMatch;
        }
        return false;
    }
    public bool ShowHint()
    {
        foreach (G7_Piece p in pieces)
        {
            if (hint.hintPieces.Find(x => x.id == p.id)) continue;
            if (p.isExtra) continue;
            if (p.status == G7_Piece.Status.OnBoard)
            {
                var samePieces = FindSamePieces(p);
                bool rightPos = false;
                foreach (var samePiece in samePieces)
                {
                    if (p.boardPositions[0] == samePiece.center)
                    {
                        rightPos = true;
                        break;
                    }
                    else
                    {
                        p.MoveToBottom();
                    }
                }

                if (!rightPos)
                {
                    ShowHint(p);
                    return true;
                }

            }

        }

        foreach (var piece in pieces)
        {
            if (hint.FindId(piece)) continue;
            if (piece.isExtra) continue;

            if (piece.status == G7_Piece.Status.OnBottom)
            {

                if (ShowHint(piece))
                {
                    return true;
                }

            }
        }



        return false;
    }

    private bool ShowHint(G7_Piece piece)
    {
        var samePieces = FindSamePieces(piece);
        foreach (var samePiece in samePieces)
        {
            if (samePiece.isExtra) continue;
            G7_Piece onPos = samePieces.Find(x => x.status == G7_Piece.Status.OnBoard && x.boardPositions[0] == samePiece.center);
            if (onPos == null)
            {
                var hintPiece = CreatePiece(samePiece.defaultPositions, G7_Resources.instance.hintPiecesTransform);
                hintPiece.transform.localScale = Vector3.one * 5f;
                hintPiece.transform.localPosition = GetLocalPosition(samePiece.center);
                hintPiece.id = piece.id;
                hintPiece.setSpriteTiles(piece.spriteHint);
                hintPiece.transform.DOScale(Vector3.one, 0.3f);

                hint.Add(hintPiece);
                return true;
            }

        }
        return false;
    }
    // tìm piece giong nhau
    private List<G7_Piece> FindSamePieces(G7_Piece sample)
    {
        List<G7_Piece> result = new List<G7_Piece>();
        foreach (G7_Piece piece in pieces)
        {
            if (piece == sample) continue;
            List<Vector2> matchPositions = GetMatchPositions(piece, sample.center);
            if (Compare2List(matchPositions, sample.defaultPositions))
            {
                result.Add(piece);
            }
        }
        result.Insert(0, sample);
        return result;
    }
    private bool Compare2List(List<Vector2> list1, List<Vector2> list2)
    {
        if (list1.Count != list2.Count) return false;
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i]) return false;
        }
        return true;
    }
    private bool Compare2List(List<Vector2> list1, List<Vector2> list2, Vector2 InvalidNumber)
    {
        if (list1.Count != list2.Count) return false;
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] - list2[i] != InvalidNumber && list2[i] - list1[i] != InvalidNumber) return false;
        }
        return true;
    }
    public void CheckPos(G7_Piece p1)
    {
        if (Compare2List(p1.defaultPositions, p1.boardPositions)) return;
        foreach (G7_Piece piece in pieces)
        {

            if (piece.id == p1.id) continue;
            if (piece.status == G7_Piece.Status.OnBoard) continue;
            if (piece.tiles.Count != p1.tiles.Count) continue;

            if (!Compare2List(piece.tilePositions, p1.tilePositions)) continue;
            if (!Compare2List(piece.defaultPositions, p1.defaultPositions, piece.defaultPositions[0] - p1.defaultPositions[0])) continue;
            Conver2Piece(piece, p1);


        }
    }
    public void Conver2Piece(G7_Piece p1, G7_Piece p2)
    {
        int id1 = p1.id;
        List<Vector2> defaultPositions1 = p1.defaultPositions;
        Vector2 center = p1.center;
        Sprite spriteHint1 = p1.spriteHint;
        p1.id = p2.id;
        p1.defaultPositions = p2.defaultPositions;
        p1.center = p2.center;
        //    p1.spriteHint = p1.sprite;

        p2.id = id1;
        p2.defaultPositions = defaultPositions1;
        p2.center = center;
        //  p2.spriteHint = s;

    }
}
