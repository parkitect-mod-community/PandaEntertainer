using UnityEngine;
using System.Collections.Generic;
using TrackedRiderUtility;

public class Main : IMod
{
    public string Identifier { get; set; }
	public static AssetBundleManager AssetBundleManager = null;
    private TrackRiderBinder binder;

    public void onEnabled()
    {
       
        hider = new GameObject ();

		if (Main.AssetBundleManager == null) {

			AssetBundleManager = new AssetBundleManager (this);
		}

        binder = new TrackRiderBinder ("750af72f6eff659e238b2a5c8826e3c8");

        TrackedRide trackedRide = binder.RegisterTrackedRide<TrackedRide> ("Wooden Coaster","SideFrictionCoaster", "Side Friction Coaster");
        trackedRide.price = 3600;
        trackedRide.dropsImportanceExcitement = .7f;
        trackedRide.inversionsImportanceExcitement = .67f;
        trackedRide.averageLatGImportanceIntensity = .7f;
        trackedRide.maxBankingAngle = 20;
        trackedRide.accelerationVelocity = .09f;
        trackedRide.carTypes = new CoasterCarInstantiator[]{ };

        SideFrictionTrackGenerator meshGenerator = binder.RegisterMeshGenerator<SideFrictionTrackGenerator> (trackedRide);
        TrackRideHelper.PassMeshGeneratorProperties (TrackRideHelper.GetTrackedRide ("Wooden Coaster").meshGenerator,trackedRide.meshGenerator);
        trackedRide.meshGenerator.customColors = new Color[] {
            new Color (68f / 255f, 47f / 255f, 37f / 255f, 1),
            new Color (74f / 255f, 32f / 255f, 32f / 255f, 1),
            new Color (66f / 255f, 66f / 255f, 66f / 255f, 1)
        };

        CoasterCarInstantiator coasterCarInstantiator = binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator> (trackedRide, "SideFrictionInstantiator", "Side Friction Car", 5, 7, 2);

        BaseCar car = binder.RegisterCar<BaseCar> (Main.AssetBundleManager.Car, "SideFrictionCar", .25f,.02f, true, new Color[] 
            { new Color(0f / 255, 4f / 255, 190f / 255), 
                new Color(138f / 255, 15f / 255, 15f / 255), 
                new Color(101f / 255, 21f / 255, 27f / 255),
                new Color(172f / 255, 41f / 255, 42f / 255)
            });

        car.gameObject.AddComponent<RestraintRotationController>().closedAngles = new Vector3(0,0,120);
        coasterCarInstantiator.carGO = car.gameObject;

        binder.Apply ();
        //deprecatedMappings
        string oldHash = "a9sfj-[a9w34ainw;kjasinda";

        GameObjectHelper.RegisterDeprecatedMapping ("side_friction_GO", trackedRide.name);
        GameObjectHelper.RegisterDeprecatedMapping ("Side Friction@CoasterCarInstantiator"+oldHash, coasterCarInstantiator.name);
        GameObjectHelper.RegisterDeprecatedMapping ("SideFriction_Car"+oldHash, car.name);

	}

    public void onDisabled()
    {
        binder.Unload ();
	}

    public string Name
    {
        get { return "Side Friction Coaster"; }
    }

    public string Description
    {
        get { return "An early wooden coaster design that used a trough/side rail design that isn't use anymore in most modern day coaster. "; }
    }


	public string Path { get; set; }

}

