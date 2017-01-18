using UnityEngine;
using System.Collections;

public class MeshManager : MonoBehaviour {

    HexMeshGenerator meshGenerator;

    public int Width;
    public int Height;

    public bool DrawMesh;

    public Material MeshMaterial;

    public GameObject Prefab;

	void Start () {

        meshGenerator = new HexMeshGenerator(1);
        
        Transform container = new GameObject("Generated Mesh Container").transform;

        var pos = meshGenerator.GenerateVertices(Width, Height);
        for (int p = 0; p < pos.Length; p++)
        {
            var g = Instantiate(Prefab, pos[p] + Vector3.up, Prefab.transform.rotation, container) as GameObject;
            g.name = p.ToString();
            g.GetComponent<TextMesh>().text = p.ToString();
        }
        

        if (DrawMesh)
        {
            var go = new GameObject("Generated mesh");
            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = meshGenerator.GenerateMesh();
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = MeshMaterial;
        }


    }
}
