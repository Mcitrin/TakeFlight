using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class hungerMeter : MonoBehaviour
{


    float futureTime;
    public Image fill;
    public Image red;
    public Text text;
    public Movement owl;

    bool pulsing = false;

    

    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        if(owl.bState != Movement.BirdState.flaping)
        fill.fillAmount -= .0001f;
        else
        fill.fillAmount -= .0003f;

        if (fill.fillAmount == 0 && !pulsing)
            StartCoroutine("pulse");

        if (fill.fillAmount != 0)
        {
            
            StopCoroutine("pulse");
            red.color = new Color(red.color.r, red.color.g, red.color.b,0);
            text.enabled = false;
            pulsing = false;
        }



    }


    public IEnumerator pulse()
    {
        // Debug.Log(this.gameObject.GetComponentInChildren<Text>().text + " is pulsing");
        pulsing = true;

        text.enabled = true;

        Color32 temp = red.color;
        for (int i = 60; i > 0; i--)
        {
            text.text = "" + i;
            

            for (int n = 255; n > 3; n--)// puls off
            {
                n-=2;
                red.color = new Color32((byte)temp.r, (byte)temp.g, (byte)temp.b, (byte)(n));
                yield return new WaitForEndOfFrame();
            }
            for (int n = 255; n < 97; n++)// puls on
            {
                n+=2;
                red.color = new Color32((byte)temp.r, (byte)temp.g, (byte)temp.b, (byte)(n));
                yield return new WaitForEndOfFrame();
            }
        }

    }
}


