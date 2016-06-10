using UnityEngine;
using System.Collections.Generic;

public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager = null;
    public static Configuration Configeration = null;

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

        MineTrainSupportInstantiator supportInstaiator = ScriptableObject.CreateInstance<MineTrainSupportInstantiator> ();
        AssetManager.Instance.registerObject (supportInstaiator);
        registeredObjects.Add (supportInstaiator);
        supportInstaiator.baseMaterial = selected.meshGenerator.material;

        trackRider.dropsImportanceExcitement = .7f;
        trackRider.inversionsImportanceExcitement = .67f;
        trackRider.averageLatGImportanceExcitement = .7f;
        trackRider.meshGenerator = ScriptableObject.CreateInstance<MinetrainTrackGenerator> ();
        trackRider.meshGenerator.stationPlatformGO = selected.meshGenerator.stationPlatformGO;
        trackRider.meshGenerator.material = selected.meshGenerator.material;
        trackRider.meshGenerator.liftMaterial = selected.meshGenerator.liftMaterial;
        trackRider.meshGenerator.frictionWheelsGO = selected.meshGenerator.frictionWheelsGO;
        trackRider.meshGenerator.supportInstantiator = supportInstaiator;//selected.meshGenerator.supportInstantiator;
        trackRider.meshGenerator.crossBeamGO = selected.meshGenerator.crossBeamGO;


        Color[] colors = new Color[] { new Color(63f / 255f, 46f / 255f, 37f / 255f, 1), new Color(43f / 255f, 35f / 255f, 35f / 255f, 1), new Color(90f / 255f, 90f / 255f, 90f / 255f, 1) };
        trackRider.meshGenerator.customColors = colors;
        trackRider.meshGenerator.customColors = colors;
        trackRider.setDisplayName("MineTrain Coaster");
        trackRider.price = 3600;
        trackRider.name = "Corkscrew_coaster_GO";
        AssetManager.Instance.registerObject (trackRider);
        registeredObjects.Add (trackRider);

        //get car
        GameObject carGo = UnityEngine.GameObject.Instantiate(Main.AssetBundleManager.BackCarGo);
        Rigidbody carRigid = carGo.AddComponent<Rigidbody> ();
        carRigid.isKinematic = true;
        carGo.AddComponent<BoxCollider> ();

        GameObject frontcarGo = UnityEngine.GameObject.Instantiate(Main.AssetBundleManager.FrontCarGo);
        Rigidbody frontcarRigid = frontcarGo.AddComponent<Rigidbody> ();
        frontcarRigid.isKinematic = true;
        frontcarGo.AddComponent<BoxCollider> ();

        //add Component
        MineTrainCar frontCar = frontcarGo.AddComponent<MineTrainCar> ();
        MineTrainCar car = carGo.AddComponent<MineTrainCar> ();

        frontCar.Decorate (true);
        car.Decorate (false);

        CoasterCarInstantiator coasterCarInstantiator = ScriptableObject.CreateInstance<CoasterCarInstantiator> ();
        List<CoasterCarInstantiator> trains = new List<CoasterCarInstantiator>();

        coasterCarInstantiator.name = "Mine Train@CoasterCarInstantiator";
        coasterCarInstantiator.defaultTrainLength = 5;
        coasterCarInstantiator.maxTrainLength = 7;
        coasterCarInstantiator.minTrainLength = 2;
        coasterCarInstantiator.carGO = carGo;
        coasterCarInstantiator.frontCarGO = frontcarGo;

        //register cars
        AssetManager.Instance.registerObject (frontCar);
        AssetManager.Instance.registerObject (car);
        registeredObjects.Add (frontCar);
        registeredObjects.Add (car);
        //Offset
        float CarOffset = .02f;
        car.offsetBack = CarOffset;
        frontCar.offsetBack = CarOffset;

        //Restraints
        RestraintRotationController controller = carGo.AddComponent<RestraintRotationController>();
        RestraintRotationController controllerFront = frontcarGo.AddComponent<RestraintRotationController>();
        controller.closedAngles = new Vector3(0,0,120);
        controllerFront.closedAngles = new Vector3(0, 0, 120);


        //Custom Colors
        Color[] CarColors = new Color[] { new Color(68f / 255, 58f / 255, 50f / 255), new Color(176f / 255, 7f / 255, 7f / 255), new Color(55f / 255, 32f / 255, 12f / 255),new Color(61f / 255, 40f / 255, 19f / 255)};

        MakeRecolorble(frontcarGo, "CustomColorsDiffuse", CarColors);
        MakeRecolorble(carGo, "CustomColorsDiffuse", CarColors);

        coasterCarInstantiator.displayName = "MineTrain Car";
        AssetManager.Instance.registerObject (coasterCarInstantiator);
        registeredObjects.Add (coasterCarInstantiator);

        trains.Add (coasterCarInstantiator);

        trackRider.carTypes = trains.ToArray();

        hider.SetActive (false);
        carGo.transform.parent = hider.transform;
        frontcarGo.transform.parent = hider.transform;


	}

    private void MakeRecolorble(GameObject GO, string shader, Color[] colors)
    {
        CustomColors cc = GO.AddComponent<CustomColors>();
        cc.setColors(colors);

        foreach (Material material in AssetManager.Instance.objectMaterials)
        {
            if (material.name == shader)
            {
                SetMaterial(GO, material);
                break;
            }
        }

    }

    private void SetMaterial(GameObject go, Material material)
    {
        // Go through all child objects and recolor     
        Renderer[] renderCollection;
        renderCollection = go.GetComponentsInChildren<Renderer>();

        foreach (Renderer render in renderCollection)
        {
            render.sharedMaterial = material;
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

