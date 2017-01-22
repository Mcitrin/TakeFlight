using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Owl : MonoBehaviour
{
    public enum OwlStates { flying, glide, landing, landed, attacking, branching, branched}

    public OwlStates state = OwlStates.glide;

    public AnimationManager animationManager;

    public float speed = 5.0f;
    public Camera cam;
    public float faceingUp;
    public Transform camLead;
    public Transform camReset;
    public GameObject head;
    public Claw claw;
    public GameObject blood;

    float MAX_SPEED = 35;
    float MIN_SPEED = 5;
    float distToGround;
    float maxDist;
    float dif;

    Vector3 landingPoint;
    Vector3 moveCamTo;

    GameObject prey;

    bool still = false;

    // Use this for initialization
    void Start()
    {
        //animationManager.setBool("glide", true);
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        maxDist = (distToGround + 10);
    }

    void FlightCam()
    {

        if (Input.GetButton("R3") && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            cam.transform.rotation = Quaternion.LookRotation(Vector3.down, transform.forward);
            cam.transform.position = transform.position + Vector3.down * 2;
        }
        else
        {
            moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
            float bias = 0.96f;

            cam.transform.position = cam.transform.position * bias +
                                             moveCamTo * (1.0f - bias);


            cam.transform.LookAt(transform.position + transform.forward * 30.0f);
        }

    }
    void GroundCam()
    {
        Vector3 norm = Vector3.Normalize(head.transform.position - cam.transform.position);
        float angle = -Mathf.Asin(norm.y);

        cam.transform.LookAt(head.transform);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camLead.transform.position, .5f);

        if (Input.GetAxis("RVertical") > 0) // rotate up
        {
            if (angle < (Mathf.PI * (1.0f / 4.0f)))
            {
                camLead.transform.RotateAround(head.transform.position, cam.transform.right, Input.GetAxis("RVertical"));
                //camReset.transform.RotateAround(head.transform.position, cam.transform.right, Input.GetAxis("RVertical"));
            }
        }
        if (transform.position.y > camLead.transform.position.y - .25f)
        {
            camLead.transform.position = new Vector3(camLead.transform.position.x, transform.position.y + .25f, camLead.transform.position.z);
            //camReset.transform.position = new Vector3(camReset.transform.position.x, transform.position.y + .25f, camReset.transform.position.z);
        }

        if (transform.position.y < camLead.transform.position.y - .25f)
        {
            if (Input.GetAxis("RVertical") < 0) // rotate down
            {
                if (angle > -(Mathf.PI * (1.0f / 4.0f)))
                {
                    camLead.transform.RotateAround(head.transform.position, cam.transform.right, Input.GetAxis("RVertical"));
                    //camReset.transform.RotateAround(head.transform.position, cam.transform.right, Input.GetAxis("RVertical"));
                }
            }
        }

        if (still || state == OwlStates.branched) // owl is still
        {
            camLead.transform.RotateAround(head.transform.position, Vector3.up, Input.GetAxis("RHorizontal"));
        }
        else // owl is moveing
        {
            //camLead.transform.position = Vector3.MoveTowards(camLead.transform.position, camReset.transform.position, .5f);
            transform.Rotate(0.0f, (int)Input.GetAxis("RHorizontal") * 2.5f, 0.0f);
        }
        
    }

    void GetFlightInput()
    {
        SearchForBranch();

        // gravitys effect on speed
        speed -= transform.forward.y * Time.deltaTime * 20.0f;

        // move forward
        transform.position += transform.forward * Time.deltaTime * speed;


       RaycastHit hit;
       if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist))
       {
            if (claw.prey.Count != 0)
            {
                SearchForPrey();
                animationManager.setBool("catch", true);
            }
            else if (hit.transform.gameObject.tag == "ground")
           {
                    landingPoint = hit.point;
                    state = OwlStates.landing;
           }
       }

        // pitch
        if (Input.GetAxis("Vertical") > 0) // rotate down
        {

            if (faceingUp > -1)
                transform.RotateAround(transform.position,
                transform.right, Input.GetAxis("Vertical") * 2.5f * (Time.deltaTime + 1));
        }

        if (Input.GetAxis("Vertical") < 0) // rotate up
        {
            if (faceingUp < 1)
                transform.RotateAround(transform.position,
                transform.right, Input.GetAxis("Vertical") * 2.5f * (Time.deltaTime + 1));
        }

        // yaw
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.RotateAround(transform.position, Vector3.up, (Input.GetAxis("Horizontal") * 1.5f * (Time.deltaTime + 1)));
            //transform.RotateAround(transform.position, Vector3.up, (Input.GetAxis("RHorizontal") * 1.5f * (Time.deltaTime + 1)));

            if (Input.GetAxis("Horizontal") == -1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, (Vector3.up + -Camera.main.transform.right).normalized), 5 * Time.deltaTime);
            }
            else if (Input.GetAxis("Horizontal") == 1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, (Vector3.up + Camera.main.transform.right).normalized), 5 * Time.deltaTime);
            }

        }

        else
        {
            if (transform.rotation != Quaternion.LookRotation(transform.forward, Vector3.up))
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 5 * Time.deltaTime);
        }

        // clamp speed
        if (speed < MIN_SPEED) // min speed
        {
            speed = MIN_SPEED;
        }

        else if (speed > MAX_SPEED) // max speed
        {
            speed = MAX_SPEED;
        }
    }

    void GetGroundInput()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            still = false;
            animationManager.ChangeState("idle", "walk");
        }
        else
        { 
        still = true;
        }

        if (Input.GetButtonDown("A"))
        {
            TakeOff();
        }
        
        transform.position += (transform.forward * Input.GetAxis("Vertical") / 4.5f);
        transform.position += (transform.right * Input.GetAxis("Horizontal") / 4.5f);
    }

    void UpdateAimationMan()
    {
        animationManager.ResetAllBools();
        if (state == OwlStates.flying)
            animationManager.setBool("flying", true);
        else if (state == OwlStates.glide)
            animationManager.setBool("glide", true);
        else if (state == OwlStates.landing || state == OwlStates.branching)
            animationManager.setBool("land", true);
        else if (state == OwlStates.landed || state == OwlStates.branched)
            animationManager.setBool("idle", true);
        else if (state == OwlStates.attacking )
            animationManager.setBool("catch", true);
    }

    void UpdateState()
    {
        switch (state)
        {
            case OwlStates.flying:
                GetFlightInput();
                FlightCam();

                transform.position += Vector3.up * Time.deltaTime * 15;
                if (Input.GetButtonUp("A"))
                    state = OwlStates.glide;
                break;

            case OwlStates.glide:
                GetFlightInput();
                FlightCam();

                if (Input.GetButton("A"))
                    state = OwlStates.flying;
                break;

            case OwlStates.landing:
                Land();
                break;

            case OwlStates.landed:
                GroundCam();
                GetGroundInput();
                break;

            case OwlStates.attacking:
                Land();
                Debug.Log("attack");
                if (Vector3.Distance(transform.position,landingPoint) < distToGround)
                {
                    StartCoroutine(Spurt());
                    prey.GetComponent<NavMeshAgent>().Stop();
                    prey.GetComponent<Agent>().state = Agent.AgenStates.dead;

                    state = OwlStates.landed;
                    transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                    speed = 20.0f;

                    
                    

                    claw.prey.Clear();
                    //blood.SetActive(false);
                }
                break;
            case OwlStates.branching:
                Land();
                if (transform.position == landingPoint)
                {
                    animationManager.setBool("land", true);
                    state = OwlStates.branched;
                    transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                    speed = 20.0f;
                }
                break;
            case OwlStates.branched:
                GroundCam();
                if (Input.GetButtonDown("A"))
                {
                    TakeOff();
                }
                    break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" && state == OwlStates.landing)
        {
            animationManager.setBool("land", true);
            state = OwlStates.landed;
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            speed = 20.0f;
        }
        if (collision.gameObject.tag == "static" && state == OwlStates.attacking)
        {
            state = OwlStates.glide;
        }
    }

    void Land()
    {
        transform.position = Vector3.MoveTowards(transform.position, landingPoint, .5f);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), Vector3.up), Time.deltaTime + .025f);

        //FlightCam();
        cam.transform.LookAt(head.transform);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camLead.transform.position, .5f);
    }

    void TakeOff()
    {
        animationManager.setBool("takeOff", true);

        speed = 20.0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 100 * Time.deltaTime);
        this.GetComponent<Rigidbody>().useGravity = false;
        state = OwlStates.flying;

        transform.position += Vector3.up * Time.deltaTime * 5;
    }

    void SearchForBranch()
    {
        // if were close to a branch
        //for (int i = 0; i < claw.branches.Count; i++)
        //{
            if (claw.branches.Count != 0)
            {
                if (Vector3.Distance(transform.position, claw.branches[0].transform.position) <= maxDist/2)
                {
                    state = OwlStates.branching;
                    landingPoint = claw.branches[0].transform.position + Vector3.up * .5f;
                    claw.branches.Clear();
                }
            }
        //}
    }

    void SearchForPrey()
    {
        float neer = 100;
        GameObject target = null;

        for (int i = 0; i < claw.prey.Count; i++)
        {
            if(Vector3.Distance(transform.position,claw.prey[i].transform.position + transform.forward * 5) < neer)
            {
                neer = Vector3.Distance(transform.position, claw.prey[i].transform.position + transform.forward * 5);
                target = claw.prey[i];
            }

            landingPoint = target.transform.position;
            state = OwlStates.attacking;
            prey = target;
        }
    }
    
    IEnumerator Spurt()
    {
        blood.SetActive(true);
        yield return new WaitForSeconds(1);
        blood.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {        
        UpdateAimationMan();
        UpdateState();

        dif = Vector3.Dot(Vector3.up, transform.forward);
        faceingUp = Mathf.Round((dif) * 4) / 4;

        

    }


}
