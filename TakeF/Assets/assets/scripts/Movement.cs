using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{

    public enum BirdState { glide, dive, climb, landing, landed, flap, flaping };
    public BirdState bState = BirdState.landed;
    public float speed = 0.0f;
    public Animation birdAnim;
    public float faceingUp;
    public float dif;

    float MAX_SPEED = 50;
    float MIN_SPEED = 5;

    Vector3 moveCamTo;
    float terrainHeightAtOurPosition;

    float terrainHeightAtCamPosition;


    public GameObject CamReset;
    public Camera flighCam;

    public grab claws;

    public bool still;
    bool reset;
    public bool eating;

    public bool grabbing;

    public GameObject head;
    public AnimationManager animationManager;
    public hungerMeter meter;

    public GameObject blood;

    public UiButtons uiButtons;

    float distToGround;

    Vector3 landingPoint;

    public List<Train> rains;

    Terrain turr;

    // Use this for initialization
    void Start()
    {
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        StartCoroutine(updateTurr());
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up + transform.forward * 5,out hit, distToGround + 25 ))
        {
            if (hit.transform.gameObject.tag == "ground")
            {
                landingPoint = hit.point;



                return true;
            }
        }



            return false;
        

        
    }



    void SwitchCams(bool flying)
    {

        if (flying)
            flighCam.gameObject.transform.parent = null;
        else
        {
            flighCam.gameObject.transform.parent = transform;
            StartCoroutine(resetOrbitalCam());
        }


        // flighCam.gameObject.SetActive(!flighCam.gameObject.active);
        // groundCam.gameObject.SetActive(!groundCam.gameObject.active);

    }

    IEnumerator TakeOffAndLand(bool takeOff)
    {

        if(takeOff)
        {
            animationManager.ChangeState("idle", "takeOff");
            animationManager.setBool("walk", false);
            yield return new WaitForSeconds(.5f);
            animationManager.setBool("takeOff", false);
        }
        else
        {
            animationManager.ChangeState("glide", "land");
            animationManager.setBool("flying", false);
            
        }

        
    }

    IEnumerator Eat()
    {
        
        if (claws.eatingItem != null)
        {
             Debug.Log(Time.time);
            yield return new WaitForSeconds(5f);
            Debug.Log(Time.time);

            claws.closeThings.Remove(claws.eatingItem);
            Destroy(claws.eatingItem);
            meter.fill.fillAmount += .5f;
            

            eating = false;
        }

        StopCoroutine("Eat");
            eating = false;
       
           
        
    }

    IEnumerator resetOrbitalCam()
    {


        flighCam.transform.position = Vector3.MoveTowards(flighCam.transform.position, CamReset.transform.position, .15f);
        if (flighCam.transform.position != CamReset.transform.position)
        {
            yield return new WaitForSeconds(.005f);
            StartCoroutine(resetOrbitalCam());
        }
        else
            yield return new WaitForSeconds(0);

    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" && bState == BirdState.landing)
        {
            bState = BirdState.landed;
            resetRotation();
            this.GetComponent<Rigidbody>().useGravity = true;
            speed = 20.0f;
            //animationManager.setBool("land", true);


        }
    }

    void UpdateFlightCam()
    {

        if (Input.GetButton("R3") && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            flighCam.transform.rotation = Quaternion.LookRotation(Vector3.down, transform.forward);
            flighCam.transform.position = transform.position + Vector3.down * 2;
        }
        else
        {
            moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
            float bias = 0.96f;

            flighCam.transform.position = flighCam.transform.position * bias +
                                             moveCamTo * (1.0f - bias);

            // if (bState != BirdState.flaping)
            flighCam.transform.LookAt(transform.position + transform.forward * 30.0f);
            // else
            // flighCam.transform.LookAt(transform.position);
        }

    }

    void resetRotation()
    {
        //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, transform.localEulerAngles.y, 0,0),100);
        transform.rotation =  Quaternion.Euler(0, transform.localEulerAngles.y, 0); //// WOOOOO!!!!!!! its works now

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    }

    void UpdateButtons()
    {
        if (bState != BirdState.landed && bState != BirdState.landing)// if flying
        {

            

            if (IsGrounded()) /// if can land
            {
                uiButtons.OnOff(true, uiButtons.land);
            }
            else
            {
                uiButtons.OnOff(false, uiButtons.land);
            }

            if (claws.closeThings.Count > 0 && claws.heildItem == null)
            {
                uiButtons.OnOff(true, uiButtons.grab);
            }
            else
            {
                uiButtons.OnOff(false, uiButtons.grab);
            }


        }
        else /// not flying
        {
            uiButtons.OnOff(false, uiButtons.grab);
            uiButtons.OnOff(false, uiButtons.land);
        }

         if (bState == BirdState.landed && claws.closeThings.Count > 0) 
        {
            uiButtons.OnOff(true, uiButtons.eat);
        }
         else
        {
            uiButtons.OnOff(false, uiButtons.eat);
        }

        if(bState == BirdState.landing && bState != BirdState.landed)
       {
           uiButtons.OnOff(false, uiButtons.flap);
       }
        else
       {
           uiButtons.OnOff(true, uiButtons.flap);
       }

        

    }

    IEnumerator turnForward()
    {
        

        flighCam.gameObject.transform.parent = null;

        //transform.rotation = (Quaternion.Euler(0, flighCam.transform.eulerAngles.y, 0));

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);



        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        yield return new WaitForSeconds(0);

        flighCam.gameObject.transform.parent = transform;
        flighCam.transform.position = CamReset.transform.position;
        reset = true;


    }

    IEnumerator updateTurr()
    {

        turr = GetTerrain();
        

        yield return new WaitForSeconds(30);
        

        StartCoroutine(updateTurr());
    }

    Terrain GetTerrain()
    {
        Terrain rain = new Terrain();
        float dist;
        float close = 9999999;

        foreach (Train item in rains)
        {
            dist = Vector3.Distance(transform.position, item.tran.position);
            if(dist < close)
            {
                close = dist;
                rain = item.terr;
            }
        }

        return rain;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(new Quaternion(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z, 0.0f));
        //Debug.Log(Input.GetAxis("Horizontal"));

        terrainHeightAtOurPosition = turr.SampleHeight(transform.position);

        terrainHeightAtCamPosition = turr.SampleHeight(flighCam.transform.position);

        if (transform.position.y <= terrainHeightAtOurPosition)
            transform.position = new Vector3(transform.position.x,terrainHeightAtOurPosition,transform.position.z);

        if (transform.position.y <= terrainHeightAtOurPosition + .5)
        {
            if (bState == BirdState.landing)
            {
                bState = BirdState.landed;
                resetRotation();
                this.GetComponent<Rigidbody>().useGravity = true;
                speed = 20.0f;
               // animationManager.setBool("land", true);

            }
        }


        //UpdateButtons();



        if (bState != BirdState.landed && bState != BirdState.landing)
        {
            // move forward
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (bState == BirdState.landing)
        {
            transform.position = Vector3.MoveTowards(transform.position, landingPoint, .5f);
        }

            // // check if landed
            // if (terrainHeightAtOurPosition > transform.position.y)
            // {
            //     bState = BirdState.landed;
            //
            //     speed = 10.0f;
            //
            //     this.GetComponent<Rigidbody>().useGravity = true;
            //
            //     SwitchCams();
            // }

            switch (bState)
        {
            case BirdState.flaping:

                UpdateFlightCam();

                transform.position += Vector3.up * Time.deltaTime * 15;
                //birdAnim.Play("MagpieWingsBeat_ANIM");
                animationManager.ChangeState("glide", "flying");

                //Camera.main.transform.position += transform.up;

                if (Input.GetButtonUp("A"))
                {
                    bState = BirdState.glide;
                    //birdAnim.Play("MagpieFly_ANIM");
                    animationManager.ChangeState("flying", "glide");
                }

                GetInputFlight();

                break;
            case BirdState.glide:

                if (Input.GetButtonDown("A"))
                {
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 100 * Time.deltaTime);
                    bState = BirdState.flaping;
                }


                GetInputFlight();

                // if (faceingUp == -1)
                //     bState = BirdState.dive;
                // else if (faceingUp == 1)
                //     bState = BirdState.climb;

                break;

            case BirdState.landing:
               
                    animationManager.setBool("land", true);
                
                break;

            case BirdState.landed:
                animationManager.setBool("land", false);
                GetInputGround();
                break;
        }




    }
    void GetInputFlight()
    {

        UpdateFlightCam();

        transform.RotateAround(head.transform.position, transform.right, (Input.GetAxis("Vertical")));

        if (Input.GetAxis("Horizontal") != 0)
        {



            //transform.Rotate(-Input.GetAxis("Vertical") * 1.5f, Input.GetAxis("Horizontal") * 2.5f,0.0f); // pitch roll 
            transform.RotateAround(head.transform.position, Vector3.up, (Input.GetAxis("Horizontal")));
            transform.RotateAround(head.transform.position, Vector3.up, (Input.GetAxis("RHorizontal")));


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

        //Debug.Log(faceingUp);
        if (faceingUp == 1)
        {
            if (bState != BirdState.flaping)
                speed += -transform.forward.y * Time.deltaTime * 20.0f; /// faceing up
        }
        else if (faceingUp == -1)
            speed -= transform.forward.y * Time.deltaTime * 20.0f;


        if (speed < MIN_SPEED) // min speed
        {
            speed = MIN_SPEED;
        }

        else

        if (speed > MAX_SPEED) // max speed
        {
            speed = MAX_SPEED;
        }

        dif = Vector3.Dot(Vector3.up, transform.forward);
        faceingUp = Mathf.Round(((dif)));// / 1));


        if (Input.GetButtonDown("B") && IsGrounded())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 100 * Time.deltaTime);

            StartCoroutine(TakeOffAndLand(false));
            bState = BirdState.landing;


            SwitchCams(false);
            this.GetComponent<Rigidbody>().useGravity = true;

        }

        if (flighCam.transform.parent == transform && bState != BirdState.landing)
            flighCam.transform.SetParent(null);


        if(bState != BirdState.flap)
        transform.position += Vector3.down * Time.deltaTime * 2.5f; // gravity

        if (transform.position.y - terrainHeightAtOurPosition < 10 && transform.position.y - terrainHeightAtOurPosition > 0)
            Debug.Log( -(((transform.position.y - terrainHeightAtOurPosition) / 10) -1) +1  );

     if(transform.position.y - terrainHeightAtOurPosition < .5f && faceingUp == -1)
     {
         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(transform.forward.x, transform.localEulerAngles.y, transform.forward.z), Vector3.up), -(((transform.position.y - terrainHeightAtOurPosition) / 5) - 1)   * Time.deltaTime + .01f);
         if(IsGrounded())
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 100 * Time.deltaTime);

                StartCoroutine(TakeOffAndLand(false));
                bState = BirdState.landing;


                SwitchCams(false);
                this.GetComponent<Rigidbody>().useGravity = true;
            }
     }

    }
    void GetInputGround()
    {
        flighCam.transform.LookAt(head.transform.position);


        // transform.Rotate(Input.GetAxis("Vertical") * 1.5f, 0.0f, -Input.GetAxis("Horizontal") * 2.5f); // pitch roll 

        Vector3 norm = Vector3.Normalize(head.transform.position - flighCam.transform.position);

        float angle = -Mathf.Asin(norm.y);

        //Debug.Log("t" + terrainHeightAtOurPosition + ",h" + head.transform.position + ",f" + flighCam.transform.position);

    


        if (Input.GetAxis("RVertical") > 0) // rotate up
        {
            if (angle < (Mathf.PI * (1.0f / 4.0f)))
                flighCam.transform.RotateAround(head.transform.position, flighCam.transform.right, Input.GetAxis("RVertical"));
        }

        
        if (terrainHeightAtCamPosition > flighCam.transform.position.y - .25f)
            flighCam.transform.position = new Vector3(flighCam.transform.position.x, terrainHeightAtCamPosition + .25f, flighCam.transform.position.z);

        if (terrainHeightAtCamPosition < flighCam.transform.position.y - .25f)
        {
            if (Input.GetAxis("RVertical") < 0) // rotate down
            {
                if (angle > -(Mathf.PI * (1.0f / 4.0f)))
                    flighCam.transform.RotateAround(head.transform.position, flighCam.transform.right, Input.GetAxis("RVertical"));
            }
        }
        //flighCam.transform.localRotation = new  Quaternion(Mathf.Clamp(flighCam.transform.localRotation.x, 0f,.02f),flighCam.transform.localRotation.y,flighCam.transform.localRotation.z,0);


        //Debug.Log(flighCam.transform.eulerAngles);


        //flighCam.transform.RotateAround(GameObject.FindGameObjectWithTag("head").transform.position, this.transform.right, (Input.GetAxis("RVertical")));



        if (still)
        {
            flighCam.transform.RotateAround(head.transform.position, Vector3.up, Input.GetAxis("RHorizontal"));
        }

        else
        {
            transform.Rotate(0.0f, Input.GetAxis("RHorizontal") * 2.5f, 0.0f);

            transform.position += (transform.forward * Input.GetAxis("Vertical") / 4.5f);
            transform.position += (transform.right * Input.GetAxis("Horizontal") / 4.5f);
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) /// if moving forward
        {

            // Debug.Log("" + flighCam.transform.position + " , " + CamReset.transform.position);



            if (!reset)
            {
                if (transform.rotation != Quaternion.Euler(0, flighCam.transform.localEulerAngles.y, 0)) ;
                StartCoroutine(turnForward());
            }


            //birdAnim.Play("walk");
            animationManager.ChangeState("idle", "walk");


            still = false; /// enable movement



        }
        else
        {

            //birdAnim.Play("MagpieGround_ANIM");
            if (claws.eatingItem == null)
            { 
            animationManager.ChangeState("walk", "idle");
             }

            else
            {
                animationManager.ChangeState("walk", "eat");
            }

            still = true;
            reset = false;



        }

        if (Input.GetButtonDown("A"))
        {
            animationManager.setBool("takeOff", true);
            StartCoroutine(TakeOffAndLand(true));

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 100 * Time.deltaTime);
            this.GetComponent<Rigidbody>().useGravity = false;
            bState = BirdState.flaping;

            transform.position += Vector3.up * Time.deltaTime * 5;

            if (speed != 20)
                speed = 20;

            SwitchCams(true);

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

        if (Input.GetButtonDown("R3"))
        {
            StartCoroutine(turnForward());
        }


        if(claws.eatingItem != null)
        {
            blood.SetActive(true);

           animationManager.ChangeState("idle", "eat");
           animationManager.setBool("walk", false);
           animationManager.setBool("eat", true);


            if (!eating)
            {
                StartCoroutine("Eat");
                eating = true;
            }

        }
        else
        {
            blood.SetActive(false);
            animationManager.setBool("eat", false);
        }


    }



}
