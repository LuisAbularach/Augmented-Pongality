using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Player;
    public bool inPlay = false;
    public float movementSpeed;
    public float dirx,diry,dirz;
    Vector3 direction;
    bool okay = false;
    float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START BALL!!!");
        inPlay = false;
        direction = Vector3.forward;
//        GameObject.name = "Ball";
        distance = 0.1f;

        Player = GameObject.Find("LocalPlayer");


    }

    // Update is called once per frame
    void Update()
    {
            //Keep ball in position with camera
            //transform.parent =  Camera.main.transform;
            //Debug.Log("inPlay");

        
        
        if(inPlay)
        {
            Debug.Log("MOVING");
            transform.position += direction * Time.deltaTime * movementSpeed;
        }
        else
        {
            Debug.Log("NOT MOVING");
            transform.position = Player.transform.position + transform.forward * 0.1f;
        }

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
        if(col.gameObject.name == "paddle"|| col.gameObject.name == "Wall" || col.gameObject.name == "AR Default Plane"){
            transform.position += Vector3.back * Time.deltaTime * movementSpeed;
            // transform.Rotate(0,180,0);
            direction = Vector3.back;
             if(col.gameObject.name == "paddle"&&!okay){
                
             }

            //turn sphere
            //transform.rotation = Player.transform.rotation;

            if(movementSpeed<0)
                movementSpeed -= 0.1f;
            else
                movementSpeed += 0.1f;
            okay = true;
            //movementSpeed *= -1;
        }

    }

    // public void SetPosition(GameObject camera)
    // {
    //      transform.parent = camera.transform.position + camera.transform.position.forward * distance;
    // }
}
