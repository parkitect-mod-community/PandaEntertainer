using System;
using UnityEngine;

public class MineTrainSupportInstantiator : SupportInstantiator
{
    public Material baseMaterial;

    public override void instantiateSupports (MeshGenerator meshGenerator, TrackSegment4 trackSegment, UnityEngine.GameObject putMeshOnGO)
    {
        CrossedTiles crossedTiles = trackSegment.getCrossedTiles ();
        foreach (CrossedTileInfo current in crossedTiles.crossedTilesInfo) {
            MineTrainSupports component = UnityEngine.Object.Instantiate (Main.AssetBundleManager.SupportMineTrainGo).GetComponent<MineTrainSupports> ();
            component.crossedTiles = crossedTiles.getCrossedSides (current.x, current.z);
            component.x = current.x;
            component.y = (int)current.minYOnRails;
            component.z = current.z;
            component.baseMaterial = baseMaterial;
            component.transform.parent = putMeshOnGO.transform;
            component.Initialize ();

        }
    }
}


