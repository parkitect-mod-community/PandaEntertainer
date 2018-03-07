using System;
using System.Collections.Generic;
using System.Linq;
using PandaEntertainer;
using UnityEngine;
using Object = UnityEngine.Object;

public class Main : IMod
{
    public static AssetBundleManager AssetBundleManager;
    private readonly List<EmployeeCostume> registeredObjects = new List<EmployeeCostume>();
    private GameObject _hider;


    public string Path
    {
        get { return ModManager.Instance.getModEntries().First(x => x.mod == this).path; }
    }

    public void onEnabled()
    {
        _hider = new GameObject();
        _hider.SetActive(false);

        if (AssetBundleManager == null) AssetBundleManager = new AssetBundleManager(this);

        var employee = AssetManager.Instance.getPrefab<Employee>(Prefabs.Entertainer);

        var panda =
            new BodyPartContainerContainer("Panda Costume", BodyPartContainerContainer.PrefabType.ENTERTAINER, _hider);

        panda.AddTorso(AssetBundleManager.Body);
        panda.AddHairstyles(AssetBundleManager.Head);

        var costumeContainer = new EmployeeCostumeContainer("Panda", "Panda", new Color[] { });
        costumeContainer.SetMalePartContainer(panda.Apply());

        Array.Resize(ref employee.costumes, employee.costumes.Length + 1);
        employee.costumes[employee.costumes.Length - 1] = costumeContainer.EmployeeCostume;
        AssetManager.Instance.registerObject(costumeContainer.EmployeeCostume);
        
        registeredObjects.Add(costumeContainer.EmployeeCostume);
    }

    public void onDisabled()
    {
        var employee = AssetManager.Instance.getPrefab<Employee>(Prefabs.Entertainer);
        var costumes = new List<EmployeeCostume>(employee.costumes);

        foreach (var current in registeredObjects)
        {
            costumes.Remove(current);
            ScriptableSingleton<AssetManager>.Instance.unregisterObject(current);
        }

        employee.costumes = costumes.ToArray();
        Object.DestroyImmediate(_hider);
    }

    public string Name => "Panda Entertainer";

    public string Description => "Adds a fuzzy panda entertainer to the Parkitect.";

    string IMod.Identifier => "PandaEntertainer";
}