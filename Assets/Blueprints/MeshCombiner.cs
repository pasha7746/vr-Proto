using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshCombiner : MonoBehaviour
{
   

    public List<MeshFilter> combinedMeshFilters;


	// Use this for initialization
	void Start ()
    {
		CombineInstance[] combine= new CombineInstance[combinedMeshFilters.Count];
        for (int i = 0; i < combinedMeshFilters.Count; i++)
        {
            combine[i].mesh = combinedMeshFilters[i].sharedMesh;
            combine[i].transform = combinedMeshFilters[i].transform.localToWorldMatrix;
            combinedMeshFilters[i].gameObject.SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh= new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
