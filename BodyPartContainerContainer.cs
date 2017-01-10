using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace PandaEntertainer
{
    public class BodyPartContainerContainer
    {
        public enum PrefabType{
            ENTERTAINER
        }

        private BodyPartsContainer bodyContainer;

        private List<GameObject> torsos = new List<GameObject>();
        private List<GameObject> heads = new List<GameObject>();
        private List<GameObject> legs = new List<GameObject>();
        private List<GameObject> hairstyles = new List<GameObject>();

        private Employee employee;

        public BodyPartContainerContainer (string name,PrefabType type)
        {
            bodyContainer = ScriptableObject.CreateInstance<BodyPartsContainer> ();
            bodyContainer.name = name;

			employee = AssetManager.Instance.getPrefab<Employee> (Prefabs.Entertainer);

        }

        public void AddTorso(GameObject torso)
        {

            GameObject m = remap (employee.costumes [0].bodyPartsMale.getTorso (0), torso);
            torsos.Add(m);
        }

        public void AddHeads(GameObject head)
        {
            GameObject m = remap (employee.costumes [0].bodyPartsMale.getHead (0), head);
            heads.Add(m);
        }

        public void AddLegs(GameObject leg)
        {
            GameObject m = remap (employee.costumes [0].bodyPartsMale.getLegs (0), leg);
            legs.Add(m);
        }

        public void AddHairstyles(GameObject hairstyle)
        {
			hairstyles.Add(RemapMaterial(employee.costumes[0].bodyPartsMale.getHairstyle(0),hairstyle));
        }

        private GameObject remap(GameObject duplicator, GameObject mappedTo)
        {
            GameObject go = GameObject.Instantiate (duplicator);

            SkinnedMeshRenderer skinnedMesh = go.GetComponentInChildren<SkinnedMeshRenderer> ();
            SkinnedMeshRenderer mappingMesh = mappedTo.GetComponentInChildren<SkinnedMeshRenderer> ();
    		

            if (skinnedMesh == null) {
                UnityEngine.Debug.Log ("does not have skinned mesh:" + mappedTo.name );
                return go;
            }

            Dictionary<int,string> oldMapping = new Dictionary<int, string> ();
            for (int x = 0; x < mappingMesh.bones.Length; x++) {
                oldMapping.Add (x, mappingMesh.bones [x].name);
            }

            List<Matrix4x4> bp = new List<Matrix4x4> ();
            Dictionary<String,int> newMapping = new Dictionary<String,int>();
            for (int x = 0; x < skinnedMesh.bones.Length; x++) {
                newMapping.Add (skinnedMesh.bones [x].name,x);

                Transform t = go.transform.FindRecursive (skinnedMesh.bones [x].name);
                if(t != null)
                    bp.Add (t.worldToLocalMatrix * go.transform.localToWorldMatrix);
                else
                    bp.Add (skinnedMesh.sharedMesh.bindposes[x]);

            }

            List<BoneWeight> boneWeights = new List<BoneWeight> ();
            for (int x = 0; x < mappingMesh.sharedMesh.boneWeights.Length; x++) {
                BoneWeight tempWeight = new BoneWeight ();
                tempWeight.boneIndex0 = Remapper (mappingMesh.sharedMesh.boneWeights [x].boneIndex0, newMapping, oldMapping);
                tempWeight.boneIndex1 = Remapper (mappingMesh.sharedMesh.boneWeights [x].boneIndex1, newMapping, oldMapping);
                tempWeight.boneIndex2 = Remapper (mappingMesh.sharedMesh.boneWeights [x].boneIndex2, newMapping, oldMapping);
                tempWeight.boneIndex3 = Remapper (mappingMesh.sharedMesh.boneWeights [x].boneIndex3, newMapping, oldMapping);

                tempWeight.weight0 =  mappingMesh.sharedMesh.boneWeights [x].weight0;
                tempWeight.weight1 =  mappingMesh.sharedMesh.boneWeights [x].weight1;
                tempWeight.weight2 =  mappingMesh.sharedMesh.boneWeights [x].weight2;
                tempWeight.weight3 =  mappingMesh.sharedMesh.boneWeights [x].weight3;
                boneWeights.Add (tempWeight);
                
            }

            Mesh tempMesh = UnityEngine.Object.Instantiate (skinnedMesh.sharedMesh);

            tempMesh.Clear ();
            tempMesh.vertices = mappingMesh.sharedMesh.vertices;
            tempMesh.uv = mappingMesh.sharedMesh.uv;
            tempMesh.triangles = mappingMesh.sharedMesh.triangles;
            tempMesh.RecalculateBounds();
            tempMesh.normals = mappingMesh.sharedMesh.normals;
            tempMesh.tangents = mappingMesh.sharedMesh.tangents;

			tempMesh.boneWeights = boneWeights.ToArray ();
            tempMesh.bindposes = bp.ToArray();
            skinnedMesh.sharedMesh = tempMesh;

			/*Material mat =  Material.Instantiate(skinnedMesh.sharedMaterial);
			mat.SetTexture("_MainTex", mappingMesh.material.GetTexture("_MainTex"));
			mat.SetTexture("_DetailAlbedoMap",mappingMesh.material.GetTexture("_MainTex"));
*/

			skinnedMesh.sharedMaterial = mappingMesh.material;



			return go;
        }


		public GameObject RemapMaterial(GameObject duplicator, GameObject mappedTo)
		{
            
			MeshRenderer skinnedMesh = duplicator.GetComponentInChildren<MeshRenderer>();
			MeshRenderer mappingMesh = mappedTo.GetComponentInChildren<MeshRenderer>();

			Material material= Material.Instantiate(skinnedMesh.sharedMaterial);
			material.mainTexture = mappingMesh.material.mainTexture;

			mappingMesh.sharedMaterial = material;
			return mappedTo; 
		}

        private int Remapper(int index, Dictionary<String,int> newMapping,Dictionary<int,String> oldMapping)
        {
            String boneName = oldMapping [index];
            if (newMapping.ContainsKey(boneName)) {
                return newMapping[boneName];
            } else {
                UnityEngine.Debug.Log ("can't find bone mapping:" + boneName);
            }
            return 0;
        }


        public BodyPartsContainer Apply()
        {
            typeof(BodyPartsContainer).GetField ("torsos", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, torsos.ToArray());
            typeof(BodyPartsContainer).GetField ("heads", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, heads.ToArray());
            typeof(BodyPartsContainer).GetField ("legs", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, legs.ToArray());
            typeof(BodyPartsContainer).GetField ("hairstyles", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer,hairstyles.ToArray());

            typeof(BodyPartsContainer).GetField ("accessories", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, new WearableProduct[]{});
            typeof(BodyPartsContainer).GetField ("headItems", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, new WearableProduct[]{});
            typeof(BodyPartsContainer).GetField ("faceItems", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic).SetValue (bodyContainer, new WearableProduct[]{});


            return bodyContainer;
        }

        public void Dispose()
        {
        }



    }
}

