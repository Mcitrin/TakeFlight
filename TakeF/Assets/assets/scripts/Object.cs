using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

    
    float bounds;

    float terrainHeightAtOurPosition;


    // Use this for initialization
    void Start () {

        bounds = this.GetComponent<BoxCollider>().bounds.extents.y ;

    }

   // void OnCollisionEnter(Collision collision)
   // {
   //
   //     if (collision.gameObject.tag == "bird")
   //     {
   //         
   //         
   //     }
   // }

    // Update is called once per frame
    void Update () {

        //transform.position += Vector3.forward/10;
        //transform.Rotate(0,Random.value,0);

        if(transform.position.y > terrainHeightAtOurPosition + bounds)
        transform.position += Vector3.down * Time.deltaTime * 5f; // gravity
        else if(transform.position.y < terrainHeightAtOurPosition + bounds)
        transform.position = new Vector3(transform.position.x,terrainHeightAtOurPosition + bounds, transform.position.z);

        terrainHeightAtOurPosition = Terrain.activeTerrain.SampleHeight(transform.position);

       
            
             
	}
}
