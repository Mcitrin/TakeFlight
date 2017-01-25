using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {


    public enum AgenStates { idle, flee, wander, dead, seak };
    public AgenStates state = AgenStates.idle;
    Vector3 target;

    public Node targetNode;

    public Owl owl;

    bool walk;
    bool idle;
    public bool dead;

    bool waiting;
    bool wandering;

   public float futerTime;
    NavMeshAgent agent;

    public float radious = 80;

    Vector3 prev;

    public Animator anim;

    public GameObject body;
    //public AnimationClip walkClip;
    //public AnimationClip idleCilp;
    //public AnimationClip deadClip;






    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        Random.seed = (int)System.DateTime.Now.Ticks;

        owl = GameObject.FindWithTag("bird").GetComponent<Owl>();       
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        //if (owl.claws.heildItem != null)
        //{
        //    if (owl.claws.heildItem == gameObject)
        //    {
        //        dead = true;
        //        state = AgenStates.dead;
        //    }
        //}

        if (!dead & state != AgenStates.dead)
        {

            agent.destination = (target);

            if (Vector3.Distance(transform.position, owl.transform.position) <= radious && state != AgenStates.flee)
            {
                agent.Resume();

                state = AgenStates.flee;
            }

            

            manageBools();
        }

        switch (state)
        {
            case AgenStates.idle:
                 

                idle = true;

                if (!waiting)
                {
                    StartCoroutine(wait());
                }

                break;
            case AgenStates.flee:
                


                Vector3 direction = (owl.transform.position - transform.position).normalized;

                target = (transform.position + direction * -10);

                if (Vector3.Distance(transform.position, owl.transform.position) >= 150 && !dead)
                    state = AgenStates.wander;

                break;

            case AgenStates.wander:
                

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

            case AgenStates.dead:

                anim.SetBool("dead", true);

               //if (owl.claw.heildItem != gameObject)
               //{
               //    if (transform.position.y > terrainHeightAtOurPosition)
               //        transform.position -= Vector3.up * .75f;
               //
               //    if (transform.position.y < terrainHeightAtOurPosition)
               //        transform.position = new Vector3(transform.position.x, terrainHeightAtOurPosition, transform.position.z);
               //
               //}
                dead = true;
                break;

            default:
                break;
        }



       // if (Time.time >= 3 && !changed)
       // {
       //     
       //    target.position = (target2.position);
       //     
       //     
       //     changed = true;
       // }
       // elseagent.destination = (target.position);
       //     


    }

  

    void manageBools()
    {
       

        if (state != AgenStates.idle)
        {
            waiting = false;
            idle = false;
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);

        }
        else
        {
            idle = true;
            anim.SetBool("idle", true);

        }

        if (state != AgenStates.wander && state != AgenStates.flee)
        {
            walk = false;
            anim.SetBool("walk", false);
        }
        else
        {
            walk = true;
            anim.SetBool("walk", true);


        }

        if (state != AgenStates.wander)
        {
            futerTime = 0;
            wandering = false;
        }

    }

    public Vector3 GetTarget()
    {
        wandering = true;

        //float dist = 0;
        //Vector3 vec = new Vector3(0,0,0);

        //foreach (Transform item in targets)
        //{
        //    if (Vector3.Distance(item.position, owl.transform.position) > dist && item.position != target && item.position != prev)
        //    {
        //        vec = item.transform.position;
        //        dist = Vector3.Distance(item.position, owl.transform.position);
        //
        //    }
        //    if (Random.value < .5)
        //        dist = 0;
        //}


        targetNode = targetNode.getNodes()[Random.Range(0, targetNode.getNodes().Count)];
        if (targetNode != null)
            return targetNode.transform.position;
        else
        {
            Debug.Log(gameObject.name);
           return new  Vector3(0, 0, 0);
        }
        //prev = vec;
        //return vec;

    }

    IEnumerator wait()
    {
        waiting = true;

        yield return new WaitForSeconds(Random.Range(3, 10));

        if (!dead)
        {
            state = AgenStates.wander;
            agent.Resume();
        }
    }
}
