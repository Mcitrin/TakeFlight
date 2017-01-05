using UnityEngine;
using System.Collections;

public class moon : MonoBehaviour {


    public Material stars;
    bool change;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y < 50 && stars.color.a != 0 && !change)
            stars.color = new Color(stars.color.r, stars.color.r, stars.color.r, 1);
        // StartCoroutine(onOff(false));

        if (transform.position.y > 50 && stars.color.a != 1 && !change)
            stars.color = new Color(stars.color.r, stars.color.r, stars.color.r, 0);
        //StartCoroutine(onOff(true));


        transform.RotateAround(Vector3.zero, Vector3.right, 5 * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }



    public IEnumerator onOff(bool on)
    {
        
        change = true;



        if (!on)
        {
            for (float i = 1; i > 0; i -= .01f)
            {

                stars.color = new Color(stars.color.r, stars.color.r, stars.color.r, i);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            for (float i = 0; i < 1; i += .01f)
            {
                stars.color = new Color(stars.color.r, stars.color.r, stars.color.r, i);
                yield return new WaitForEndOfFrame();
            }
        }

        change = false;
    }

    

}
