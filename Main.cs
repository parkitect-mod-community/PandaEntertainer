﻿using UnityEngine;
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
        malePanda.AddTorso (Main.AssetBundleManager.TestEntertainer);
            malePanda.AddHairstyles (employee.costumes [0].bodyPartsMale.getHairstyle (0));

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
        get { return "An early wooden coaster design that used a trough/side rail design that isn't use anymore in most modern day coaster. "; }
    }


	public string Path { get; set; }

}
}

