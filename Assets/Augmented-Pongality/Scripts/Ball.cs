using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace GoogleARCore.Examples.CloudAnchors{
public class Ball : NetworkBehaviour
{
    public GameObject StartButton;
     enum PreviouslyCameFrom {Paddle, LeftWall, RightWall, EnemyPaddle};
    public GameObject Player;
    private Rigidbody rb;
    public bool SinglePlayer;
    [SyncVar] public bool inPlay;
    [SyncVar] public float movementSpeed;
    public float dirx,diry,dirz;
    [SyncVar] public Vector3 direction;

    [SyncVar] public float P2Back;
    public float P1back;
    bool okay = false;
    float distance;
    public GameObject paddle;
    public GameObject Camera;
    public GameObject Canvas;
    public GameObject _net_manager;

    public bool isP2;

    public float angle;
    public int PreviousLocation;
    public int ScoreToWin, P1score, P2score;
    public bool darkenSet;

    public delegate void SetUpComplete();
    public static event SetUpComplete OnSetUpComplete;
    
    public GameObject Score; // added because removed static from score
    bool setUpComplete = true;
    // Start is called before the first frame update
    void Start()
    {
        //Load preferences
        ScoreToWin = PlayerPrefs.GetInt("maxScore");
        P1score = 0;
        P2score = 0;

        SinglePlayer = false;
        isP2 = true;
        darkenSet = false;
        P1back = -.5f;
        Debug.Log("START BALL!!!");
        inPlay = false;
        direction = Vector3.forward;
//        GameObject.name = "Ball";
        distance = 0.1f;
        Score = GameObject.Find("Player2Zone(Clone)"); // attached Score.cs to this object
        if(Score != null)
            P2Back = Score.transform.position.z + 0.5f;

        paddle = GameObject.Find("paddle");
        Player = GameObject.Find("LocalPlayer");

        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        rb = gameObject.GetComponent<Rigidbody>(); 
    }


    // Update is called once per frame
    void Update()
    {
        if(inPlay && !SinglePlayer){
            //Score
            if (transform.position.z >=  P2Back)
            {
                P1score++;
                Debug.Log("PLAYER 1: " + P1score + "Score to win: " + ScoreToWin);
                if(P1score == ScoreToWin)
                {
                    Debug.Log("RESTING GAME BOARD");
                    Reset();
                }
                else{
                    // transform.position = new Vector3(0f, 3.66f, 0f);
                    if(!isP2)
                        Score.GetComponent<Score>().ChangeScore(1);
                    
                    // direction = new Vector3(0f, 0f, 1f);
                    angle = 90;

                    movementSpeed = Mathf.Abs(.5f);
                    //inPlay = false;
                    //Create random angle to throw the ball from center
                    float randAngle = UnityEngine.Random.Range(65f, 115f);
                    Debug.Log("Winner\'s Random Angle is: " + randAngle);
                    float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
                    float zComp = Mathf.Sin(DegreeToRadian(randAngle));
                    if (zComp >= 0)
                        zComp *= -1;
                    if(!isP2)
                        direction = new Vector3(xComp, 0, zComp);
                }
            }


            if (transform.position.z <= -0.5f) // Enemy Won
            {
                P2score++;
                if(P2score == ScoreToWin)
                {
                    Reset();
                }
                else{
                    Score.GetComponent<Score>().ChangeScore(2);
                    movementSpeed = Mathf.Abs(.2f);
                    //inPlay = false;
                    //transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);
                    float randAngle = UnityEngine.Random.Range(65f, 115f);
                    Debug.Log("Winner\'s Random Angle is: " + randAngle);
                    float xComp = -1 * Mathf.Cos(DegreeToRadian(randAngle) );
                    float zComp = Mathf.Sin(DegreeToRadian(randAngle));

                    if(!isP2)
                        direction = new Vector3(xComp, 0, zComp);
                }
            }


            if (transform.position.z >  P2Back) 
            {
                transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);

            }
            if (transform.position.z < -0.5f)
            {
                transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);
            }

            transform.position += direction * Time.deltaTime * movementSpeed;
            if(OnSetUpComplete != null && setUpComplete)
            {   OnSetUpComplete();
                setUpComplete=false;
            }

            //P2 needs to change the values to their field
            if(isP2&&!darkenSet)
            {
                GameObject.Find("DarkenCanvas").SetActive(true);
                Camera.GetComponent<DarkenAR>().SetPlayer2Field(P2Back - .5f);
                darkenSet = true;
            }
            
        }




        // }
        if(paddle!=null){
            //Debug.Log(paddle.transform.rotation.y);
        }

        //Single player not implimented
        if(SinglePlayer && inPlay)
        {
            transform.position = new Vector3(0, 1, ((P2Back - 1)/2) + 0.5f);

            if(transform.position.z < -0.5f)
            {
                transform.position = new Vector3(0,0,0);
                movementSpeed = 0.5f;
            }
        }
    }

    public void Reset()
    {
        Debug.Log("RESET");
        //Call when game is won
        inPlay = false;
        movementSpeed = 0.5f;

        transform.position = new Vector3(0, 0.5f, 0);

        StartButton.SetActive(true);

        Score.GetComponent<Score>().ResetScore();
    }

    public void StartBallMovement(GameObject Button)
    {
        StartButton = Button;
        StartButton.SetActive(false); 

        Debug.Log("inPlay: " + inPlay);
        Debug.Log("Start Ball Moving");
        inPlay = true;
        isP2 = false;
        Debug.Log(inPlay);

        // transform.position += direction * Time.deltaTime * movementSpeed;

        //Enable panel to darken screen
        Canvas = GameObject.Find("DarkenCanvas");
        //Canvas.SetActive(true);
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
        //When colides on server we sync position of host to client
        // if(!isP2)
        //     transform.position = transform.position;

        Debug.Log(col.gameObject.name);
          if (col.gameObject.name == "paddle")
        {
            //when ball hits camera vibrate
            Handheld.Vibrate();

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
            if (movementSpeed < 15 && movementSpeed > 0)
                movementSpeed += 0.5f;//(movementSpeed + 1);
            else if(movementSpeed < 0 && movementSpeed > -15)
                movementSpeed += -0.5f;

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
      
        if (col.gameObject.name == "LeftWall" && isServer)
        {
            Vector3 vel = direction;
            
            Player.GetComponent<LocalPlayerController>().CmdSetProperties( new Vector3(vel.x * -1, 0, vel.z),movementSpeed);
            
            PreviousLocation = (int)PreviouslyCameFrom.LeftWall;
        }

        if (col.gameObject.name == "RightWall" && isServer)
        {
            Vector3 vel = direction;
            
            Player.GetComponent<LocalPlayerController>().CmdSetProperties(new Vector3(vel.x * -1, 0, vel.z),movementSpeed);

            PreviousLocation = (int)PreviouslyCameFrom.RightWall;

        }

    }
    public void CmdsetP2Back(float back)
    {
        P2Back = back;
    }

    [ClientRpc]
    public void RpcRemoveSnackbar()
    {
        _net_manager = GameObject.Find("Network Manager");
        _net_manager.GetComponent<NetworkManagerUIController>().RemoveSnackbar();
    }
}
}