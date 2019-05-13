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
    public GameObject Score; //added by mo
    public float fieldLength;
    public float wallHeight;

    public float boundary; // If ball goes outside this number, player loses

    public float speedIncrease;
    public float maxSpeed;


    void Start()
    {
        Debug.Log("ANGLE IS: "+Vector3.Angle(new Vector3(2,2,0), new Vector3(0, 3, 0)));
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
        Score = GameObject.Find("Player2Zone");//added by mo
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("direction speed is: " + direction);
        float ballBound = transform.position.z;
        if (ballBound <= -1 * boundary || ballBound >= boundary)
        {

            transform.position = new Vector3(0, wallHeight / 2, 0); // ball is placed at middle height of the walls
            movementSpeed = Math.Abs(movementSpeed);
            inPlay = false;

            float randAngle = UnityEngine.Random.Range(65f, 115f);//65f;//UnityEngine.Random.Range(20f, 160f);
            Debug.Log("Winner\'s Random Angle is: " + randAngle);
            float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle));
            float zComp = Mathf.Sin(DegreeToRadian(randAngle));
            if (ballBound >= boundary)  // Enemy won
            {
                //Score.ChangeScore(2); //dont forget to fix this
                Score.GetComponent<Score>().ChangeScore(2); // added by mo
                if (zComp >= 0)
                    zComp *= -1;
            }
            else  // You won
            {
                //Score.ChangeScore(1); //dont forget to fix this
                Score.GetComponent<Score>().ChangeScore(1); // added by mo
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
            // float paddleYAngle = col.gameObject.transform.rotation.eulerAngles.y;
            int paddleYAngle; 
            float x, z;

            paddleYAngle = Mathf.RoundToInt(col.gameObject.transform.rotation.eulerAngles.y);
            Debug.Log("paddle Y angle before change is: " + paddleYAngle);
            if (paddleYAngle == 360)
                paddleYAngle = 0;
            if (paddleYAngle < 0)
                paddleYAngle += 360;
            x = Mathf.Cos(DegreeToRadian(paddleYAngle));
            z = Mathf.Sin(DegreeToRadian(paddleYAngle));

            // You want the angle of whichever paddle you hit or collided with
            Debug.Log("Euler angle y: " + paddleYAngle);
            if (col.gameObject.transform.position.z >= 0) // Your paddle, quads II and III
            {
                if (paddleYAngle == 0)
                {
                    z = -1;
                    x = 0;
                }
                if (paddleYAngle > 0 && paddleYAngle < 90)
                {
                    z *= -1;
                    x *= -1;
                }
                if (paddleYAngle == 90)
                {
                    z = -1;
                    x = 0;
                }
                if (paddleYAngle > 90  && paddleYAngle < 180)
                {
                   Debug.Log("X is: " + x + "Z is neg: " + z);
                   x *= -1;
                   z *= -1;
                }
                if (paddleYAngle == 180)
                {
                    z = -1;
                    x = 0;
                }
                
            }
            if (col.gameObject.transform.position.z < 0) // enemy paddle
            {
                if (paddleYAngle == 0 || paddleYAngle == 90 || paddleYAngle == 180 || paddleYAngle == 270)
                {
                    z = 1;
                    x = 0;
                }
                if (paddleYAngle > 180 && paddleYAngle < 270)
                {
                    x *= -1;
                    z *= -1;
                }
                if (paddleYAngle > 270 && paddleYAngle < 360)
                {
                    x *= -1;
                    z *= -1;
                }
            }
           

            Debug.Log("cos x: " + RadianToDegree(x) + " sin z: " + RadianToDegree(z));

            if (Mathf.Abs(movementSpeed) < maxSpeed)
            {
                float moveSpeed = Mathf.Abs(movementSpeed) + speedIncrease;
                if (movementSpeed < 0)
                {
                    movementSpeed = moveSpeed * -1;
                }
                else
                {
                    movementSpeed = moveSpeed;
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




 // float x, z;
            // if (direction.z < 0)
            // {
            //     x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2));
            //     z = Mathf.Sin(DegreeToRadian(paddleYAngle) + (Mathf.PI / 2));
            //     if (z < 0)
            //         z *= -1;
            // }
            // else
            // {
            //     x = -1 * Mathf.Cos(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2));
            //     z = Mathf.Sin(DegreeToRadian(paddleYAngle) + Mathf.PI + (Mathf.PI / 2));
            // }



             // keep angle between quadrants III & IV
            // if ((paddleYAngle >= 0 && paddleYAngle <= 90) ||
            //     (paddleYAngle > 180 && paddleYAngle < 270))
            // {
            //     Debug.Log("0 < rotation < 90");


            //     if (paddle.transform.rotation.eulerAngles.y > 180 && paddle.transform.rotation.eulerAngles.y < 270)
            //     {
            //         Debug.Log("Reverse");
            //         movementSpeed = movementSpeed * -1;
            //     }
            // }

            // else
            // {
            //     Debug.Log("-90 < rotation < 0");

            //     if (paddle.transform.rotation.eulerAngles.y > 90 && paddle.transform.rotation.eulerAngles.y < 180)
            //     {
            //         Debug.Log("Reverse");
            //         movementSpeed = movementSpeed * -1;
            //     }
            // }
