using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Node : MonoBehaviour
{

    public typeNode statusNode;
    public Vector3 cellPos;
    public Tilesss obj;
    public G7_Node(typeNode statusNode, Tilesss obj)
    {
        this.statusNode = statusNode;
        //  this.cellPos = cellPos;
        if (obj != null)
            this.obj = obj;
    }
    public void show(bool action)
    {
        if (obj != null)
        {
            obj.gameObject.SetActive(action);
        }

    }
    public typeNode checkNode()
    {
        return statusNode;
    }
}
public enum typeNode
{
    none,
    empty,
    full

}
