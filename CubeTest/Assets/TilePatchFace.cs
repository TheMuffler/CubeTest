using UnityEngine;
using System.Collections;

public class TilePatchFace : MonoBehaviour {

    public int tw, th;
    [HideInInspector]
    public Vector3 normal;
    [HideInInspector]
    public TilePatchFace left, right, top, bottom;

    GameObject tilePrefab;
    public Material[] randomMats;


    GameObject[,] tiles = new GameObject[0,0];
    

	// Use this for initialization
	void Start () {
        //tilePrefab = (GameObject)Resources.Load("Tile");
        //Generate(5, 5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Generate(int tw, int th)
    {
        this.tw = tw;
        this.th = th;

        foreach (GameObject tile in tiles)
            Destroy(tile);

        tiles = new GameObject[tw, th];

        for (int i = 0; i < tw; ++i)
        {
            for (int j = 0; j < th; ++j)
            {
                GameObject tile = NewTile();
                //if (i == 0)
                //    tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = randomMats[0];
                tile.transform.parent = transform;
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.localPosition = new Vector3(-tw / 2f + i + 0.5f, 0, -th / 2f + j + 0.5f);
                tiles[i, j] = tile;
            }
        }
    }

    public void SliceRight()
    {
        tw -= 1;
        if (tw <= 0)
            return;
        for (int j = 0; j < th; ++j)
        {
            remove(tiles[tw, j]);
        }
        GameObject[,] oldtiles = tiles;
        tiles = new GameObject[tw, th];
        for(int i = 0; i < tw; ++i)
        {
            for(int j = 0; j < th; ++j)
            {
                tiles[i, j] = oldtiles[i, j];
            }
        }
    }

    public void SliceLeft()
    {
        tw -= 1;
        if (tw <= 0)
            return;
        for (int j = 0; j < th; ++j)
        {
            remove(tiles[0, j]);
        }
        GameObject[,] oldtiles = tiles;
        tiles = new GameObject[tw, th];
        for (int i = 0; i < tw; ++i)
        {
            for (int j = 0; j < th; ++j)
            {
                tiles[i, j] = oldtiles[i+1, j];
            }
        }
    }

    public void SliceTop()
    {
        th -= 1;
        if (th <= 0)
            return;
        for (int i = 0; i < tw; ++i)
        {
            //Destroy(tiles[i,th]);
            //Rigidbody rb = tiles[i, th].GetComponent<Rigidbody>();
            //rb.isKinematic = false;
            remove(tiles[i, th]);
        }
        GameObject[,] oldtiles = tiles;
        tiles = new GameObject[tw, th];
        for (int i = 0; i < tw; ++i)
        {
            for (int j = 0; j < th; ++j)
            {
                tiles[i, j] = oldtiles[i, j];
            }
        }
    }

    public void SliceBottom()
    {
        th -= 1;
        if (th <= 0)
            return;
        for(int i = 0; i < tw; ++i)
        {
            remove(tiles[i, 0]);
        }
        GameObject[,] oldtiles = tiles;
        tiles = new GameObject[tw, th];
        for(int i = 0; i < tw; ++i)
        {
            for(int j = 0; j < th; ++j)
            {
                tiles[i, j] = oldtiles[i, j + 1];
            }
        }
    }


    GameObject NewTile()
    {
        if(!tilePrefab)
            tilePrefab = (GameObject)Resources.Load("Tile");
        GameObject tile = (GameObject)Instantiate(tilePrefab);
        tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = randomMats[Random.Range(0, randomMats.Length)];
        return tile;
    }

    void remove(GameObject t)
    {
        t.transform.parent = null;
        Rigidbody rb = t.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddExplosionForce(400, WorldCube.instance.center, 100);
    }

    public void removeAll()
    {
        foreach (GameObject t in tiles)
            remove(t);
    }
}
