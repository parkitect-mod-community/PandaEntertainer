using UnityEngine;
using System.Collections.Generic;

public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager = null;
    public static Configuration Configeration = null;

    public static string HASH = "a9sfj-[a9w34ainw;kjasinda";

    private List<UnityEngine.Object> registeredObjects = new List<UnityEngine.Object>();

    GameObject hider;
    public void onEnabled()
    {

        hider = new GameObject ();

		if (Main.AssetBundleManager == null) {

			AssetBundleManager = new AssetBundleManager (this);
		}

        TrackedRide selected = null;
        foreach (Attraction t in AssetManager.Instance.getAttractionObjects ()) {
            if (t.getUnlocalizedName() == "Wooden Coaster") {
                selected = (TrackedRide)t;
                break;
                    
            }
        }



        TrackedRide trackRider = UnityEngine.Object.Instantiate (selected);

        trackRider.dropsImportanceExcitement = .7f;
        trackRider.inversionsImportanceExcitement = .67f;
        trackRider.averageLatGImportanceExcitement = .7f;
        trackRider.meshGenerator = ScriptableObject.CreateInstance<SideFrictionTrackGenerator> ();
        trackRider.meshGenerator.stationPlatformGO = selected.meshGenerator.stationPlatformGO;
        trackRider.meshGenerator.material = selected.meshGenerator.material;
        trackRider.meshGenerator.liftMaterial = selected.meshGenerator.liftMaterial;
        trackRider.meshGenerator.frictionWheelsGO = selected.meshGenerator.frictionWheelsGO;
        trackRider.meshGenerator.supportInstantiator = selected.meshGenerator.supportInstantiator;
        trackRider.meshGenerator.crossBeamGO = selected.meshGenerator.crossBeamGO;


        Color[] colors = new Color[] { new Color(68f / 255f, 47f / 255f, 37f / 255f, 1), new Color(74f / 255f, 32f / 255f, 32f / 255f, 1), new Color(66f / 255f, 66f / 255f, 66f / 255f, 1) };
        trackRider.meshGenerator.customColors = colors;
        trackRider.setDisplayName("Side Friction Coaster");
        trackRider.price = 3600;
        trackRider.name = "side_friction_GO" ;
        trackRider.maxBankingAngle = 20;
        trackRider.accelerationVelocity = .09f;
        trackRider.maximumVelocity = 80f;


        AssetManager.Instance.registerObject (trackRider);
        registeredObjects.Add (trackRider);

        //get car
        GameObject carGo = UnityEngine.GameObject.Instantiate(Main.AssetBundleManager.Car);
        Rigidbody carRigid = carGo.AddComponent<Rigidbody> ();
        carRigid.isKinematic = true;
        carGo.AddComponent<BoxCollider> ();


        //add Component
        MineTrainCar car = carGo.AddComponent<MineTrainCar> ();
        car.name = "SideFriction_Car" + HASH;

        car.offsetFront = .02f;
        car.Decorate (true);

        CoasterCarInstantiator coasterCarInstantiator = ScriptableObject.CreateInstance<CoasterCarInstantiator> ();
        List<CoasterCarInstantiator> trains = new List<CoasterCarInstantiator>();

        coasterCarInstantiator.name = "Side Friction@CoasterCarInstantiator" + HASH;
        coasterCarInstantiator.defaultTrainLength = 5;
        coasterCarInstantiator.maxTrainLength = 7;
        coasterCarInstantiator.minTrainLength = 2;
        coasterCarInstantiator.carGO = carGo;
  
        //register cars
        AssetManager.Instance.registerObject (car);
        registeredObjects.Add (car);
        //Offset
        car.offsetBack = .25f;

        //Restraints
        RestraintRotationController controller = carGo.AddComponent<RestraintRotationController>();
        controller.closedAngles = new Vector3(0,0,120);


        //Custom Colors
        Color[] CarColors = new Color[] { new Color(0f / 255, 4f / 255, 190f / 255), new Color(138f / 255, 15f / 255, 15f / 255), new Color(101f / 255, 21f / 255, 27f / 255),new Color(172f / 255, 41f / 255, 42f / 255)};

        MakeRecolorble(carGo, "CustomColorsDiffuse", CarColors);

        coasterCarInstantiator.displayName = "Side Friction Car";
        AssetManager.Instance.registerObject (coasterCarInstantiator);
        registeredObjects.Add (coasterCarInstantiator);

        trains.Add (coasterCarInstantiator);

        trackRider.carTypes = trains.ToArray();

        hider.SetActive (false);
        carGo.transform.parent = hider.transform;
      

	}

    private void MakeRecolorble(GameObject GO, string shader, Color[] colors)
    {
        CustomColors cc = GO.AddComponent<CustomColors>();
        cc.setColors(colors);

        foreach (Material material in AssetManager.Instance.objectMaterials)
        {
            if (material.name == shader)
            {
                CoasterTools.SetMaterial(GO, material);
                break;
            }
        }

    }



    public void onDisabled()
    {
        foreach(UnityEngine.Object o in registeredObjects)
        {
            AssetManager.Instance.unregisterObject (o);
        }
        UnityEngine.GameObject.DestroyImmediate (hider);
	}

    public string Name
    {
        get { return "Mine Train Coaster"; }
    }

    public string Description
    {
        get { return "Mine Train Coaster"; }
    }


	public string Path { get; set; }

}

