using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_color : MonoBehaviour
{
    public int mat_id;
    MeshRenderer mesh;
    public Material[] maters;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.sharedMaterials[mat_id] = maters[1];
    }
    public void Dead()
    {
        mesh.sharedMaterial = maters[maters.Length -1];
    }
    public void Set_color(int id)
    {
        mesh.sharedMaterials[mat_id] = maters[id];
    }
}
