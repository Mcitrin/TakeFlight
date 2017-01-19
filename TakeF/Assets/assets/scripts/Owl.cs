using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Owl : MonoBehaviour
{
    public enum OwlStates { flying, glide, landing, landed, attacking}

    public OwlStates state = OwlStates.glide;

    public AnimationManager animationManager;
    
    public float speed = 5.0f;
    public Camera cam;
    public float faceingUp;
    public Transform camReset;

    float MAX_SPEED = 35;
    float MIN_SPEED = 5;
    float distToGround;
    float maxDist;
    float percentageToGround;
    float dif;

    float futureTime;

    Vector3 landingPoint;
    Vector3 moveCamTo;

    

    // Use this for initialization
    void Start()
    {
        //animationManager.setBool("glide", true);
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        maxDist = (distToGround + 5);
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

    void GetFlightInput()
    {

        //Debug.Log(transform.forward);
        //
        //transform.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), Vector3.up);
        // gravity
        speed -= transform.forward.y * Time.deltaTime * 20.0f;

        // move forward
        transform.position += transform.forward * Time.deltaTime * speed;


        RaycastHit hit;
      if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist))
      {
          if (hit.transform.gameObject.tag == "ground")
          {
                //percentageToGround = -(((transform.position.y - hit.point.y) / maxDist) - 1);
                percentageToGround = -((Vector3.Distance(transform.position, hit.point) / maxDist) - 1);

                ////Debug.Log(percentageToGround);

                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.LookRotation(Vector3.up,-transform.forward), percentageToGround * Time.deltaTime + .025f);

            }
      }


        //if(futureTime != 0 && Time.time >= futureTime)
        //{
        //    state = OwlStates.landing;
        //    futureTime = 0;
        //}


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
            transform.RotateAround(transform.position, Vector3.up, (Input.GetAxis("Horizontal") * 2.5f * (Time.deltaTime + 1)));
            //transform.RotateAround(transform.position, Vector3.up, (Input.GetAxis("RHorizontal") * 2.5f * (Time.deltaTime + 1)));

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

        else

        if (speed > MAX_SPEED) // max speed
        {
            speed = MAX_SPEED;
        }
    }

    void GetGroundInput()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camReset.transform.position,.5f);
    }

    void UpdateAimationMan()
    {
        animationManager.ResetAllBools();
        if (state == OwlStates.flying)
            animationManager.setBool("flying", true);
        else if (state == OwlStates.glide)
            animationManager.setBool("glide", true);
        else if (state == OwlStates.landing)
            animationManager.setBool("land", true);
    }

    void UpdateState()
    {
        switch (state)
        {
            case OwlStates.flying:

                transform.position += Vector3.up * Time.deltaTime * 15;
                if (Input.GetButtonUp("A"))
                    state = OwlStates.glide;

                break;
            case OwlStates.glide:

                if (Input.GetButton("A"))
                    state = OwlStates.flying;

                break;
            case OwlStates.landing:

                // move forward
                //transform.position = Vector3.MoveTowards(transform.position, landingPoint + transform.position, .5f);
                
                FlightCam();
                break;
            case OwlStates.landed:
                
                break;
            case OwlStates.attacking:

                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" && state == OwlStates.landing)
        {
            state = OwlStates.landed;
            //resetRotation();
            
            speed = 20.0f;


        }
    }

    

    // Update is called once per frame
    void Update()
    {
       
        if (state == OwlStates.flying || state == OwlStates.glide)
        {
            GetFlightInput();
            FlightCam();
        }



        if (state == OwlStates.landed)
            GetGroundInput();

        UpdateAimationMan();
        UpdateState();


        dif = Vector3.Dot(Vector3.up, transform.forward);
        faceingUp = Mathf.Round((dif)*4)/4;

        //Debug.Log(Input.GetAxis("RVertical"));

    }


}
