using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {


    public WorldCube worldCube;

    private static WorldManager _instance = null;
    public static WorldManager instance
    {
        get
        {
            return instance;
        }
    }


    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
