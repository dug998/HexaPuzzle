using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Node : MonoBehaviour
{
   
    public typeNode statusNode;
    public Vector3 cellPos;
    public Tile obj;
    public G7_Node(typeNode statusNode, Tile obj = null)
    {
        this.statusNode = statusNode;
        //  this.cellPos = cellPos;
        this.obj = obj;
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
