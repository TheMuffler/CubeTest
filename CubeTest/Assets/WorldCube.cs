using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCube : MonoBehaviour {

    private static WorldCube _instance;
    public static WorldCube instance
    {
        get
        {
            return _instance;
        }
    }


    [HideInInspector]
    public TilePatchFace[] faces = new TilePatchFace[6];

    public int width = 5, height = 5, depth = 5;

    [HideInInspector]
    public float leftdist, rightdist, topdist, bottomdist, frontdist, backdist;
    [HideInInspector]
    public float lrOffset, tbOffset, fbOffset;

    GameObject facePrefab;

    AudioSource sound;
    public AudioClip shatterClip;

    TilePatchFace left
    {
        get
        {
            return faces[0];
        }
        set
        {
            faces[0] = value;
        }
    }
    TilePatchFace right
    {
        get
        {
            return faces[1];
        }
        set
        {
            faces[1] = value;
        }
    }
    TilePatchFace top
    {
        get
        {
            return faces[2];
        }
        set
        {
            faces[2] = value;
        }
    }
    TilePatchFace bottom
    {
        get
        {
            return faces[3];
        }
        set
        {
            faces[3] = value;
        }
    }
    TilePatchFace front
    {
        get
        {
            return faces[4];
        }
        set
        {
            faces[4] = value;
        }
    }
    TilePatchFace back
    {
        get
        {
            return faces[5];
        }
        set
        {
            faces[5] = value;
        }
    }


    public Vector3 center
    {
        get {
            return transform.position + transform.right * lrOffset + transform.up * tbOffset + transform.forward * fbOffset;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start() {

        sound = GetComponent<AudioSource>();

        leftdist = width / 2f;
        rightdist = width / 2f;
        topdist = height / 2f;
        bottomdist = height / 2f;
        frontdist = depth / 2f;
        backdist = depth / 2f;


        facePrefab = Resources.Load<GameObject>("CubeFace");
        left = newFace(depth, height, Vector3.left, width/2f);
        right = newFace(depth, height, Vector3.right, width / 2f);
        top = newFace(width, depth,Vector3.up, height / 2f);
        bottom = newFace(width, depth, Vector3.down, height / 2f);
        front = newFace(width, height,Vector3.forward, depth / 2f);
        back = newFace(width, height,Vector3.back, depth / 2f);

        right.name = "right";
        left.name = "left";
        top.name = "top";
        bottom.name = "bottom";
        front.name = "front";
        back.name = "back";

        //left.left = back;
        //left.right = front;
        //left.top = top;
        //left.bottom = bottom;

        //right.left = front;
        //right.right = back;
        //right.top = top;
        //right.bottom = bottom;

        //top.left = left;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
            ShaveRight();
        if (Input.GetKeyDown(KeyCode.X))
            ShaveTop();
        if (Input.GetKeyDown(KeyCode.C))
            ShaveLeft();
        if (Input.GetKeyDown(KeyCode.V))
            ShaveBottom();
        if (Input.GetKeyDown(KeyCode.B))
            ShaveFront();
        if (Input.GetKeyDown(KeyCode.N))
            ShaveBack();
    }

    TilePatchFace newFace(int w, int h, Vector3 normal, float dist)
    {
        TilePatchFace face = ((GameObject)Instantiate(facePrefab)).GetComponent<TilePatchFace>();
        face.Generate(w, h);
        face.transform.parent = transform;

        face.transform.localRotation = Quaternion.LookRotation(normal);
        face.transform.Rotate(90, 0, 0);
        face.normal = normal;
        face.transform.localPosition = normal * dist;

        return face;
    }

    void ShaveRight()
    {
        if (width <= 1)
            return;

        sound.PlayOneShot(shatterClip);

        right.removeAll();
        Destroy(right.gameObject);
        rightdist -= 1f;
        lrOffset -= 0.5f;
        width -= 1;
        right = newFace(depth, height, Vector3.right, rightdist);

        right.transform.localPosition += Vector3.up * tbOffset + Vector3.forward * fbOffset;

        top.SliceRight();
        bottom.SliceRight();
        front.SliceRight();
        back.SliceLeft();
    }

    void ShaveTop()
    {
        if (height <= 1)
            return;
        sound.PlayOneShot(shatterClip);

        top.removeAll();
        Destroy(top.gameObject);
        topdist -= 1f;
        tbOffset -= 0.5f;
        height -= 1;
        top = newFace(width, depth, Vector3.up, topdist);

        top.transform.localPosition += Vector3.right * lrOffset+Vector3.forward*fbOffset;

        right.SliceBottom();
        left.SliceBottom();
        front.SliceBottom();
        back.SliceBottom();
    }

    void ShaveLeft()
    {
        if (width <= 1)
            return;
        sound.PlayOneShot(shatterClip);
        left.removeAll();
        Destroy(left.gameObject);
        leftdist -= 1f;
        lrOffset += 0.5f;
        width -= 1;
        left = newFace(depth, height, Vector3.left, leftdist);

        left.transform.localPosition += Vector3.up * tbOffset + Vector3.forward * fbOffset;

        top.SliceLeft();
        bottom.SliceLeft();
        front.SliceLeft();
        back.SliceRight();
    }

    void ShaveBottom()
    {
        if (height <= 1)
            return;
        sound.PlayOneShot(shatterClip);

        bottom.removeAll();
        Destroy(bottom.gameObject);
        bottomdist -= 1f;
        tbOffset += 0.5f;
        height -= 1;
        bottom = newFace(width, depth, Vector3.down, bottomdist);
        
        bottom.transform.localPosition += Vector3.right * lrOffset + Vector3.forward * fbOffset;

        right.SliceTop();
        left.SliceTop();
        front.SliceTop();
        back.SliceTop();
    }

    void ShaveFront()
    {
        if (depth <= 1)
            return;
        sound.PlayOneShot(shatterClip);
        front.removeAll();
        Destroy(front.gameObject);
        frontdist -= 1f;
        fbOffset -= 0.5f;
        depth -= 1;
        front = newFace(width, height, Vector3.forward, frontdist);

        front.transform.localPosition += Vector3.right * lrOffset + Vector3.up * tbOffset;

        right.SliceLeft();
        left.SliceRight();
        bottom.SliceBottom();
        top.SliceTop();
    }

    void ShaveBack()
    {
        if (depth <= 1)
            return;
        sound.PlayOneShot(shatterClip);
        back.removeAll();
        Destroy(back.gameObject);
        backdist -= 1f;
        fbOffset += 0.5f;
        depth -= 1;
        back = newFace(width, height, Vector3.back, backdist);

        back.transform.localPosition += Vector3.right * lrOffset + Vector3.up * tbOffset;

        right.SliceRight();
        left.SliceLeft();
        bottom.SliceTop();
        top.SliceBottom();
    }
}
