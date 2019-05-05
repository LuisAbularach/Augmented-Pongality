using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace GoogleARCore.Examples.CloudAnchors{
public class Ball : NetworkBehaviour
{
     enum PreviouslyCameFrom {Paddle, LeftWall, RightWall, EnemyPaddle};
    public GameObject Player;
    private Rigidbody rb;
    [SyncVar] public bool inPlay;
    [SyncVar] public float movementSpeed;
    public float dirx,diry,dirz;
    [SyncVar] public Vector3 direction;

    [SyncVar] public float P2Back;
    public float P1back;
    bool okay = false;
    float distance;
    public GameObject paddle;

    public bool isP2;

    public float angle;
    public int PreviousLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        P1back = -.5f;
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
        //Score
        if (transform.position.z >=  P2Back)
        {
            // transform.position = new Vector3(0f, 3.66f, 0f);
            Score.ChangeScore(1);
            // direction = new Vector3(0f, 0f, 1f);
            angle = 90;

            movementSpeed = Mathf.Abs(.5f);
            //inPlay = false;
            transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);
            float randAngle = UnityEngine.Random.Range(10f, 170f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            if(!isP2)
                direction = new Vector3(xComp, 0, zComp);
        }
         if (transform.position.z <= P1back) // Enemy Won
        {
            Score.ChangeScore(2);
            movementSpeed = Mathf.Abs(.2f);
            //inPlay = false;
            transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);
            float randAngle = UnityEngine.Random.Range(10f, 170f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            if (zComp >= 0)
                zComp *= -1;
            if(!isP2)
                direction = new Vector3(xComp, 0, zComp);
            
                
        }
        
        if (!inPlay) // inPlay == false
        {
            
        }
        else
        {
        transform.position += direction * Time.deltaTime * movementSpeed;
        }


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
          if (col.gameObject.name == "paddle")
        {
            Debug.Log("Detected paddle");
            
            Debug.Log("Euler angle y: " + paddle.transform.rotation.eulerAngles.y);
            float paddleYAngle = paddle.transform.rotation.eulerAngles.y;
            float x, z;
            if (direction.z < 0)
            {
                x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2) );
                z = Mathf.Sin(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2));
            }
            else
            {
                x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2) );
                z = Mathf.Sin(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2));
            }
            
            
            Debug.Log("cos x: " + RadianToDegree(x) + "sin z" + RadianToDegree(z));
            if (movementSpeed < 31)
                movementSpeed += 0.5f;//(movementSpeed + 1);
            // keep angle between quadrants III & IV
            if ((paddleYAngle >= 0 && paddleYAngle <= 90) ||
                (paddleYAngle > 180 && paddleYAngle < 270)){
                Debug.Log("0 < rotation < 90");
                angle = paddle.transform.rotation.eulerAngles.y * -1;

                if(paddle.transform.rotation.eulerAngles.y > 180 && paddle.transform.rotation.eulerAngles.y < 270)
                {
                    Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }
            
            else{
            Debug.Log("-90 < rotation < 0");    
            angle = paddle.transform.rotation.eulerAngles.y;
                if(paddle.transform.rotation.eulerAngles.y > 90 && paddle.transform.rotation.eulerAngles.y < 180){
                     Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }
            Debug.Log("paddle : "+ angle);
            Player.GetComponent<LocalPlayerController>().CmdSetProperties(new Vector3(x, 0, z),movementSpeed);
            PreviousLocation = (int)PreviouslyCameFrom.Paddle;
        }
      
        if (col.gameObject.name == "LeftWall" && !isP2)
        {
            Vector3 vel = direction;
            
            Player.GetComponent<LocalPlayerController>().CmdSetProperties( new Vector3(vel.x * -1, 0, vel.z),movementSpeed);
            
            PreviousLocation = (int)PreviouslyCameFrom.LeftWall;
        }

        if (col.gameObject.name == "RightWall" && !isP2)
        {
            Vector3 vel = direction;
            
            Player.GetComponent<LocalPlayerController>().CmdSetProperties(new Vector3(vel.x * -1, 0, vel.z),movementSpeed);

            PreviousLocation = (int)PreviouslyCameFrom.RightWall;

        }

    }

#pragma warning disable 618
        [Command]
#pragma warning restore 618
    public void CmdsetP2Back(float back)
    {
        P2Back = back;
    }
}
}