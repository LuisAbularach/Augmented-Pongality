using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int yourScore;
    public static int enemyScore;
    // Start is called before the first frame update

    
    void Start()
    {
        yourScore = enemyScore = 0;
    }

 

    public static int ChangeScore(int whoWon)
    {
        if (whoWon == 1) // you Won
        {
            yourScore++;
            Debug.Log("Player 1 score: " + yourScore);
            return 1;
        }
        else  // enemy won
        {
            enemyScore++;
            Debug.Log("Player 2 Enemy won score: " + enemyScore);
            return 2;
        }
    }
}
