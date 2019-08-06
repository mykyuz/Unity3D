using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public int TankNumber = 1;
    public float Movespeed = 12f;
    public float rotationSpeed = 180f;
    public AudioSource moveAudio;
    public AudioClip EneginIdle;
    public AudioClip EneginMove;
    public float AudioPitch = 0.2f;

    private string HorizontalRaw;
    private string VerticalRaw;
    private float h;
    private float v;
    private Rigidbody rbody;
    private float CurrentPitch;
   

    void Enable()
    {

    }
	void Start () {
        rbody = GetComponent<Rigidbody>();
        moveAudio = GetComponent<AudioSource>();
        HorizontalRaw = "Horizontal" + TankNumber;
        VerticalRaw = "Vertical" + TankNumber;
        CurrentPitch = moveAudio.pitch;
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis(HorizontalRaw);
        v = Input.GetAxis(VerticalRaw);
        EneginAudio(h,v);
	    
	}
    void EneginAudio(float h, float v)
    {
        if ( h!=0f || v!=0f ) // run
        {
            if (moveAudio.clip == EneginIdle)
            {
                moveAudio.clip = EneginMove;
                moveAudio.pitch = Random.Range(CurrentPitch - AudioPitch, CurrentPitch + AudioPitch);
                moveAudio.Play();
            }
            
        }
        else  //not run
        {
            if (moveAudio.clip == EneginMove)
            {
                moveAudio.clip = EneginIdle;
                moveAudio.pitch = Random.Range(CurrentPitch - AudioPitch, CurrentPitch + AudioPitch);
                moveAudio.Play();
            }
        }
    }
    void FixedUpdate()
    {
        Move();
        Trun();
    }
    void Move()
    {
        Vector3 movement = v * Movespeed * transform.forward * Time.deltaTime;
        rbody.MovePosition(transform.position + movement);
    }

    void Trun()
    {
        Vector3 ro = new Vector3(0f, rotationSpeed * h * Time.deltaTime, 0f);
        Quaternion newRo = Quaternion.Euler(ro);
        rbody.MoveRotation(transform.rotation * newRo);
    }
}
