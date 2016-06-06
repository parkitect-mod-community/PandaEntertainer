using System;
using UnityEngine;

public class MineTrainSupports : Support
{
    protected GameObject instance;

    public int x;

    public int y;

    public int z;

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


        base.build ();

        Vector3 position = new Vector3((float)this.x + 0.5f, this.y, (float)this.z + 0.5f);
        LandPatch terrain = GameController.Instance.park.getTerrain(position);
        if (terrain == null)
            return;

        int lowest = terrain.getLowestHeight ();

        float offset = 0;
        if (((float)Mathf.RoundToInt (this.y * 2f) / 2f) > this.y) {
            offset -= .5f;
        }
        int num = Mathf.FloorToInt (this.y + offset) -1;
        if (num < lowest) {
            UnityEngine.Object.Destroy (base.gameObject);
            return;
        }
        for (int i = num; i >= lowest; i--) {
            GameObject box_frame = UnityEngine.Object.Instantiate<GameObject>(Main.AssetBundleManager.Support_Go[(int)i % Main.AssetBundleManager.Support_Go.Length]);
            position.y = (float)i ;
            box_frame.transform.position = position;
            box_frame.transform.parent = this.transform;
            box_frame.isStatic = true;
            box_frame.transform.Rotate(Vector3.forward,(i % 4) * 90f);



            box_frame.GetComponent<Renderer> ().sharedMaterial = baseMaterial;
            SetUV (box_frame, 14, 15);

            BoundingBox boundingBox2 = box_frame.AddComponent<BoundingBox>();
            boundingBox2.layers = BoundingVolume.Layers.Support;
            boundingBox2.setBounds(new Bounds(Vector3.up / 2f, Vector3.one));
            boundingBox2.setManuallyPositioned();
            boundingBox2.setPosition(position, box_frame.transform.rotation);
            this.boundingVolumes.Add(boundingBox2);
        }


    }


    private GameObject SetUV(GameObject GO, int gridX, int gridY)
    {
        Mesh mesh = GO.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.0625f * ((float)gridX + 0.5f), 1f - 0.0625f * ((float)gridY + 0.5f));
        }
        mesh.uv = uvs;
        return GO;
    }

    protected override void hideUnhide(bool possiblyCollidesWithNewObject = false)
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


