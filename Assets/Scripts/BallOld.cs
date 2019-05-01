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
        if (transform.position.z <=  -30)
        {
            // transform.position = new Vector3(0f, 3.66f, 0f);
            Score.ChangeScore(1);
            // direction = new Vector3(0f, 0f, 1f);
            angle = 90;

            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;
            transform.position = new Vector3(0, 4, 0);
            float randAngle = UnityEngine.Random.Range(10f, 170f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            direction = new Vector3(xComp, 0, zComp);
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

        if (transform.position.z >= 30) // Enemy Won
        {
            Score.ChangeScore(2);
            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;
            transform.position = new Vector3(0, 4, 0);
            float randAngle = UnityEngine.Random.Range(10f, 170f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            if (zComp >= 0)
                zComp *= -1;
            direction = new Vector3(xComp, 0, zComp);
            
                
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
                movementSpeed++;
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