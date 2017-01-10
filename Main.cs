using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
namespace PandaEntertainer
{
public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager = null;

    public void onEnabled()
    {
       

		if (Main.AssetBundleManager == null) {

			AssetBundleManager = new AssetBundleManager (this);
		}
        Employee employee = AssetManager.Instance.getPrefab<Employee> (Prefabs.Entertainer);
          
        BodyPartContainerContainer malePanda = new BodyPartContainerContainer ("Panda Costume", BodyPartContainerContainer.PrefabType.ENTERTAINER);
		malePanda.AddTorso (Main.AssetBundleManager.Body);
		malePanda.AddHairstyles (Main.AssetBundleManager.Head);

       EmployeeCostumeContainer costumeContainer = new EmployeeCostumeContainer ("Panda", "Panda", new Color[]{ });
       costumeContainer.SetMalePartContainer (malePanda.Apply());
       
        Array.Resize< EmployeeCostume> (ref employee.costumes, employee.costumes.Length + 1);
        employee.costumes [employee.costumes.Length - 1] = costumeContainer.employeeCostume;
            AssetManager.Instance.registerObject(costumeContainer.employeeCostume);

       
	}

    public void onDisabled()
    {
	}

    public string Name
    {
        get { return "Panda Entertainer"; }
    }

    public string Description
    {
        get { return "Adds a fuzzy panda entertainer to the Parkitect."; }
    }


	public string Path { get; set; }

}
}

