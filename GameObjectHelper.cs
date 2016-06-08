using System;
using UnityEngine;


public static class GameObjectHelper
{

    public static GameObject SetUV(GameObject GO, int gridX, int gridY)
    {
        Mesh mesh = GO.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.0625f * ((float)gridX + 0.5f), 1f - 0.0625f * ((float)gridY + 0.5f));
        }
        mesh.uv = uvs;
        return GO;
    }
     
}
