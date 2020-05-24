﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ParkitectAssetEditor
{
    /// <summary>
    /// Help class for serializing the asset pack.
    /// </summary>
    public static class AssetPackSerializer
    {
        /// <summary>
        /// Saves the specified asset pack.
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns></returns>
        public static bool CreateAssetBundle(this AssetPack assetPack)
        {
            if (assetPack.Assets.Any(a => a.GameObject == null))
            {
                foreach (var asset in assetPack.Assets.Where(a => a.GameObject == null))
                {
                    Debug.LogError(string.Format("Could not save asset pack because GameObject of asset {0} is missing.", asset.Name));

                    return false;
                }
            }

            // make sure the prefab directory exists
            Directory.CreateDirectory(Path.Combine(ProjectManager.Project.Value.ProjectDirectory, "Resources/AssetPack"));

            // create the prefabs and store the paths in prefabPaths
            var prefabPaths = new List<string>();
            foreach (var asset in assetPack.Assets)
            {
                var path = string.Format("Assets/Resources/AssetPack/{0}.prefab", asset.Guid);

                PrefabUtility.CreatePrefab(path, asset.GameObject);

                prefabPaths.Add(path);
            }

            // use the prefab list to build an assetbundle
            AssetBundleBuild[] descriptor = {
                new AssetBundleBuild()
                {
                    assetBundleName = "assetPack",
                    assetNames      = prefabPaths.ToArray()
                }
            };

            BuildPipeline.BuildAssetBundles(ProjectManager.Project.Value.ModDirectory, descriptor, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneLinux64);

            return true;
        }

        /// <summary>
        /// Fills asset pack with gameobjects from the scene and/or prefabs.
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        public static void LoadGameObjects(this AssetPack assetPack)
        {
            for (var i = assetPack.Assets.Count - 1; i >= 0; i--)
            {
                var asset = assetPack.Assets[i];

                // instantiate the prefab if game object doesn't exist.
                if (asset.GameObject == null)
                {
                    Debug.Log(string.Format("Can't find {0} in the scene, instantiating prefab.", asset.Name));
                    try // if one object fails to load, don't make it fail the rest
                    {
                        var go = Resources.Load<GameObject>(string.Format("AssetPack/{0}", asset.Guid));

                        asset.GameObject = Object.Instantiate(go);
                        asset.GameObject.name = asset.Name;
                    }
                    catch (System.Exception)
                    {
                        Debug.LogError(string.Format("Could not find GameObject at Assets/Resources/AssetPack/{0} for asset {1}, skipped loading of asset", asset.Guid, asset.Name));

                        assetPack.Assets.Remove(asset);
                    }
                }
            }
        }
    }
}
