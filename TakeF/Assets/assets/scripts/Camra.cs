using UnityEngine;
using System.Collections;


public class Camra : MonoBehaviour {

    //public fly bird;
    float dist;

	// Use this for initialization
	void Start () {

        //dist = Vector3.Distance(this.gameObject.transform.position, bird.head.transform.position);
	
	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawRay(transform.position, new Vector3(0,-1,0) * 10, Color.red);


        // if (Vector3.Distance(this.gameObject.transform.position, bird.head.transform.position) > dist)
        //     Debug.Log("" + Vector3.Distance(this.gameObject.transform.position, bird.head.transform.position) + " , " + dist);

       // if (bird.bState == fly.BirdState.landed)
     {
            if (Input.GetKey("joystick 1 button 5"))
                StartCoroutine(move(true));
            

            if (Input.GetKey("joystick 1 button 4"))
                StartCoroutine(move(false));

        }

    }

   IEnumerator move(bool forward)
   {
      //if(bird.bState == fly.BirdState.landed)
        {
            yield return new WaitForSeconds(.005f);

            if (forward && Input.GetKey("joystick 1 button 5"))
            {
                //this.transform.position = Vector3.Lerp(this.transform.position, bird.transform.position, .1f);
                StartCoroutine(move(true));
            }
            else
            if (!forward && Input.GetKey("joystick 1 button 4"))
            {
                //this.transform.position = Vector3.Lerp(this.transform.position, -bird.transform.position, .1f);
                StartCoroutine(move(false));
            }
        }
   }
}  
