using UnityEngine;
using System.Collections.Generic;

public class grab : MonoBehaviour
{

    public Movement bird;
    public GameObject heildItem;
    public SphereCollider sphere;
    float bounds;

    public Transform claws;

    public GameObject eatingItem;

    public List<GameObject> closeThings;

    // Use this for initialization
    void Start()
    {
        bounds = sphere.bounds.extents.magnitude;
    }

    void pickUp(bool pickup)
    {
        bool onOff;

        if (pickup)
        {
            //heildItem.GetComponent<Agent>().anim.SetBool("dead", true);
            //heildItem.GetComponent<Agent>().enabled = false;

         
            heildItem = closeThings[0].gameObject;
            onOff = false;
            heildItem.transform.SetParent(this.gameObject.transform);

            
            heildItem.GetComponent<Agent>().dead = true;
            heildItem.GetComponent<Agent>().state = Agent.AgenStates.dead;

            

            if (heildItem.GetComponent<NavMeshAgent>().enabled)
            {
                heildItem.GetComponent<NavMeshAgent>().Stop();
                heildItem.GetComponent<NavMeshAgent>().enabled = false;
                heildItem.transform.position = claws.position;
            }

            //heildItem.GetComponent<Rigidbody>().isKinematic = true;

        }
        else
        {
            onOff = true;
            heildItem.transform.SetParent(null);
            heildItem = null;
            //heildItem.GetComponent<Rigidbody>().isKinematic = false;
        }


    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "pickUp" && !closeThings.Contains(coll.gameObject))
        {
            closeThings.Add(coll.gameObject);
        }
    }

    void Update()
    {

        //if(Input.GetButton("R2(button)"))
        //{
        //    Debug.Log("r2");
        //}


        // Input.GetJoystickNames()[0] == ("Controller (Xbox One For Windows)")     check wich joystick is pluged in

        for (int i = 0; i < closeThings.Count; i++)
        {
            if (Vector3.Distance(closeThings[i].transform.position, this.transform.position) > bounds)
                closeThings.Remove(closeThings[i]);
        }

        if (bird.bState != Movement.BirdState.landed)/// flying
        {
            if (closeThings.Count > 0 && heildItem == null)
            {
              //if ( Input.GetButton("R2(button)")) // Input.GetAxis("R2") == 1)
              //{
                 if (heildItem == null)
                     pickUp(true);
               //}
            }
           //else if (heildItem != null)
           //{
           //    if( !Input.GetButton("R2(button)"))  // Input.GetAxis("R2") != 1)
           //        pickUp(false);
           //}

        }
        else ///// landede
        {

            if (Input.GetButton("X"))
            {
                foreach (GameObject item in closeThings)
                {

                    if (item.GetComponent<Agent>() != null)
                    {

                        if (eatingItem == null && item.GetComponent<Agent>().dead)
                        {
                            eatingItem = item;
                        }
                    }
                }

            }
            else
            {
                eatingItem = null;
            }




            if (heildItem != null)
            {
                pickUp(false);
            }
        }





        if (heildItem != null)
        {
            heildItem.transform.position = claws.position;
        }
    }



}

