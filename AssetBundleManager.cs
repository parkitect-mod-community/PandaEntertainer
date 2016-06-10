using System;
using UnityEngine;
using System.Collections.Generic;

public class AssetBundleManager
{
	private Main Main {get;set;}
    public GameObject FrontCarGo;
    public GameObject BackCarGo;

    public GameObject[] Support_Go;
    public GameObject[] AngledSupportGo;
    public GameObject SupportHalf;




	public AssetBundleManager (Main main)
	{
		this.Main = main;
        FrontCarGo = LoadAsset<GameObject> ("Front_Car");
        BackCarGo = LoadAsset<GameObject> ("Cart");

        Support_Go = new GameObject[] {
            LoadAsset<GameObject> ("Support_1"),
            LoadAsset<GameObject> ("Support_2"),
            LoadAsset<GameObject> ("Support_3 1")
            };

        AngledSupportGo = new GameObject[] {
            LoadAsset<GameObject>("Angle_support_1"),
            LoadAsset<GameObject>("Angle_support_2")
        };

        SupportHalf = LoadAsset<GameObject> ("Half_Support");



    }



	private T  LoadAsset<T>(string prefabName) where T : UnityEngine.Object
	{
		try
		{
			T asset;

			char dsc = System.IO.Path.DirectorySeparatorChar;
			using (WWW www = new WWW("file://" + Main.Path + dsc + "assetbundle" + dsc + "MineTrain"))
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
					asset = bundle.LoadAsset<T>(prefabName);
					bundle.Unload(false);

					return asset;
				}
				catch (Exception e)
				{
					UnityEngine.Debug.LogException(e);
					bundle.Unload(false);
					return null;
				}
			}
		}
		catch (Exception e)
		{
			UnityEngine.Debug.LogException(e);
			return null;
		}
	}
}


