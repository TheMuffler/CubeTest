using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vehicle : MonoBehaviour {


    public class Geneset
    {
        public float score = 0;
        public float[] param;
        public float accelweight(int i, int power = 0)
        {
            return param[Vehicle.raycount*power+i];
        }
        public float turnweight(int i,int power = 0)
        {
            return param[Vehicle.raycount * Vehicle.powers + Vehicle.raycount * power + i];
        }
        public float accelConst(){
            return param[Vehicle.raycount*Vehicle.powers*2];
        }
        public float turnConst()
        {
            return param[Vehicle.raycount * Vehicle.powers * 2 + 1];
        }

        public Geneset()
        {
            param = new float[Vehicle.raycount*2*powers+2];
            for (int i = 0; i < param.Length; ++i)
            {
                param[i] = -1f/5f+Random.value*2 / 5f;
            }
        }

        public Geneset(Geneset a, Geneset b)
        {
            param = new float[Vehicle.raycount*2*powers+2];
            HashSet<int> bset = new HashSet<int>();
            while (bset.Count < param.Length/2)
            {
                bset.Add(Random.Range(0, param.Length));
            }
            for (int i = 0; i < param.Length; ++i)
            {
                param[i] = bset.Contains(i) ? b.param[i] : a.param[i];
            }
            for (int i = 0; i < param.Length; ++i)
            {
                if (Random.value <= 0.05f)
                {
                    param[i] = -1f / 5f + Random.value * 2 / 5f;
                }
            }
        }


    }

    GameObject ground;
    CharacterController control;
    Rigidbody rb;
    public Geneset genes = null;
    float score = 0;
    float vel = 0;
    float accel = 0;
    float torque = 0;
    float life = 10;

    public static int powers = 2;
    public static int raycount = 18;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        control = GetComponent<CharacterController>();
        ground = FindObjectOfType<Terrain>().gameObject;
        //genes = new Geneset();
	}

    void Breed(Geneset a, Geneset b)
    {
        genes = new Geneset(a, b);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (genes == null)
            return;

        float[] rays = new float[Vehicle.raycount];
        float angle = Mathf.Atan2(transform.forward.z, transform.forward.x)-Mathf.PI/2f;
        for (int i = 0; i < Vehicle.raycount; ++i)
        {
            
            RaycastHit hit;
            float ang = angle + Mathf.PI / Vehicle.raycount * i;
            Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang)));
            bool result = Physics.Raycast(ray, out hit, 1000, ~(1 << 8));
            rays[i] = result ? hit.distance : 0;
            Debug.DrawLine(ray.origin,ray.origin+ray.direction*hit.distance, Color.red);
            if (!result)
                GameManager.instance.RecordGenes(this);
        }

        accel = genes.accelConst()/5f;
        torque = 0;
        for (int i = 0; i < rays.Length; ++i)
        {
            for (int p = 0; p < Vehicle.powers; ++p)
            {
                torque += Mathf.Pow(rays[i], p + 1) * genes.turnweight(i,p);
                accel += Mathf.Pow(rays[i],p+1) * genes.accelweight(i,p)/5f;
            }
        }

        transform.Rotate(Vector3.up * torque * Time.fixedDeltaTime);
        vel += accel * Time.fixedDeltaTime;
        //vel = Mathf.Clamp(vel, -10f, 10f);


        if(vel > 0)
            genes.score += Mathf.Abs(vel) * Time.fixedDeltaTime;
        
        life -= Time.fixedDeltaTime;
        if (life <= 0f)
            GameManager.instance.RecordGenes(this);

        rb.velocity = transform.forward * vel;

        //transform.Translate(Vector3.forward * vel * Time.fixedDeltaTime);
        //control.SimpleMove(transform.forward * vel);
	}

    public bool recordLock = false;
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject != ground)
        {
            GameManager.instance.RecordGenes(this);
        }
    }

}