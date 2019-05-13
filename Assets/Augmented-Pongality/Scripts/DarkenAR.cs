using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
\ Player Area\
|            |
|            |
|      .5    |
|            |
| ---------- |
|            |
|      .5    |
|            |
|            |
\ Player Area \
 */
public class DarkenAR : MonoBehaviour
{
    public CanvasGroup uiElement;

    public GameObject field;
    public float origPaddlePos;
    public float fieldBound;
    public float playerArea;
    public float front;
    public float back;

    // Start is called before the first frame update
    void Start()
    {
        //default host player field values
        front = 0.5f;
        back = -0.5f;

        uiElement.alpha = 0;


    }

    //Set the front and back values of Player2 field
    public void SetPlayer2Field(float middle)
    {
        Debug.Log("PLAYER 2 setting up");
        //front back flipped for player 2
        back = middle - 0.6f;
        front = middle - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        float pos = gameObject.transform.parent.transform.position.z;

            if (pos <= front && pos >= back)
            {
                uiElement.alpha = 0;
            }
            else
            {
                uiElement.alpha = 1;
            }
        
    }
}
