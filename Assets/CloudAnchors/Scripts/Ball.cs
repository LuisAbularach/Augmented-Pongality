using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Ball : NetworkBehaviour
{
    public GameObject Player;
    [SyncVar] public bool inPlay;
    [SyncVar] public float movementSpeed;
    public float dirx,diry,dirz;
    [SyncVar] public Vector3 direction;
    bool okay = false;
    float distance;
    public GameObject paddle;

    public float angle;
    public int PreviousLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START BALL!!!");
        inPlay = false;
        direction = Vector3.forward;
//        GameObject.name = "Ball";
        distance = 0.1f;
        paddle = GameObject.Find("paddle");
        Player = GameObject.Find("LocalPlayer");


    }

    // Update is called once per frame
    void Update()
    {
            //Keep ball in position with camera
            //transform.parent =  Camera.main.transform;
            //Debug.Log("inPlay");

        
        
        if (!inPlay) // inPlay == false
        {
            
        }
        else
        {
        transform.position += direction * Time.deltaTime * movementSpeed;
        }

        // }
        // else
        // {
        //     Debug.Log("NOT MOVING");
        //     transform.position = Player.transform.position + transform.forward * 0.1f;
        // }

        //Check for tap
        // if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     //start game
        //     inPlay = true;
        //     Debug.Log("click");

        // }
        // if(inPlay && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     //start game
        //     inPlay = false;
        //     Debug.Log("click");

        // }
    }

    public void StartBallMovement()
    {
        Debug.Log("inPlay: " + inPlay);
        Debug.Log("Start Ball Moving");
        inPlay = true;
        Debug.Log(inPlay);

        // transform.position += direction * Time.deltaTime * movementSpeed;
        
    }

    void OnCollisionEnter(Collision col){
        // if(col.gameObject.name == "Wall" || col.gameObject.name == "AR Default Plane"){
        //     transform.position += Vector3.back * Time.deltaTime * movementSpeed;
        //     // transform.Rotate(0,180,0);
        //     direction = Vector3.back;
        //     //  if(col.gameObject.name == "paddle"&&!okay){
                
        //     //  }

        //     //turn sphere
        //     //transform.rotation = Player.transform.rotation;

        //     if(movementSpeed<0)
        //         movementSpeed -= 0.1f;
        //     else
        //         movementSpeed += 0.1f;
        //     okay = true;
        //     //movementSpeed *= -1;
        // }

        Debug.Log("Collision Name: " + col.gameObject.name);

        if (col.gameObject.name == "paddle")
        {
            Debug.Log("Detected paddle");
            //Debug.Log("Eulger angle x: " + paddle.transform.rotation.eulerAngles.x +
            //" Euler angle z: " + paddle.transform.rotation.eulerAngles.z);
            Debug.Log("Euler angle y: " + paddle.transform.rotation.eulerAngles.y);
            //direction = Vector3.back;
            //transform.Rotate(new Vector3(0, 1, 0), paddle.transform.rotation.eulerAngles.y);
            float xAngle = paddle.transform.rotation.eulerAngles.y;
            float zAngle = paddle.transform.rotation.eulerAngles.y;
            //Check to see if angles are more than 180 and convert to local
            if(xAngle > 180)
            {
                xAngle -= 180.0f;
            }
            if(zAngle < 180)
            {
                zAngle -= 180.0f;
            }
            float x = -1 * Mathf.Cos(DegreeToRadian(xAngle) + Mathf.PI + (Mathf.PI / 2) );
            float z = Mathf.Sin(DegreeToRadian(zAngle) + Mathf.PI + (Mathf.PI / 2));
            // float x = Mathf.Cos(DegreeToRadian(xAngle + 90));
            // float z = Mathf.Sin(DegreeToRadian(zAngle + 90));
            Debug.Log("cos x: " + RadianToDegree(x) + "sin z" + RadianToDegree(z));
            
            // keep angle between quadrants III & IV
            if (paddle.transform.rotation.eulerAngles.y >= 0 && 
            paddle.transform.rotation.eulerAngles.y <= 90){
                angle = paddle.transform.rotation.eulerAngles.y * -1;
            }
            angle = paddle.transform.rotation.eulerAngles.y;
            Debug.Log("paddle : "+ angle);
            direction = new Vector3(x, 0, z);
//            PreviousLocation = (int)PreviouslyCameFrom.Paddle;
        }


    }

    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180.0f;
    }

    private float RadianToDegree(float angle)
    {
        return angle * (180.0f / Mathf.PI);
    }

    // public void SetPosition(GameObject camera)
    // {
    //      transform.parent = camera.transform.position + camera.transform.position.forward * distance;
    // }
}
