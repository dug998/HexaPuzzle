using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_HintPieces : MonoBehaviour
{
    public List<G7_Piece> hintPieces = new List<G7_Piece>();
    public Queue<G7_Piece> piecesQueue = new Queue<G7_Piece>();
    public void Add(G7_Piece p)
    {
        hintPieces.Add(p);
        piecesQueue.Enqueue(p);
        StartCoroutine(DeLayCall(4, () =>
        {
            G7_Piece p = piecesQueue.Dequeue();
            hintPieces.Remove(p);
            Destroy(p.gameObject);
        }));
    }
    IEnumerator DeLayCall(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    public bool FindId(G7_Piece p)
    {
        return hintPieces.Find(x => x.id == p.id);
    }



}
