using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class G7_LevelPref 
{
    public List<G7_PiecePrefs> piecesPrefs;
}
[System.Serializable]
public class G7_PiecePrefs
{
    public int id;
    public string boardPosition;
}