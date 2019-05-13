using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Add timer, longer you are outside of player zone, darker it becomes.
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

    public bool paddleOutsideOfArea;
    public int timeOutisdeOfArea;

    public int userCustomizeSetting; // User will be able to choose how fast screen darkens, must be between 1 - 10, will mean 0.01 to 0.10

    public float count;
    public int previous;
    public bool startCountingDown;
    public float countdownStartTime;
    public int timeLeft; // You can change this so countdown starts from 10 seconds, 15 secs, etc
    public int currentTimeLeft; // Make sure this is always greater than 0, for it to work
    public GameObject countDownText;
    public GameObject enemyCountDownText;
    public GameObject darkCube;
    public GameObject yourPanel;
    public GameObject enemyPanel;

    // Start is called before the first frame update
    void Start()
    {
        // userCustomizeSetting = 1;

        uiElement.alpha = 0;
        origPaddlePos = gameObject.transform.parent.transform.position.z;
        fieldBound = field.transform.lossyScale.z;
        playerArea = fieldBound / 10;
        // player area will start at edge of field plus 10% of field length, use z key to go forward, use s to go backward
        paddleOutsideOfArea = false;
        startCountingDown = false;
        timeLeft = 5;
        currentTimeLeft = 5;
        enemyCountDownText.gameObject.SetActive(false);
        countDownText.gameObject.SetActive(false);
        darkCube.SetActive(false);

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
        Debug.Log("Lower bound is: " + lowerBound);
        Debug.Log("Upper bound is: " + upperBound);


    }

    // Update is called once per frame
    void Update()
    {
        float pos = gameObject.transform.parent.transform.position.z;

        if (origPaddlePos >= 0) // Your paddle
        {
            if (pos >= lowerBound && pos <= upperBound)
            {
                paddleOutsideOfArea = false;
                uiElement.alpha = 0;
                count = 0;
                startCountingDown = false;
                timeLeft = 5;
                currentTimeLeft = 5;
                countDownText.SetActive(false);
                darkCube.SetActive(false);
                yourPanel.SetActive(true);
            }
            else
            {
                if (!paddleOutsideOfArea)
                {
                    timeOutisdeOfArea = Mathf.RoundToInt(Time.fixedTime);
                    Debug.Log("Time started is: " + Time.fixedTime);
                    paddleOutsideOfArea = true;

                    count = 0;
                    previous = 0;

                }

                float darkAlpha = Time.fixedTime - timeOutisdeOfArea;
                int integerAlpha = Mathf.RoundToInt(darkAlpha);
                float remainder = darkAlpha % 1;
                int remain = Mathf.RoundToInt(remainder * 10);
                if (remain != previous)
                {
                    previous = remain;
                    count += (userCustomizeSetting / 100f);
                    uiElement.alpha = count;
                }
                if (count >= 1f && !startCountingDown)
                {
                    startCountingDown = true;
                    countdownStartTime = Time.fixedTime;
                    Debug.Log("Count down start time is: " + countdownStartTime);
                    countDownText.SetActive(true);
                    darkCube.SetActive(true);
                    float darkCubePos = transform.parent.position.z;
                    
                    darkCube.transform.position = new Vector3(0, 4, darkCubePos);
                    yourPanel.SetActive(false);

                }
                if (darkCube.activeSelf)
                {
                    float darkCubePos = transform.parent.position.z;
                    
                    
                    darkCube.transform.position = new Vector3(0, 4, darkCubePos);
                    darkCube.transform.rotation = transform.parent.rotation;
                }
                if (startCountingDown && currentTimeLeft > 0)
                {
                    float countdown = Time.fixedTime - countdownStartTime;
                    currentTimeLeft = timeLeft - Mathf.RoundToInt(countdown);
                    Debug.Log("Countdown now: " + currentTimeLeft);
                    Text texty = countDownText.gameObject.GetComponent<Text>();
                    if (currentTimeLeft == 0)
                        texty.text = "Game Over";
                    else
                        texty.text = currentTimeLeft.ToString();
                }

            }
        }

        else // ENEMY PADDLE
        {
            if (pos <= lowerBound && pos >= upperBound)
            {
                paddleOutsideOfArea = false;
                uiElement.alpha = 0;
                count = 0;
                startCountingDown = false;
                timeLeft = 5;
                currentTimeLeft = 5;
                enemyCountDownText.SetActive(false);
                darkCube.SetActive(false);
                enemyPanel.SetActive(true);
            }


            else
            {
                if (!paddleOutsideOfArea)
                {
                    paddleOutsideOfArea = true;
                    timeOutisdeOfArea = Mathf.RoundToInt(Time.fixedTime);
                    Debug.Log("Time started is: " + Time.fixedTime);
                    count = 0;
                    previous = 0;
                }
                float darkAlpha = Time.fixedTime - timeOutisdeOfArea;
                int integerAlpha = Mathf.RoundToInt(darkAlpha);
                float remainder = darkAlpha % 1;
                int remain = Mathf.RoundToInt(remainder * 10);
                if (remain != previous && count < 1f)
                {
                    previous = remain;
                    count += (userCustomizeSetting / 100f);
                    uiElement.alpha = count;
                }
                if (count >= 1f && !startCountingDown)
                {
                    startCountingDown = true;
                    countdownStartTime = Time.fixedTime;
                    Debug.Log("Count down start time is: " + countdownStartTime);
                    enemyCountDownText.SetActive(true);
                    darkCube.SetActive(true);
                    float darkCubePos = transform.parent.position.z;

                   

                    darkCube.transform.position = new Vector3(0, 4, darkCubePos);
                    darkCube.transform.rotation = transform.parent.rotation;
                    
                    enemyPanel.SetActive(false);
                }
                if (darkCube.activeSelf)
                {
                    float darkCubePos = transform.parent.position.z;
                    
                    
                    darkCube.transform.position = new Vector3(0, 4, darkCubePos);
                    darkCube.transform.rotation = transform.parent.rotation;
                }

                if (startCountingDown && currentTimeLeft > 0)
                {
                    float countdown = Time.fixedTime - countdownStartTime;
                    currentTimeLeft = timeLeft - Mathf.RoundToInt(countdown);
                    Debug.Log("Countdown now: " + currentTimeLeft);
                    Text texty = enemyCountDownText.gameObject.GetComponent<Text>();
                    if (currentTimeLeft == 0)
                        texty.text = "Game Over";
                    else
                        texty.text = currentTimeLeft.ToString();
                }

            }
        }

    }
}


// countdownDisplay = new GameObject("CountdownText");
// countdownDisplay.AddComponent<Text>();
// Text texty = countdownDisplay.GetComponent<Text>();
// texty.text = "Game Over";
//  texty.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
// texty.fontSize = 100;
// texty.fontStyle = FontStyle.Bold;
// texty.color = Color.red;
// countdownDisplay.transform.parent = transform.Find("Canvas/Panel");