using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class G7_GameController : MonoBehaviour
{
    public static G7_GameController instance;
    public G7_Board Board;

    public Vector3 mousePos;
    public bool canMove;
    public LayerMask layerMask;

    public Vector3 offset = new Vector3(0, 0.2f, -0.4f);
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (G7_MoveBlock.currenMoveBlock == null)
            return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 0;
        G7_MoveBlock.currenMoveBlock.transform.position = mousePosition + offset;
        getMousePos();
    }
    void getMousePos()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            mousePos = hit.collider.gameObject.GetComponent<Tile>().getSize();
            mousePos.y = 0;


        }
        else
        {
            mousePos = new Vector3(-1, -1, -1);
        }



        G7_MoveBlock.currenMoveBlock.newPosCol = Mathf.RoundToInt(mousePos.x);
        G7_MoveBlock.currenMoveBlock.newPosRow = Mathf.RoundToInt(mousePos.z);
        G7_MoveBlock.currenMoveBlock.setTransform();


    }

    public bool CheckBoard(G7_MoveBlock block)
    {
        return Board.CheckBoard(block);

    }
    public void AddObject(G7_MoveBlock moveBlock)
    {
        Board.AddObject(moveBlock);
    }
    public void RemoveObject(G7_MoveBlock moveBlock)
    {
        Board.RemoveObject(moveBlock);
    }
    public void CheckVisibleSuggest(G7_MoveBlock moveBlock)
    {
        Board.CheckVisibleSuggest(moveBlock);
    }
    public void WinGame()
    {
        Debug.Log("win game");
    }
}
