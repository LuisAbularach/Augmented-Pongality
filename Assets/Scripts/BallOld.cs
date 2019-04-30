using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallOld : MonoBehaviour
{
    // use an array or enum to keep track of where ball previously was, then hit thing
    enum PreviouslyCameFrom {Paddle, LeftWall, RightWall, EnemyPaddle};
    
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
    public static int PreviousLocation;

    
    void Start()
    {
        inPlay = false;
        direction = Vector3.forward;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(paddle.transform.rotation.eulerAngles.y);
        // if (transform.position.z >= 30)
        // {
        //     transform.position = new Vector3(0f, 3.66f, 30f);
        //     Score.enemy = true;
        //     Score.scoreUpdated = true;
        //     angle = 90;
        // }
       
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

        if (transform.position.z >= 30)
        {
            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;
            Score.enemy = true;
            transform.position = new Vector3(0,4,13);
            if (Score.your)
            {
                direction = new Vector3(0,0,1);
                Score.your = false;
            }
                
            else
            {
                direction = new Vector3(0, 0, -1);
                Score.enemy = false;
            }
                
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
        
        if (col.gameObject.name == "paddle")
        {
            Debug.Log("Detected paddle");
            
            Debug.Log("Euler angle y: " + paddle.transform.rotation.eulerAngles.y);
            float xAngle = paddle.transform.rotation.eulerAngles.y;
            float zAngle = paddle.transform.rotation.eulerAngles.y;
            float x = -1 * Mathf.Cos(DegreeToRadian(xAngle) + Mathf.PI + (Mathf.PI / 2) );
            float z = Mathf.Sin(DegreeToRadian(zAngle) + Mathf.PI + (Mathf.PI / 2));
            
            Debug.Log("cos x: " + RadianToDegree(x) + "sin z" + RadianToDegree(z));
            if (movementSpeed < 31)
                movementSpeed++;
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
            Vector3 vel = direction;
            
            direction = new Vector3(vel.x * -1, 0, vel.z);
            
            PreviousLocation = (int)PreviouslyCameFrom.LeftWall;
        }

        if (col.gameObject.name == "RightWall")
        {
            Vector3 vel = direction;
            
            direction = new Vector3(vel.x * -1, 0, vel.z);

            PreviousLocation = (int)PreviouslyCameFrom.RightWall;

        }
        if (col.gameObject.name == "Opponent Wall")
        {
            // x and z axes are concerned
            Vector3 vel = direction;
            ///*direction*/ = new Vector3(1, 0, 1);
            direction = new Vector3(vel.x, 0, vel.z * -1);
            Debug.Log("Detected Opponent Wall paddle");
            PreviousLocation = (int)PreviouslyCameFrom.EnemyPaddle;
        }
    }
}