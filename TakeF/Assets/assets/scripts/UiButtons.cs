using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UiButtons : MonoBehaviour {


    public Image grab;
    public Image eat;
    public Image land;
    public Image flap;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnOff(bool on, Image image)
    {
        if(on)
        {
            image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            image.color = new Color32(125, 125, 125, 125);
        }



    }
}
