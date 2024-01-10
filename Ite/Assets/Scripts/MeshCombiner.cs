using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] List<MeshFilter> sourceMeshFilters;
    [SerializeField] MeshFilter targetMeshFilter;
    MeshCollider collider;

    private void Awake()
    {
        targetMeshFilter = GetComponent<MeshFilter>();
        collider = GetComponent<MeshCollider>();
        CombineMeshes();
        print("but spotted");
    }

    [ContextMenu("Combine Meshes")]
    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (var i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }
}
