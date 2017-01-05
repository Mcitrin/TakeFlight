using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cuntMenu : MonoBehaviour {
    bool first = false;
    public Text text;
    public Canvas controles;
	// Use this for initialization
	void Start () {
        StartCoroutine(flashText());
    }
	
	// Update is called once per frame
	void Update () {
	
        if(!first && Input.GetButtonDown("start"))
        {
            text.enabled = false;
            controles.enabled = false;
            first = true;
        }

        else if(first && Input.GetButtonDown("start"))
        {
            controles.enabled = !controles.isActiveAndEnabled;
        }


	}

    public IEnumerator flashText()
    {
        Color RGB = text.color;
        for (int n = 0; n < 5; n++)
        {
            text.color = RGB;
            yield return new WaitForSeconds(.5f);
            text.color = new Color(1, 1, 1, .5f);
            yield return new WaitForSeconds(.5f);
        }
        text.color = RGB;


        if (!first)
            StartCoroutine(flashText());
    }
}
