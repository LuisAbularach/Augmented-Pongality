// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BallOld : MonoBehaviour
// {
// public GameObject Player;
// bool inPlay;
// // Start is called before the first frame update
// public Vector3 direction;
// bool okay = false;
// public GameObject sign;
// public GameObject paddle;
// public float speed = 8f;
// public GameObject enemyWall;

// public float angle;
// public int PreviousLocation;
// public int Quad;
// private Rigidbody rb;
// void Start()
// {
// inPlay = false;
// direction = Vector3.forward;
// rb = gameObject.GetComponent<Rigidbody>();
// }

// // Update is called once per frame
// void Update()
// {
// if (transform.position.z >= 30)// GAME OVER
// {
// transform.position = new Vector3(0f, 3.66f, 30f);
// sign.transform.position = new Vector3(15, 10, -5);
// angle = 90;
// }


// if (inPlay)
// {
// // transform.position += direction * Time.deltaTime * movementSpeed;
// }


// if (Input.GetKeyDown(KeyCode.Space))
// {
// //start game
// inPlay = true;
// Debug.Log("click");
// rb.velocity = new Vector3(0f, 0f, speed);

// }
// Debug.Log("VELOCITY: " + gameObject.GetComponent<Rigidbody>().velocity);
// }

// private float DegreeToRadian(float angle)
// {
// return Mathf.PI * angle / 180.0f;
// }

// private float RadianToDegree(float angle)
// {
// return angle * (180.0f / Mathf.PI);
// }

// void OnCollisionEnter(Collision col)
// {


// if ( (col.gameObject.name == "paddle" && !okay ) ||
// (col.gameObject.name == "Opponent NEW wall" && !okay) )
// {
// float x = 0;
// float z = 0;
//     if (col.gameObject.name == "paddle")
//     {
//     Debug.Log("Detected paddle");
//     Debug.Log("Euler angle y: " + paddle.transform.rotation.eulerAngles.y);
//     float PaddleAngle = paddle.transform.rotation.eulerAngles.y;
//     x = -1 * Mathf.Cos(DegreeToRadian(PaddleAngle) + Mathf.PI + Mathf.PI/2);
//     z = Mathf.Sin(DegreeToRadian(PaddleAngle) + Mathf.PI + Mathf.PI/2);
//     Debug.Log("Paddle: Cos x: " + x + " sin z: " + z);
//     }
//     if (col.gameObject.name == "Opponent NEW wall")
//     {
//     Debug.Log("Detected ENEMY player 2 paddle");

//     float enemyPaddleAngle = enemyWall.transform.rotation.eulerAngles.y;
//     x = -1 * Mathf.Cos(DegreeToRadian(enemyPaddleAngle) + Mathf.PI/2);
//     z = Mathf.Sin(DegreeToRadian(enemyPaddleAngle) + Mathf.PI/2);
// }


// float xComp = Mathf.Abs(x);
// float zComp = Mathf.Abs(z);
// float maxi = Mathf.Max(xComp, zComp);
// if (maxi == xComp)
// {
// float xPartMax = speed / xComp;
// x *= xPartMax;
// z *= xPartMax;
// }
// else
// {
// float zPartMax = speed / zComp;
// x *= zPartMax;
// z *= zPartMax;
// }
// // Now that we have ratio, we have to increase its proportions
// // Get maximum of x and z
// rb.velocity = new Vector3(x, 0f, z);



// }

// if (col.gameObject.name == "Purple Wall")
// {
// Debug.Log("Purple Wall Hit.");
// }

// if (col.gameObject.name == "Orange Wall")
// {
// Debug.Log("Orange Wall Hit.");
// }




// }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallOld : MonoBehaviour
{
    // use an array or enum to keep track of where ball previously was, then hit thing
    enum PreviouslyCameFrom {Paddle, OrangeWall, PurpleWall, EnemyPaddle};
    
    public GameObject Player;
    public bool inPlay;
    // Start is called before the first frame update
    public float movementSpeed;
    public float dirx, diry, dirz;
    public Vector3 direction;
    bool okay = false;
    public GameObject sign;
    public GameObject paddle;
    public float angle;
    public int PreviousLocation;
    void Start()
    {
        inPlay = false;
        direction = Vector3.forward;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(paddle.transform.rotation.eulerAngles.y);
        if (transform.position.z >= 30)
        {
            transform.position = new Vector3(0f, 3.66f, 30f);
            sign.transform.position = new Vector3(15, 10, -5);
            angle = 90;
        }
       
        if (!inPlay) // inPlay == false
        {
            
        }
        else
        {
            transform.position += direction * Time.deltaTime * movementSpeed;
        }
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //start game
            inPlay = true;
            Debug.Log("click");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;
            transform.position = new Vector3(0,4,13);
            direction = new Vector3(0,0,1);
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
    void OnCollisionEnter(Collision col)
    {
        
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
            direction = new Vector3(x, 0, z);
            PreviousLocation = (int)PreviouslyCameFrom.Paddle;
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
        if (col.gameObject.name == "Opponent Wall" && !okay)
        {
            // x and z axes are concerned
            direction = Vector3.forward;
            ///*direction*/ = new Vector3(1, 0, 1);
            Debug.Log("Detected player 2 paddle");
        }
    }
}