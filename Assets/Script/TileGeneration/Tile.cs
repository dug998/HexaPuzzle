using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class Tile : MonoBehaviour
{
    public List<Material> materials;
    public Tile connectedTile;
    // public Character occupyingCharacter;
    public int row;
    public int col;

    private bool _active;
    public int index;
    private MeshRenderer _meshRenderer;
    public Material _material;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
    }
    public Vector3 getSize() { return new Vector3(col, 0, row); }
    public void VisibleSuggest(bool active)
    {
        if (index < -1)
        {
            return;
        }
        if (_active == active)
            return;
        _active = active;
        if (active)
        {
            _meshRenderer.material = materials[index];
        }
        else
        {
            _meshRenderer.material = _material;
        }
    }
}
