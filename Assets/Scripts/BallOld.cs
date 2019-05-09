using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallOld : MonoBehaviour
{


    public GameObject Player;
    public bool inPlay;
    // Start is called before the first frame update
    public float movementSpeed;
    public float dirx, diry, dirz;
    public Vector3 direction;
    bool okay = false;
    public GameObject field; // get the length of field from transform's scale
    public GameObject paddle;
    public GameObject wall; // assumes left wall & right wall are both the same height

    public float fieldLength;
    public float wallHeight;

    public float boundary; // If ball goes outside this number, player loses

    public float speedIncrease;
    public float maxSpeed;


    void Start()
    {
        
        inPlay = false;
        //direction = Vector3.forward;
        fieldLength = field.transform.lossyScale.z;
        Debug.Log("FIELD LENGTH: " + fieldLength);
        wallHeight = wall.transform.lossyScale.y;
        Debug.Log("WALL HEIGHT: " + wallHeight);
        boundary = (fieldLength / 2) + (fieldLength / 4); 
        // when ball goes past field length plus and extra 25% of field length, player loses, the 25% accounts for player zone
        Debug.Log("BOUNDARY: " + boundary);
        speedIncrease = fieldLength / 10; // speed of ball will increase by 10% each time it hits the paddle
        Debug.Log("Speed Increase Unit: " + speedIncrease);
        maxSpeed = fieldLength;
        Debug.Log("Max speed is: " + maxSpeed);
        direction = new Vector3(0, 0, speedIncrease);
        movementSpeed = speedIncrease;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("direction speed is: " + direction);
        float ballBound = transform.position.z;
        if (ballBound <= -1 * boundary || ballBound >= boundary)
        {
            
            transform.position = new Vector3(0, wallHeight/2, 0); // ball is placed at middle height of the walls
            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;
            
            float randAngle = UnityEngine.Random.Range(10f, 170f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle));
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            if (ballBound >= boundary)  // Enemy won
            {
                Score.ChangeScore(2);
                if (zComp >= 0)
                    zComp *= -1;
            }
            else  // You won
            {
                Score.ChangeScore(1);
            }

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
        if (col.gameObject.name != "LeftWall" && col.gameObject.name != "RightWall" &&
            col.gameObject.name != "Opponent Wall") // This takes care of same script being used on different paddles
        {
            Debug.Log("Detected paddle of: " + col.gameObject.name);
            float paddleYAngle = col.gameObject.transform.rotation.eulerAngles.y; 
            // You want the angle of whichever paddle you hit or collided with
            Debug.Log("Euler angle y: " + paddleYAngle);

            float x, z;
            if (direction.z < 0)
            {
                x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2));
                z = Mathf.Sin(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2));
            }
            else
            {
                x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2));
                z = Mathf.Sin(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2));
            }


            Debug.Log("cos x: " + RadianToDegree(x) + " sin z: " + RadianToDegree(z));
            if (movementSpeed < maxSpeed)
                movementSpeed += speedIncrease;
            // keep angle between quadrants III & IV
            if ((paddleYAngle >= 0 && paddleYAngle <= 90) ||
                (paddleYAngle > 180 && paddleYAngle < 270))
            {
                Debug.Log("0 < rotation < 90");


                if (paddle.transform.rotation.eulerAngles.y > 180 && paddle.transform.rotation.eulerAngles.y < 270)
                {
                    Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }

            else
            {
                Debug.Log("-90 < rotation < 0");

                if (paddle.transform.rotation.eulerAngles.y > 90 && paddle.transform.rotation.eulerAngles.y < 180)
                {
                    Debug.Log("Reverse");
                    movementSpeed = movementSpeed * -1;
                }
            }

            direction = new Vector3(x, 0, z);

        }






        if (col.gameObject.name == "LeftWall")
        {
            Vector3 vel = direction;

            direction = new Vector3(vel.x * -1, 0, vel.z);


        }

        if (col.gameObject.name == "RightWall")
        {
            Vector3 vel = direction;

            direction = new Vector3(vel.x * -1, 0, vel.z);



        }
        if (col.gameObject.name == "Opponent Wall")
        {
            // x and z axes are concerned
            Vector3 vel = direction;
            
            direction = new Vector3(vel.x, 0, vel.z * -1);
            Debug.Log("Detected Opponent Wall paddle");

        }
    }
}