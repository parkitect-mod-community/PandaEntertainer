using UnityEngine;

namespace PandaEntertainer
{
    public class AssetBundleManager
    {
        private readonly AssetBundle _assetBundle;
        public readonly GameObject Body;
        public readonly GameObject Head;

        public AssetBundleManager(Main main)
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;
            _assetBundle = AssetBundle.LoadFromFile(main.Path + dsc + "entertainer");

            Head = _assetBundle.LoadAsset<GameObject>("head");
            Body = _assetBundle.LoadAsset<GameObject>("panda");
        }
    }
}