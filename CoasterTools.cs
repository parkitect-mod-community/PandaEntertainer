using System;
using UnityEngine;

public static class CoasterTools
{
    public static void SetMaterial(GameObject go, Material material)
    {
        // Go through all child objects and recolor     
        Renderer[] renderCollection;
        renderCollection = go.GetComponentsInChildren<Renderer>();

        foreach (Renderer render in renderCollection)
        {
            render.sharedMaterial = material;
        }
    }

}


