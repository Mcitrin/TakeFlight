using UnityEngine;
using System.Collections;



public class rain : MonoBehaviour {


    public Transform player;

	// Use this for initialization
	void Start () {
        transform.position = player.position + new Vector3(0, 50, 0);
    }
	
	// Update is called once per frame
	void Update () {


        transform.position = player.position + new Vector3(0, 50, 0);

    }
}
