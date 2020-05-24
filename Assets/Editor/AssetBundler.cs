using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetBundle : MonoBehaviour {

    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("./assetbundle/",BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
    }
}