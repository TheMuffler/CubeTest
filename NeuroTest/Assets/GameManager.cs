using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    List<Vehicle.Geneset> genome = new List<Vehicle.Geneset>();

    List<Vehicle.Geneset> nextGeneration = new List<Vehicle.Geneset>();

    float totalscore;

    float nextGenScore;
    public GameObject vehiclePrefab;

    public int generation = 0;

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {
        GenerateNewVehicle();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    Vehicle.Geneset GetRandGenes()
    {
        float rand = Random.value * totalscore;
        int index=0;
        while (genome[index].score < rand)
        {
            rand -= genome[index].score;
            ++index;
        }
        return genome[index];
    }

    void GenerateNewVehicle()
    {
         Vehicle v = ((GameObject)Instantiate(vehiclePrefab, Vector3.up * 0.5f, Quaternion.identity)).GetComponent<Vehicle>();
         if (genome.Count < 10)
         {
             v.genes = new Vehicle.Geneset();
         }
         else
         {
             v.genes = new Vehicle.Geneset(GetRandGenes(), GetRandGenes());
         }
    }

    public void RecordGenes(Vehicle v)
    {
        if (v.recordLock)
            return;
        Vehicle.Geneset genes = v.genes;
        v.recordLock = true;
        v.genes = null;
        nextGeneration.Add(genes);


        genes.score = Mathf.Pow(genes.score, 2);

        nextGenScore += genes.score;
        Destroy(v.gameObject);

        if (nextGeneration.Count >= 20)
        {
            genome = nextGeneration;
            nextGeneration = new List<Vehicle.Geneset>();
            totalscore = nextGenScore;
            nextGenScore = 0;
            ++generation;
        }

        GenerateNewVehicle();
    }


}
