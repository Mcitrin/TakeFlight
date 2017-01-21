using UnityEngine;
using System.Collections.Generic;

public class Claw : MonoBehaviour {

    public SphereCollider sphere;
    float bounds;

    public List<GameObject> branches;
    public List<GameObject> prey;

    // Use this for initialization
    void Start () {
        bounds = sphere.bounds.extents.magnitude;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "prey"  && !prey.Contains(coll.gameObject) && !coll.GetComponent<Agent>().dead)
        {
            prey.Add(coll.gameObject);
            //Debug.Log(coll.name);
        }

        else if (coll.gameObject.tag == "branch" && !branches.Contains(coll.gameObject))
            branches.Add(coll.gameObject);
    }

     void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "prey" && prey.Contains(coll.gameObject))
        {
            //Debug.Log(coll.name);
            prey.Remove(coll.gameObject);
        }
        else if (coll.gameObject.tag == "branch" && branches.Contains(coll.gameObject))
            branches.Add(coll.gameObject);
    }

}
