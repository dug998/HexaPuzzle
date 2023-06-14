using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_PrefData : MonoBehaviour
{
    public static string GetLevelData(int world, int level)
    {
        return PlayerPrefs.GetString("level_data_" + world + "_" + level);
    }
}
