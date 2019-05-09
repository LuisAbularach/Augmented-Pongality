using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    public float speed;
    public float speedRot;
    // public GameObject field;
    // Start is called before the first frame update
    public float paddlePos;
    
    void Start()
    {
        speed = 5f;
        speedRot = 40f;
        paddlePos = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z >= -1 || gameObject.transform.position.z <= 1)
            
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.down, speedRot * Time.deltaTime);
            float x = Mathf.Cos(DegreeToRadian(transform.rotation.eulerAngles.y));
            float z = Mathf.Sin(DegreeToRadian(transform.rotation.eulerAngles.y));
            Debug.Log("ANGLE Y is: " + transform.rotation.eulerAngles.y);
            Debug.Log("X-COMP: " + x + " Z-COMP: " + z);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.up, speedRot * Time.deltaTime);
            float x = Mathf.Cos(DegreeToRadian(transform.rotation.eulerAngles.y));
            float z = Mathf.Sin(DegreeToRadian(transform.rotation.eulerAngles.y));
            Debug.Log("ANGLE Y is: " + transform.rotation.eulerAngles.y);
            Debug.Log("X-COMP: " + x + " Z-COMP: " + z);
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x > -8)
            transform.Translate(speed * Vector3.left * Time.deltaTime * speed, Space.World);

        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x < 9)
            transform.Translate(speed * Vector3.right * Time.deltaTime * speed, Space.World);
        
        if (Input.GetKey(KeyCode.W))
        {
            if (paddlePos >= 0)
                transform.Translate(speed * new Vector3(0, 0, -1) * Time.deltaTime, Space.World);
            else
                transform.Translate(speed * new Vector3(0, 0, 1) * Time.deltaTime, Space.World);
        }
            
        
        if (Input.GetKey(KeyCode.S))
        {
            if (paddlePos >= 0)
                transform.Translate(speed * new Vector3(0, 0, 1) * Time.deltaTime, Space.World);
            else
                transform.Translate(speed * new Vector3(0, 0, -1) * Time.deltaTime, Space.World);
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
}
