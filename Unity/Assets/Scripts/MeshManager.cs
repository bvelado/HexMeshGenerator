using UnityEngine;
using System.Collections;

public class MeshManager : MonoBehaviour {

    HexMeshGenerator meshGenerator;

    public int Width;
    public int Height;

	void Start () {

        meshGenerator = new HexMeshGenerator(1);

        var pos = meshGenerator.GenerateVertices(Width, Height);
        for (int p = 0; p < pos.Length; p++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = pos[p];
            go.name = p.ToString();
            go.transform.localScale = Vector3.one * 0.2f;
        }
	}
}
