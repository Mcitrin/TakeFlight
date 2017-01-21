using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentSpawner : MonoBehaviour {

    public GameObject agent;
    public List<GameObject> agents;
    public Node start;
    public int maxAgents;
    public Terrain Turr;

    public List<Material> mats;

    bool spawning;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < maxAgents; i++)
        {
            Spawn();
        }

       
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i] == null)
                agents.Remove(agents[i]);
        }

        if (!spawning && agents.Count < maxAgents)
            StartCoroutine(SpawnMore());

	}

    IEnumerator SpawnMore()
    {
        spawning = true;
        
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (agents.Count < maxAgents)
            Spawn();

        spawning = false;
    }

    void Spawn()
    {
        agents.Insert(0,(GameObject)Instantiate(agent, transform.position, transform.rotation));
        agents[0].GetComponent<Agent>().targetNode = start;
        //agents[0].GetComponent<Agent>().Train = Turr;

        agents[0].GetComponent<Agent>().body.GetComponent<SkinnedMeshRenderer>().material = mats[Random.Range(0, mats.Count)];
    }


}
