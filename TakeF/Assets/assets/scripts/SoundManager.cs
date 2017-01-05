using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


    public Movement bird;
    public Gameplay PlayManger;
    public AudioClip eat;
    public AudioClip walk;
    public AudioClip flap;
    public AudioSource audio;

    // Use this for initialization
    void Start () {
        
	}

    //Play a sound clip
    public void playSound(AudioClip sound, float vol)
    {
        if(audio.isPlaying != sound)
        audio.PlayOneShot(sound);
        
        Debug.Log("played");
    }//End of playSound(AudioClip sound, float vol)


    // Update is called once per frame
    void Update () {
	

       
        if(bird.bState == Movement.BirdState.flap)
        {
            playSound(flap, 1f);
        }
        else
        {
            if (audio.clip == flap)
                audio.Stop();
        }
        if (bird.bState == Movement.BirdState.landed && !bird.still)
        {
            playSound(walk, 1f);
        }
        else
        {
            if (audio.clip == walk)
                audio.Stop();
        }
        if (bird.bState == Movement.BirdState.landed && bird.eating)
        {
            playSound(eat, 1f);
        }
        else
        {
            if (audio.clip == eat)
                audio.Stop();
        }



    }
}
