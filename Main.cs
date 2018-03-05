using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PandaEntertainer
{
public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager;
	private readonly List<Object> registeredObjects = new List<Object>();
	private GameObject hider;

	public void onEnabled()
	{
		hider = new GameObject();
		hider.SetActive(false);
		
		if (AssetBundleManager == null)
		{
			AssetBundleManager = new AssetBundleManager(this);
		}

		Employee employee = AssetManager.Instance.getPrefab<Employee>(Prefabs.Entertainer);
		
		BodyPartContainerContainer panda =
			new BodyPartContainerContainer("Panda Costume", BodyPartContainerContainer.PrefabType.ENTERTAINER);

		GameObject body = GameObject.Instantiate(AssetBundleManager.Body);
		body.transform.SetParent(hider.transform);
		panda.AddTorso(body);
		
		GameObject hairyle = GameObject.Instantiate(AssetBundleManager.Head);
		hairyle.transform.SetParent(hider.transform);
		panda.AddHairstyles(hairyle);

		EmployeeCostumeContainer costumeContainer = new EmployeeCostumeContainer("Panda", "Panda", new Color[] { });
		costumeContainer.SetMalePartContainer(panda.Apply());


		Array.Resize(ref employee.costumes, employee.costumes.Length + 1);
		employee.costumes[employee.costumes.Length - 1] = costumeContainer.EmployeeCostume;
		AssetManager.Instance.registerObject(costumeContainer.EmployeeCostume);
		
		registeredObjects.Add(costumeContainer.EmployeeCostume);
	}

	public void onDisabled()
    {
	    foreach (Object current in registeredObjects)
	    {
		    ScriptableSingleton<AssetManager>.Instance.unregisterObject(current);
	    }
	    UnityEngine.Object.DestroyImmediate(hider);
	}

    public string Name => "Panda Entertainer";

	public string Description => "Adds a fuzzy panda entertainer to the Parkitect.";
	
	string IMod.Identifier => "PandaEntertainer";

	
	public string Path
	{
		get
		{
			return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
		}
	}
}
}

