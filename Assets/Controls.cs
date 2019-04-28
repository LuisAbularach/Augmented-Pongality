using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    public float speed;
    public float speedRot;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
        speedRot = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.down, speedRot * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.up, speedRot * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x > -8)
            transform.Translate(speed * Vector3.left * Time.deltaTime * speed, Space.World);

        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x < 9)
            transform.Translate(speed * Vector3.right * Time.deltaTime * speed, Space.World);

    }
}
