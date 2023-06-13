using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_C_CoordinateConverter : MonoBehaviour
{
    public static string SaveCoordinatesToString(int[,] array2D)
    {
        string coordinates = "";

        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            for (int j = 0; j < array2D.GetLength(1); j++)
            {
                coordinates += $"[{i}, {j}]: {array2D[i, j]} ";
            }
        }

        return coordinates;
    }

    // Chuy?n chu?i string thành m?ng 2 chi?u
    public static int[,] ConvertStringToCoordinates(string coordinates)
    {
        string[] coordinatePairs = coordinates.Split(' ');

        int[,] array2D = new int[coordinatePairs.Length, 2];

        for (int i = 0; i < coordinatePairs.Length; i++)
        {
            string[] coordinate = coordinatePairs[i].Trim('[', ']').Split(',');
            int x = int.Parse(coordinate[0]);
            int y = int.Parse(coordinate[1]);
            int value = int.Parse(coordinate[2]);

            array2D[x, y] = value;
        }

        return array2D;
    }
}
