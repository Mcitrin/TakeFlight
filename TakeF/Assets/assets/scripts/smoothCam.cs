using UnityEngine;
using System.Collections;

public class smoothCam : MonoBehaviour {

    GameObject cameraTarget;
    public float smoothTime = 0.1f;
    Vector2 velocity;

    Vector3 tagert;

    float dist;

    void Start()
    {
        cameraTarget = GameObject.FindGameObjectWithTag("bird");

        //transform.LookAt(cameraTarget.transform.position);

        transform.position = (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f;


        dist = Vector3.Distance(transform.position, (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f);

        Debug.Log(dist + "," + Vector3.Distance(transform.position, (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f));

    }

    void Update()
    {

        transform.LookAt(cameraTarget.transform.position);
        //Debug.Log(dist + "," + Vector3.Distance(transform.position, (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f));

        transform.position = (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f;


        //if (Vector3.Distance(transform.position, (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f) > )
        //transform.position = Vector3.MoveTowards(transform.position, (cameraTarget.transform.position - transform.forward * 15) + Vector3.up * 10.0f, .075f);

        //if (Vector3.Distance(transform.position, cameraTarget.transform.position) < dist)
        //transform.position = Vector3.Lerp(transform.position, (transform.position + cameraTarget.transform.forward * dist) + Vector3.up * 5.0f, .01f);




        //thisTransform.position = new Vector3(Mathf.SmoothDamp(thisTransform.position.x, cameraTarget.transform.position.x, ref velocity.x, smoothTime),
        //    Mathf.SmoothDamp(thisTransform.position.y, cameraTarget.transform.position.y - offset + 2, ref velocity.y, smoothTime),
        //    (cameraTarget.transform.position.z + offset)) - cameraTarget.transform.forward * .5f;

        //if (Vector3.Distance(transform.position, cameraTarget.transform.position) > 15)
        //{
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraTarget.transform.forward, Vector3.up), Time.deltaTime * .01f);
        //transform.position = Vector3.Lerp(transform.position, (cameraTarget.transform.position - transform.forward * 15f) + Vector3.up * 5, .01f);
        //}

        //transform.position = cameraTarget.transform.position - OrgPosition;


        //transform.position = Vector3.Lerp(transform.position, (cameraTarget.transform.position - OrgPosition), .01f);




        //transform.position = cameraTarget.transform.position - cameraTarget.transform.forward*10 +
        //    this.transform.up*3;

        ////transform.transform.parent = this.transform;


    }
}

