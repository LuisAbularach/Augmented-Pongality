using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int yourScore;
    public static int enemyScore;

    public static bool your;
    public static bool enemy;
    // Start is called before the first frame update

    public static bool scoreUpdated;
    void Start()
    {
        yourScore = enemyScore = 0;
        your = enemy = scoreUpdated = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy && scoreUpdated) // enemy one reset round
        {
            scoreUpdated = false;
            enemyScore++;
            // enemy = false;
            Debug.Log("Enemy Score is: " + enemyScore);
            
        }
        if (your && scoreUpdated)
        {
            scoreUpdated = false;
            yourScore++;
            Debug.Log("Your Score is: " + yourScore);
            
            // your = false;
        }
    }
}
