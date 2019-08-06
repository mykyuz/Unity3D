using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform player1;
    public Transform player2;
    Vector3 offset;
    Vector3 offOne;
    Camera camera;
    public float aspect;
    void Start () {
        check();
        
    }

    void check()
    {
        if (player1 != null && player2 != null)
        {
            offset = transform.position - (player1.position + player2.position) / 2;
            camera = GetComponent<Camera>();
            float distance = Vector3.Distance(player1.position, player2.position);
            aspect = camera.orthographicSize / distance;
        }
        if (player1 == null || player2 == null)
        {
            if (player1 != null)
            {
                offOne = transform.position - player1.position;
            }
            if (player2 != null)
            {
                offOne = transform.position - player2.position;
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player1!= null && player2 != null)
        {
            FloowTwoTarget();
        }
        if (player1 == null || player2 == null)
        {
            if (player1 != null)
            {
                offOne = transform.position - player1.position;
                FloowOneTarget(player1);
            } else if (player2 != null)
            {
                offOne = transform.position - player2.position;
                FloowOneTarget(player2);
            }
        }     
       
    }
    void FloowTwoTarget()
    {
        transform.position = (player1.position + player2.position) / 2 + offset;
        float distance = Vector3.Distance(player1.position, player2.position);
        camera.orthographicSize = distance * aspect;
        //camera.orthographicSize = distance * 0.44f;
        print(distance);

    }

    void FloowOneTarget(Transform player)
    {

        transform.position = Vector3.Lerp(transform.position, player.position + offOne, 5.0f * Time.deltaTime);
    }
}
