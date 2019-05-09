using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
\ Player Area\
|            |
|            |
|            |
|            |
|            |
|            |
|            |
|            |
|            |
\ Player Area \
 */
public class Darken : MonoBehaviour
{
    public CanvasGroup uiElement;

    public GameObject field;
    public float origPaddlePos;
    public float fieldBound;
    public float playerArea;
    public float lowerBound;
    public float upperBound;


    // Start is called before the first frame update
    void Start()
    {
        uiElement.alpha = 0;
        origPaddlePos = gameObject.transform.parent.transform.position.z;
        fieldBound = field.transform.lossyScale.z;
        playerArea = fieldBound / 10; 
        // player area will start at edge of field plus 10% of field length, use z key to go forward, use s to go backward
        if (origPaddlePos >= 0)
        {
            lowerBound = fieldBound / 2;
            upperBound = lowerBound + playerArea;
        }
        else
        {
            lowerBound = fieldBound / 2 * -1;
            upperBound = lowerBound + playerArea * -1;
        }



    }

    // Update is called once per frame
    void Update()
    {
        float pos = gameObject.transform.parent.transform.position.z;

        if (origPaddlePos >= 0)
        {
            if (pos >= lowerBound && pos <= upperBound)
            {
                uiElement.alpha = 0;
            }
            else
            {
                uiElement.alpha = 1;
            }
        }
        else
        {
            if (pos <= lowerBound && pos >= upperBound)
            {
                uiElement.alpha = 0;
            }
            else
            {
                uiElement.alpha = 1;
            }
        }

    }
}
