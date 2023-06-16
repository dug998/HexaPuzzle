using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_GameManager : MonoBehaviour
{
    public static G7_GameManager instance;
    public int level = 1;
    public int world = 1;
    public G7_TileRegion tileRegion;
    public G7_LevelPref levelPrefs;

    [HideInInspector]
    public G7_GameLevel gameLevel;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        level = G7_GameState.chosenLevel;
        world = G7_GameState.chosenWorld;
        gameLevel = Resources.Load<G7_GameLevel>("Levels/World_" + world + "/Level_" + level);
        string strLevelPrefs = G7_PrefData.GetLevelData(world, level);
        if (string.IsNullOrEmpty(strLevelPrefs))
        {
            levelPrefs = new G7_LevelPref();
            levelPrefs.piecesPrefs = new List<G7_PiecePrefs>();
        }
        else
        {
            levelPrefs = JsonUtility.FromJson<G7_LevelPref>(strLevelPrefs);
        }
        tileRegion.LoadBoard(gameLevel);
    }
    public void RePlayAll()
    {
        G7_GameState.canPlay = true;
        foreach (var piece in tileRegion.pieces)
        {
            piece.MoveToBottom();
        }
    }
    public void ReplayOne()
    {

    }
    public void ShowHint()
    {
        if (G7_GameState.hint < 0)
        {
            print("no hint");
            return;
        }
        bool isShown = tileRegion.ShowHint();
        if (isShown)
        {
            G7_GameState.hint--;
        }
    }
}
