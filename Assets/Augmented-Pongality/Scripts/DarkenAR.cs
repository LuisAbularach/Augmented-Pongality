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
|            |
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
    public float front, back2;
    public float back, front2;
    public GameObject P2Field;
    public bool inPlay;
    public int customizedSetting;

    // Start is called before the first frame update
    void Start()
    {
        //default host player field values
        front = 0.5f;
        back = -0.5f;

        uiElement.alpha = 0;

        customizedSetting = PlayerPrefs.GetInt("OutofBoundsTimer");

        inPlay = false;

    }

    //Set the front and back values of Player2 field
    public void SetPlayer2Field(float middle)
    {
        Debug.Log("PLAYER 2 setting up");
        //front back flipped for player 2
        back2 = middle + 0.5f;
        front2 = middle - 0.5f;
    }

    public void Stop()
    {

    }

    public void GameStarted(GameObject field, int timer)
    {
        inPlay = true;
        if (customizedSetting != timer)
        {
            SetPlayer2Field(field.transform.position.z);
            customizedSetting = timer;
        }
    }

    public void EndGame()
    {
        inPlay = false;
    }

    // Update is called once per frame
    void Update()
    {

        float pos = gameObject.transform.parent.transform.position.z;

            //Check to see if within one of the two player fields
            if ((pos <= front && pos >= back)||(pos >= front2 && pos <= back2) )
            {
                uiElement.alpha = 0;
            }
            else if(inPlay)
            {
                uiElement.alpha +=  0.005f *customizedSetting;
            }   
        
    }
}
