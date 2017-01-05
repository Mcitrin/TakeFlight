using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : MonoBehaviour {

    public List<GameObject> objects;
    public GameObject bird;

    public enum GameState {start, play}
    public GameState state = GameState.play;

    public GameObject rain;

    // Use this for initialization
    void Start () {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(Random.Range(60, 180));

        rain.gameObject.SetActive(true);

        yield return new WaitForSeconds(Random.Range(30, 120));

        rain.gameObject.SetActive(false);

        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update () {
	
        switch(state)
        {
            case GameState.start:
                break;
            case GameState.play:
                if(bird == null)
                {
                    bird = GameObject.FindGameObjectWithTag("bird");
                }

                if (objects.Count == 0)
                {
                     for(int i = 0; i < GameObject.FindGameObjectsWithTag("pickUp").Length; ++i)
                    {
                        objects.Add(GameObject.FindGameObjectsWithTag("pickUp")[i]);

                    }
                }

                break;
                
        }

	}


}
