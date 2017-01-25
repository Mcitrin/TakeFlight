using UnityEngine;
using System.Collections.Generic;

public class Claw : MonoBehaviour {

    public SphereCollider sphere;
    float bounds;

    public List<GameObject> branches;
    public List<GameObject> prey;
    public GameObject heildItem;

    public Transform claws;

    // Use this for initialization
    void Start () {
        bounds = sphere.bounds.extents.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "prey"  && !prey.Contains(coll.gameObject))
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
            if(coll.gameObject != heildItem)
            prey.Remove(coll.gameObject);
        }
        else if (coll.gameObject.tag == "branch" && branches.Contains(coll.gameObject))
            branches.Add(coll.gameObject);
    }

    public void PickUp(bool pickUp)
    {
        if (pickUp)
        {


            if(heildItem == null)
            heildItem = prey[0].gameObject;

            heildItem.GetComponent<Rigidbody>().isKinematic = true;
            heildItem.transform.SetParent(gameObject.transform);
            heildItem.transform.position = transform.position;

            // if alive kill it
            if (heildItem.GetComponent<NavMeshAgent>().enabled)
            {
                heildItem.GetComponent<NavMeshAgent>().Stop();
                heildItem.GetComponent<NavMeshAgent>().enabled = false;
                heildItem.GetComponent<Agent>().dead = true;
                heildItem.GetComponent<Agent>().state = Agent.AgenStates.dead;
                
            }

        }
        else 
        {
            if (heildItem != null)
            {
                heildItem.transform.SetParent(null);
                heildItem.GetComponent<Rigidbody>().isKinematic = false;
                heildItem = null;
                
            }
                //Debug.Log("drop");
            
        }


    }

}
