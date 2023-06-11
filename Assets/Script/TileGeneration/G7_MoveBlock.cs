using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class G7_MoveBlock : MonoBehaviour
{
    public static G7_MoveBlock currenMoveBlock;
    public G7_Block block;
    private Collider collider;
    public int posCol, posRow;
    public int newPosCol, newPosRow;
    protected Vector3Int currentPos;
 
    private void Awake()
    {
        block = GetComponent<G7_Block>();
        collider = GetComponent<Collider>();
    }
    private void OnMouseDown()
    {
        currenMoveBlock = this;
        currentPos = getTransform();
        G7_GameController.instance.RemoveObject(currenMoveBlock);

    }
    private void OnMouseDrag()
    {
        currenMoveBlock = this;
    }
    private void OnMouseUp()
    {
        if (currenMoveBlock == this)
        {
            if (G7_GameController.instance.CheckBoard(currenMoveBlock))
            {
                currentPos = getTransform();
                G7_GameController.instance.AddObject(currenMoveBlock);
            }
            else
            {
                newPosCol = Mathf.RoundToInt(currentPos.x);
                newPosRow = Mathf.RoundToInt(currentPos.z);
                setTransform();
            }

        }
        currenMoveBlock = null;

    }
   

    public void setTransform()
    {
        if (newPosCol == posCol && newPosRow == posRow)
        {
            return;
        }
        posCol = newPosCol;
        posRow = newPosRow;
        G7_GameController.instance.CheckVisibleSuggest(this);
    }
    public Vector3Int getTransform()
    {
        return Vector3Int.RoundToInt(transform.position);
    }
}
