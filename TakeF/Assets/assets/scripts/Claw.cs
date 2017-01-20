using UnityEngine;
using System.Collections.Generic;

public class Claw : MonoBehaviour {

    public SphereCollider sphere;
    public Owl owl;
    float bounds;

    public List<GameObject> closeThings;

    // Use this for initialization
    void Start () {
        bounds = sphere.bounds.extents.magnitude;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "prey" || coll.gameObject.tag == "branch" && !closeThings.Contains(coll.gameObject))
        {
            closeThings.Add(coll.gameObject);
            Debug.Log(coll.name);
        }
    }

     void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "prey" || coll.gameObject.tag == "branch" && closeThings.Contains(coll.gameObject))
        {
            Debug.Log(coll.name);
            closeThings.Remove(coll.gameObject);
        }
    }

}
