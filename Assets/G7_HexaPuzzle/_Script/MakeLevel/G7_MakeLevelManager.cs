using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class G7_MakeLevelManager : MonoBehaviour
{
    public static G7_MakeLevelManager instance;
    public G7_GameLevel gameLevel;
    public G7_TileRegion2 tileRegion;
    public TMP_InputField worldInput, levelInput, numColInput, numRowInput;
   
    public TMP_Text loadLevelText;
    public Button mapButton, piecesButton, applyButton;

    public int world, level, numRow, numCol;

    public List<G7_Tile> listSlots = new List<G7_Tile>();
    public List<G7_Tile> listExtraTile = new List<G7_Tile>();

    public bool doneGeneratePieces;

    private string piecesResult = "";
    private List<List<G7_Tile>> pieces = new List<List<G7_Tile>>();
    public int totalTiles = 0;

    [Header("button")]
    public Button btnLoad;
    public Button btnGenerateMap;
    public Button btnGeneratePiece;
    public Button btnApple;

    private void Awake()
    {
        instance = this;
      

    }
    private void Start()
    {
        tileRegion.LoadBottomBackground();
        worldInput.text = "1";
        levelInput.text = "1";
        numRowInput.text = "5";
        numColInput.text = "6";
        UpdateUi();
    }
    private void OnEnable()
    {
        btnLoad.onClick.AddListener(OnLoadClick);
        btnGenerateMap.onClick.AddListener(GeneratePos);
        btnGeneratePiece.onClick.AddListener(GeneratePiece);
        btnApple.onClick.AddListener(GeneratePiece);
    }
    public void OnLoadClick()
    {
        gameLevel = Resources.Load<G7_GameLevel>("Levels/World_" + world + "/Level_" + level);
        if (gameLevel == null)
        {
            string folderPath = "Assets/Hexa_Puzzle/Resources/Levels/World_" + world;
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets/Hexa_Puzzle/Resources/Levels", "World_" + world);

            gameLevel = ScriptableObject.CreateInstance<G7_GameLevel>();
        }
        piecesResult = "";
        totalTiles = 0;
        pieces.Clear();
        listExtraTile.Clear();
        doneGeneratePieces = false;
        tileRegion.LoadBoardBackground();
        if (!string.IsNullOrEmpty(gameLevel.pieces))
        {
            tileRegion.LoadPieces(gameLevel);
            foreach (var p in tileRegion.pieces)
            {
                pieces.Add(p.tiles);
            }
            piecesResult = gameLevel.pieces;
            doneGeneratePieces = true;
        }
        else
        {
            tileRegion.ClearPieces();

        }
        LoadListSlots();
        UpdateUi();
    }
    public void GeneratePiece()
    {
        List<G7_Tile> piece = new List<G7_Tile>();
        if (doneGeneratePieces)
        {
            foreach (var tile in listSlots)
            {
                if (tile.isActive == false && !listExtraTile.Contains(tile))
                {
                    piece.Add(tile);
                }
            }
            if (piece.Count == 0) return;

            listExtraTile.AddRange(piece);

            string extraPiecesResult = "";
            foreach (var tile in piece)
            {
                extraPiecesResult += tile.position.x + "," + tile.position.y + "-";
            }
            extraPiecesResult += "0,0" + "-r";
            gameLevel.pieces += extraPiecesResult;
            tileRegion.LoadPieces(gameLevel);
            CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));

        }
        else
        {
            foreach (var tile in listSlots)
            {
                if (tile.isActive == false && !HasElement(tile))
                {
                    piece.Add((G7_Tile)tile);
                }
            }
            if (piece.Count == 0) return;
            totalTiles += piece.Count;

            pieces.Add(piece);

            piecesResult = CreatePiecesResult();
            print(piecesResult);
            if (totalTiles == listSlots.Count)
            {
                doneGeneratePieces = true;
                gameLevel.pieces = piecesResult;
                tileRegion.LoadPieces(gameLevel);
                foreach (var tile in listSlots)
                {
                    tile.gameObject.SetActive(false);
                }
                UpdateUi();
                CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
            }
        }


    }
    public void GeneratePos()
    {
        string result = "";
        LoadListSlots();
        foreach (var p in listSlots)
        {
            result += p.position.x + "," + p.position.y + "|";
        }
        gameLevel.positions = result;
        print(result);


        // save Asset
        CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
        UpdateLoadLevelText();
        UpdateUi();
    }
    public void AdjustPieces()
    {
        string result = "";
        foreach (var p in tileRegion.pieces)
        {
            foreach (var pos in p.defaultPositions)
            {
                result += pos.x + "," + pos.y + "-";
            }
            result += p.boardPositions[0].x + "," + p.boardPositions[0].y;

            if (p.isExtra) result += "-r";
            result += "|";
        }
        gameLevel.pieces = result;

        CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
        piecesResult = gameLevel.pieces;
    }
    public void ApplyLevel()
    {
        AdjustPieces();

    }
    private G7_GameLevel CreateOrReplaceAsset(G7_GameLevel asset, string path)
    {
        G7_GameLevel existingAsset = AssetDatabase.LoadAssetAtPath<G7_GameLevel>(path);
        if (existingAsset == null)
        {
            AssetDatabase.CreateAsset(asset, path);
            existingAsset = asset;
        }
        else
        {
            EditorUtility.CopySerialized(asset, existingAsset);
        }
        return existingAsset;
    }
    private void LoadListSlots()
    {
        listSlots.Clear();
        foreach (Transform slot in G7_Resources.instance.backgroundTilesTransform)
        {
            G7_Tile tile = slot.GetComponent<G7_Tile>();
            if (tile.isActive && tile.type == G7_Tile.Type.Background) listSlots.Add(tile);
        }

    }
    public void UpdateUi()
    {
        if (gameLevel == null)
        {
            mapButton.interactable = false;
            piecesButton.interactable = false;
            applyButton.interactable = false;
        }
        else
        {
            mapButton.interactable = true;
            piecesButton.interactable = !string.IsNullOrEmpty(gameLevel.positions);
            applyButton.interactable = doneGeneratePieces;
        }
    }
    private string GetLevelPath(int world, int level)
    {
        return "Assets/Resources/Levels/World_" + world + "/Level_" + level + ".asset";
    }
    public void UpdateLoadLevelText()
    {
        var gameLevel = Resources.Load<G7_GameLevel>("Levels/World_" + world + "/Level_" + level);
        loadLevelText.text = gameLevel == null ? "Add" : "Load";
    }
    private bool HasElement(G7_Tile element)
    {
        foreach (var piece in pieces)
        {
            foreach (var tile in piece)
            {
                if (element == tile) return true;
            }
        }
        return false;

    }
    private string CreatePiecesResult()
    {
        string result = "";

        foreach (var aPiece in pieces)
        {
            foreach (var tile in aPiece)
            {
                result += tile.position.x + "," + tile.position.y + "-";
            }
            result += "0,0" + "|";
        }

        return result;
    }
    public void OnInputValueChanged()
    {
        if (worldInput.text == "-") worldInput.text = "";
        if (levelInput.text == "-") levelInput.text = "";
        if (numRowInput.text == "-") numRowInput.text = "";
        if (numColInput.text == "-") numColInput.text = "";

        if (!string.IsNullOrEmpty(worldInput.text))
        {
            int.TryParse(worldInput.text, out world);
        }

        if (!string.IsNullOrEmpty(levelInput.text))
        {
            int.TryParse(levelInput.text, out level);
        }

        if (!string.IsNullOrEmpty(numRowInput.text))
        {
            int.TryParse(numRowInput.text, out numRow);
            if (numRow < 1) numRowInput.text = "1";
            else if (numRow > 7) numRowInput.text = "7";
        }

        if (!string.IsNullOrEmpty(numColInput.text))
        {
            int.TryParse(numColInput.text, out numCol);
            if (numCol < 1) numColInput.text = "1";
            else if (numCol > 7) numColInput.text = "9";
        }

        UpdateLoadLevelText();
    }

}
