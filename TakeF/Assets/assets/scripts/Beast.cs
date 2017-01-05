using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Beast : MonoBehaviour
{


    public enum AgenStates { idle, flee, wander, dead, seak, eat };
    public AgenStates state = AgenStates.wander;
    Vector3 target;

    public Node start;

    float terrainHeightAtOurPosition;

    public Node targetNode;

    public GameObject tagetObj;
    GameObject owl;

    bool walk;
    bool idle;
    public bool dead;

    public SphereCollider sphere;
    float bounds;

    bool waiting;
    bool wandering;

    public float futerTime;
    NavMeshAgent agent;

    public float radious = 50;

    public Animator anim;

    public GameObject eatingItem;

    public List<GameObject> closeThings;

    public float speed;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Random.seed = (int)System.DateTime.Now.Ticks;

        targetNode = start;
        target = start.transform.position;

        walk = true;
        anim.SetBool("idle", false);
        anim.SetBool("walk", true);

        owl = GameObject.FindWithTag("bird");

        bounds = sphere.bounds.extents.magnitude;

        agent.speed = speed;

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "pickUp" || coll.gameObject.tag == "bird" && !closeThings.Contains(coll.gameObject))
        {
            closeThings.Add(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

     


        terrainHeightAtOurPosition = Terrain.activeTerrain.SampleHeight(transform.position);
         manageCloseThings();
        if (state != AgenStates.eat)
        {
            agent.destination = (target);
           
            manageBools();
         }

        switch (state)
        {
            case AgenStates.idle:
                
                if (!waiting)
                {
                    StartCoroutine(wait());
                }

                break;
            case AgenStates.seak:

                if (Vector3.Distance(transform.position, tagetObj.transform.position) <= 5)
                    state = AgenStates.eat;

                agent.speed = speed + 10;

                target = (tagetObj.transform.position);

                if (Vector3.Distance(transform.position, tagetObj.transform.position) >= 90)
                    state = AgenStates.wander;

                break;

            case AgenStates.wander:

                agent.speed = speed;

                if (!wandering)
                    target = GetTarget();

                if (Vector3.Distance(target, transform.position) <= 5)
                {
                    target = GetTarget();
                    //Debug.Log(GetTarget());
                }

                if (futerTime == 0)
                {
                    futerTime = (Random.Range(3, 20) + Time.time);
                }

                if (Time.time >= futerTime)
                {
                    agent.Stop();
                    state = AgenStates.idle;
                }
                break;

            case AgenStates.eat:
                
                    agent.Stop();

                if (Vector3.Distance(transform.position, tagetObj.transform.position) > 5)
                {
                    agent.Resume();
                    state = AgenStates.seak;
                }


                break;
        
    }

  }
    void manageCloseThings()
    {
        for (int i = 0; i < closeThings.Count; i++)
        {
            if (closeThings[i] != null)
            {
                if (Vector3.Distance(transform.position, closeThings[i].transform.position) <= radious)
                {
                  

                    if(tagetObj == owl && Vector3.Distance(transform.position, owl.transform.position) > radious && closeThings[i] != owl)
                        tagetObj = closeThings[i];
               
                    else if (tagetObj == null)
                        tagetObj = closeThings[i];

                    else if (tagetObj != owl.gameObject)
                    {
                        if (closeThings[i] == owl)
                            tagetObj = closeThings[i];

                        if (Vector3.Distance(transform.position, closeThings[i].transform.position) <
                            Vector3.Distance(transform.position, tagetObj.transform.position) && tagetObj != closeThings[i])
                            tagetObj = closeThings[i];
                    }

                    if (state != AgenStates.eat)
                    {
                        agent.Resume();

                        state = AgenStates.seak;
                    }
                }

                if (Vector3.Distance(closeThings[i].transform.position, this.transform.position) > bounds)
                    closeThings.Remove(closeThings[i]);
            }
            else
            closeThings.Remove(closeThings[i]);
        }
    }
    void manageBools()
    {


        if (state != AgenStates.idle) // not ideling
        {
            anim.SetBool("idle", false);

        }
        else
        {
            anim.SetBool("idle", true); // ideling

        }
        if (state == AgenStates.wander) // wandering
        {
            anim.SetBool("walk", true);
            anim.SetBool("run", false);
        }

        if (state == AgenStates.eat) // eating
        {
            anim.SetBool("eat", true);
        }
        else                       // not eating
        {
            anim.SetBool("eat", false);
        }

        if (state == AgenStates.seak) // seaking
        {

            anim.SetBool("run", true);
            anim.SetBool("walk", false);
        }
        else
        {
            anim.SetBool("run", false);
        }

        if (state != AgenStates.wander ) // not wandering
        {
            anim.SetBool("walk", false);
            futerTime = 0;
            wandering = false;
        }

    }

    public Vector3 GetTarget()
    {
        wandering = true;

        targetNode = targetNode.getNodes()[Random.Range(0, targetNode.getNodes().Count)];
        if (targetNode != null)
            return targetNode.transform.position;
        else
        {
            return new Vector3(0, 0, 0);
        }
       
    }

    IEnumerator wait()
    {
        waiting = true;

        yield return new WaitForSeconds(Random.Range(3, 10));

        //if (!dead)
        //{
            state = AgenStates.wander;
            agent.Resume();
        //}
    }
}
