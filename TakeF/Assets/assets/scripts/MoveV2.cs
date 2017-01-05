using UnityEngine;
using System.Collections;

public class MoveV2 : MonoBehaviour {

    Rigidbody rb;
    public Camera cam;

    //movement variables
    //bool canMove = true;
    public float walkSpeed = 1.35f;
    float moveSpeed;
    public float runSpeed = 6f;
    float rotationSpeed = 40f;

    float x;
    float z;
    Vector3 inputVec;
    Quaternion lastForward;
    Quaternion inputQuat;
    Vector3 newVelocity;

    float distToGround;


    bool switched;

    // Use this for initialization
    void Start()
    {

        rb = this.GetComponent<Rigidbody>();

        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((switched));

        if ((Vector3.Distance(transform.position, cam.transform.position) < 9) && Input.GetAxisRaw("Horizontal") == 0)
            switched = true;

        if (switched && Input.GetAxisRaw("Vertical") == 0)
            switched = false;





    }



    void FixedUpdate()
    {


        //Debug.Log(Vector3.Distance(transform.position, cam.transform.position));


        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && IsGrounded())
        {
            rb.velocity += new Vector3(0, 10, 0);

        }
        MovementInput();



        if (inputVec != Vector3.zero)
            moveSpeed = UpdateMovement();
        else
        {
            if (!Input.GetKey(KeyCode.Joystick1Button0) && IsGrounded())
            {
                rb.velocity = Vector3.zero;
            }
            transform.rotation = lastForward;

        }


    }

    void MovementInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);



        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);


        inputVec = Camera.main.transform.TransformDirection(inputVec);


        inputVec = new Vector3(x, 0, z); // forward

        if (inputVec != Vector3.zero)
        {

            if (!switched)
                inputQuat = Quaternion.LookRotation(inputVec, Vector3.up) * Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);

        }

    }

    float UpdateMovement()
    {
        //GetCameraRelativeMovement();
        //Vector3 motion = inputVec;

        Vector3 motion = RotateTowardsMovementDir();



        //reduce input for diagonal movement
        if (motion.magnitude > 1)
        {
            motion.Normalize();
        }

        newVelocity = motion * runSpeed;

        //transform.position = inputVec + Camera.main.transform.forward;  




        if (IsGrounded())
            newVelocity.y = rb.velocity.y;
        else
        {
            //if we are falling use momentum
            newVelocity = rb.velocity;
        }





        rb.velocity = newVelocity;

        //return a movement value for the animator
        return inputVec.magnitude;
    }



    //rotate character towards direction moved
    Vector3 RotateTowardsMovementDir()
    {
        if (inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, inputQuat, Time.deltaTime * rotationSpeed);

            transform.rotation = Quaternion.Euler(0, inputQuat.eulerAngles.y, 0);

            lastForward = transform.rotation;

            //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
        }

        return transform.forward;
    }


    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up + transform.forward * 5, out hit, distToGround + 25))
        {
            if (hit.transform.gameObject.tag == "ground")
            {
                //landingPoint = hit.point;



                return true;
            }
        }

        return false;
    }

    }


