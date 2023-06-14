using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_TileRegion : MonoBehaviour
{
    public static G7_TileRegion instance;
    private Dictionary<Vector2, G7_Tile> slots = new Dictionary<Vector2, G7_Tile>();
    public List<G7_Piece> pieces = new List<G7_Piece>();
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void LoadBoard(G7_GameLevel gameLevel)
    {
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
            tile.transform.position = new Vector2(col, row);
        }
        // Create background 1
        foreach (var values in positions)
        {
            string[] value = values.Split(',');
            int col = int.Parse(value[0]);
            int row = int.Parse(value[1]);

            G7_Tile tile = Instantiate(values.Length == 2 ? G7_Resources.instance.tile_background : G7_Resources.instance.tile_stone);
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

        foreach (var one in data)
        {
            List<string> positions = G7_Utils.BuildListFromString<string>(one, '-');
            Vector2 bottomPos = Vector2.zero;

            List<Vector2> defaultPos = new List<Vector2>();
            bool isRedundant = positions[positions.Count - 1] == "r";// d? phòng

            if (isRedundant)
            {
                positions.RemoveAt(positions.Count - 1);
            }

            for (int i = 0; i < positions.Count; i++)
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
        }

    }
    private Vector3 GetLocalPosition(int col, int row)
    {
        return col % 2 == 0 ? new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 0.5f) * G7_Tile.HEIGHT) :
                        new Vector3((col * 1.5f + 1) * G7_Tile.EDGE_SIZE, (row + 1) * G7_Tile.HEIGHT);
    }
}
