using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PandaEntertainer
{
public class AssetBundleManager
{
	private readonly Main _main;
    public readonly GameObject Head;
	public readonly GameObject Body;

	public AssetBundleManager (Main main)
	{
		_main = main;	
		
        Head = LoadAsset<GameObject> ("head");
		Body = LoadAsset<GameObject>("panda");
	}



	private T  LoadAsset<T>(string prefabName) where T : Object
	{
		try
		{
			char dsc = System.IO.Path.DirectorySeparatorChar;
            using (WWW www = new WWW("file://" + _main.Path + dsc + "assetbundle" + dsc + "Entertainer"))
			{

				if (www.error != null)
				{
					Debug.Log("Loading had an error:" + www.error);
					throw new Exception("Loading had an error:" + www.error);
				}
				if(www.assetBundle == null)
				{
					Debug.Log("Loading had an error:" + www.error);
					throw new Exception("assetBundle is null");

				}
				AssetBundle bundle = www.assetBundle;


				try
				{
					var asset = bundle.LoadAsset<T>(prefabName);
					bundle.Unload(false);
					
					return asset;
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					bundle.Unload(false);
					return null;
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			return null;
		}
	}
}
}


