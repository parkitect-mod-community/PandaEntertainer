using UnityEngine;
using System.Collections.Generic;

public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager = null;
    public static Configuration Configeration = null;

    GameObject hider;
    public void onEnabled()
    {

        hider = new GameObject ();

        UnityEngine.Debug.Log ("workling !!!");

		if (Main.AssetBundleManager == null) {

			AssetBundleManager = new AssetBundleManager (this);
		}

        TrackedRide selected = null;
        foreach (TrackedRide t in AssetManager.Instance.getAttractionObjects ()) {
            if (t.getUnlocalizedName() == "Wooden Coaster") {
                selected = t;
                break;
                    
            }
        }

        int pillar = 89;
        int center = 100;
        int crossbeam = 255;
        int beams = 175;

        TrackedRide trackRider = UnityEngine.Object.Instantiate (selected);

        MineTrainSupportInstantiator supportInstaiator = ScriptableObject.CreateInstance<MineTrainSupportInstantiator> ();
        AssetManager.Instance.registerObject (supportInstaiator);
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


        Color[] colors = new Color[] { new Color(pillar / 255f, pillar / 255f, pillar / 255f, 1), new Color(center / 255f, center / 255f, center / 255f, 1), new Color(crossbeam / 255f, crossbeam / 255f, crossbeam / 255f, 1), new Color(beams / 255f, beams / 255f, beams / 255f, 1) };
        trackRider.meshGenerator.customColors = colors;
        trackRider.meshGenerator.customColors = colors;
        trackRider.setDisplayName("MineTrain Coaster");
        trackRider.price = 3600;
        trackRider.name = "Corkscrew_coaster_GO";
        AssetManager.Instance.registerObject (trackRider);

        //get car
        GameObject carGo = Main.AssetBundleManager.FrontCarGo;
        Rigidbody carRigid = carGo.AddComponent<Rigidbody> ();
        carRigid.isKinematic = true;
        carGo.AddComponent<BoxCollider> ();

        GameObject frontcarGo = Main.AssetBundleManager.BackCarGo;
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
        Color[] CarColors = new Color[] { new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255), new Color(73f / 255, 73f / 255, 73f / 255) };

        MakeRecolorble(frontcarGo, "CustomColorsDiffuse", CarColors);
        MakeRecolorble(carGo, "CustomColorsDiffuse", CarColors);

        coasterCarInstantiator.displayName = "MineTrain Car";
        AssetManager.Instance.registerObject (coasterCarInstantiator);

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
	}

    public string Name
    {
        get { return "Mine Train Coaster"; }
    }

    public string Description
    {
        get { return "Allows the User to modify track Path"; }
    }


	public string Path { get; set; }

}

