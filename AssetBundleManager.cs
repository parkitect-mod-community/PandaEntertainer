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
            _assetBundle = AssetBundle.LoadFromFile(main.Path + dsc + "assetbundle" + dsc + "assetpack");

            Head = _assetBundle.LoadAsset<GameObject>("5bc917ea4eecb4beea26792b912a314b");
            Body = _assetBundle.LoadAsset<GameObject>("81b749044290b496eba80cd27935f395");
        }
        
        
        public void unload()
        {
            _assetBundle.Unload(false);
        }
    }
}