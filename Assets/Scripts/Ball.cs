using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Player;
    bool inPlay;
    // Start is called before the first frame update
    public float movementSpeed;
    public float dirx,diry,dirz;
    Vector3 direction;
    bool okay = false;
    void Start()
    {
        inPlay = false;
        direction = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inPlay)
        {
            //Keep ball in position with camera
            //transform.parent =  Camera.main.transform;
            Debug.Log("inPlay");
        }
        else
        {
            transform.position += direction * Time.deltaTime * movementSpeed;
        }

        //Check for tap
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //start game
            inPlay = true;
            Debug.Log("click");

        }
        // if(inPlay && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     //start game
        //     inPlay = false;
        //     Debug.Log("click");

        // }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "paddle"|| col.gameObject.name == "Wall" || col.gameObject.name == "AR Default Plane"){
            // transform.position += Vector3.back * Time.deltaTime * movementSpeed;
            // // transform.Rotate(0,180,0);
            // direction = Vector3.back;
            //  if(col.gameObject.name == "paddle"&&!okay){
                
            //  }

            //turn sphere
            transform.rotation = Player.transform.rotation;

            // if(movementSpeed<0)
            //     movementSpeed -= 0.1f;
            // else
            //     movementSpeed += 0.1f;
            okay = true;
            //movementSpeed *= -1;
        }

    }
}
