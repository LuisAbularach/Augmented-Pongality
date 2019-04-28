using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Ball : NetworkBehaviour
{
    enum PreviouslyCameFrom {Paddle, OrangeWall, PurpleWall, EnemyPaddle};
    public GameObject Player;
    private Rigidbody rb;
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

        
        rb = gameObject.GetComponent<Rigidbody>(); 
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
        if(paddle!=null){
            //Debug.Log(paddle.transform.rotation.y);
        }
    }

    public void StartBallMovement()
    {
        Debug.Log("inPlay: " + inPlay);
        Debug.Log("Start Ball Moving");
        inPlay = true;
        Debug.Log(inPlay);

        // transform.position += direction * Time.deltaTime * movementSpeed;
        
    }
#pragma warning disable 618
        [Command]
#pragma warning restore 618
    private void CmdSetProperties(float speed, Vector3 BounceDirection)
    {
        movementSpeed = speed;
        direction = BounceDirection;
    }

    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180.0f;
    }

    private float RadianToDegree(float angle)
    {
        return angle * (180.0f / Mathf.PI);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "paddle" && !okay)
        {
            Debug.Log("Detected paddle");
            //Debug.Log("Eulger angle x: " + paddle.transform.rotation.eulerAngles.x +
            //" Euler angle z: " + paddle.transform.rotation.eulerAngles.z);
            Debug.Log("Euler angle y: " + paddle.transform.rotation.eulerAngles.y);
            //direction = Vector3.back;
            //transform.Rotate(new Vector3(0, 1, 0), paddle.transform.rotation.eulerAngles.y);
            float xAngle = paddle.transform.rotation.eulerAngles.y;
            float zAngle = paddle.transform.rotation.eulerAngles.y;
            float x = -1 * Mathf.Cos(DegreeToRadian(xAngle) + Mathf.PI + (Mathf.PI / 2) );
            float z = Mathf.Sin(DegreeToRadian(zAngle) + Mathf.PI + (Mathf.PI / 2));
            // float x = Mathf.Cos(DegreeToRadian(xAngle + 90));
            // float z = Mathf.Sin(DegreeToRadian(zAngle + 90));
            Debug.Log("cos x: " + RadianToDegree(x) + "sin z" + RadianToDegree(z));
            
            // keep angle between quadrants III & IV
            if ((paddle.transform.rotation.eulerAngles.y >= 0 && paddle.transform.rotation.eulerAngles.y <= 90)||
            (paddle.transform.rotation.eulerAngles.y > 180 && paddle.transform.rotation.eulerAngles.y < 270)){
                Debug.Log("0 < rotation < 90");
                angle = paddle.transform.rotation.eulerAngles.y * -1;

                if(paddle.transform.rotation.eulerAngles.y > 180 && paddle.transform.rotation.eulerAngles.y < 270)
                {
                    Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }
            // else
            // {
            //      angle = paddle.transform.rotation.eulerAngles.y
            // }
            else{
            Debug.Log("-90 < rotation < 0");    
            angle = paddle.transform.rotation.eulerAngles.y;
                if(paddle.transform.rotation.eulerAngles.y > 90 && paddle.transform.rotation.eulerAngles.y < 180){
                     Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }
            Debug.Log("paddle : "+ angle);
            CmdSetProperties(movementSpeed, new Vector3(x, 0, z));
            PreviousLocation = (int)PreviouslyCameFrom.Paddle;
        }
        if (col.gameObject.name == "Purple Wall")
        {
            float newAngle = (90 - angle) * -1;
            float x = Mathf.Cos(DegreeToRadian(newAngle));
            float z = Mathf.Sin(DegreeToRadian(newAngle));
            Debug.Log("Purple Wall cos x: " + RadianToDegree(x) + " sin z: " + RadianToDegree(z));
            angle = newAngle;
            direction = new Vector3(x, 0, z);
        }
        if (col.gameObject.name == "LeftWall")
        {
            float newAngle = (90 - angle) * -1;
            float x = Mathf.Cos(DegreeToRadian(newAngle));
            float z = Mathf.Sin(DegreeToRadian(newAngle));
            Debug.Log("Purple Wall cos x: " + RadianToDegree(x) + " sin z: " + RadianToDegree(z));
            angle = newAngle;
            direction = new Vector3(x, 0, z);
        }
        if (col.gameObject.name == "RightWall")
        {
            float newAngle = 360 - angle;
            float newAngle1 = 90 - newAngle;
            float nA = 180 - (newAngle1 * 2);
            float nA1 = newAngle1 + 90 + nA;
            float x = Mathf.Cos(DegreeToRadian(nA1));
            float z = Mathf.Sin(DegreeToRadian(nA1));
            angle = nA1;
            direction = new Vector3(x, 0, z);
            Debug.Log("ORANGE Wall: cos x: " + RadianToDegree(x) + " sin z: " + RadianToDegree(z));
        }
    }
}