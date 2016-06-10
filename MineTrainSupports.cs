using System;
using UnityEngine;

public class MineTrainSupports : Support
{
    protected GameObject instance;

    public float x;

    public float y;

    public float z;

    public int crossedTiles;

    public Material baseMaterial;
    protected override void createInstanceObject ()
    {
        if (this.instance != null) {
            UnityEngine.Object.Destroy (instance);
        }
        this.instance = new GameObject ("instance");
        this.instance.transform.parent = base.transform;
        this.instance.transform.localPosition = Vector3.zero;
        this.instance.transform.localRotation = Quaternion.identity;

    }

    protected override void build ()
    {


        int index = 0;
        base.build ();

        Vector3 position = new Vector3((int)this.x + 0.5f, this.y, (int)this.z + 0.5f);
        LandPatch terrain = GameController.Instance.park.getTerrain(position);
        if (terrain == null)
            return;

        int lowest = terrain.getLowestHeight ();

        int start = Mathf.FloorToInt (this.y) - 1;
        if (start < lowest)
            return;

        for (int i = start; i >= lowest; i--) {
            index++;

            GameObject support;

            if (this.crossedTiles == 9 || this.crossedTiles == 3 || this.crossedTiles == 6 || this.crossedTiles == 12) {
                support = UnityEngine.Object.Instantiate<GameObject>(Main.AssetBundleManager.AngledSupportGo[(int)index % Main.AssetBundleManager.AngledSupportGo.Length]);
            } else {
                support = UnityEngine.Object.Instantiate<GameObject>(Main.AssetBundleManager.Support_Go[(int)index % Main.AssetBundleManager.Support_Go.Length]);
            }


            position.y = (float)i ;
            support.transform.position = position;
            support.transform.parent = this.transform;
            support.isStatic = true;

            if (this.crossedTiles == 9)
                support.transform.Rotate (Vector3.forward, 180f);
            else if (this.crossedTiles == 3)
                support.transform.Rotate (Vector3.forward, 90f);
            else if(this.crossedTiles == 6)
                support.transform.Rotate (Vector3.forward, 0f);
            else if(this.crossedTiles == 12)
                support.transform.Rotate (Vector3.forward, -90f);
            else
                support.transform.Rotate (Vector3.forward, ((index + 4) % 4) * 90f);




            support.GetComponent<Renderer> ().sharedMaterial = baseMaterial;
            GameObjectHelper.SetUV (support, 14, 15);

            BoundingBox boundingBox2 = support.AddComponent<BoundingBox>();
            boundingBox2.layers = BoundingVolume.Layers.Support;
            boundingBox2.setBounds(new Bounds(Vector3.up / 2f, Vector3.one));
            boundingBox2.setManuallyPositioned();
            boundingBox2.setPosition(position, support.transform.rotation);
            this.boundingVolumes.Add(boundingBox2);
        }


    }


    protected override void hideUnhide(bool possiblyCollidesWithNewObject)
    {
        foreach (BoundingVolume current in this.boundingVolumes)
        {
            if (Collisions.Instance.check(current, BoundingVolume.Layers.Buildvolume))
            {
                current.gameObject.SetActive(false);
            }
            else
            {
                current.gameObject.SetActive(true);
            }
        }
    }
}


