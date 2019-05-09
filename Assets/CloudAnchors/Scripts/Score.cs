using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int yourScore;
    public static int enemyScore;
    // Start is called before the first frame update

    public delegate void ScoreChange(int yourScore,int enemyScore);
    public static event ScoreChange OnScoreChange;
    void Start()
    {
        yourScore = enemyScore = 0;
    }

 

    public static int ChangeScore(int whoWon)
    {
        int won;
        if (whoWon == 1) // you Won
        {
            yourScore++;
            // Debug.Log("Player 1 score: " + yourScore);
            won = 1;
        }
        else  // enemy won
        {
            enemyScore++;
            // Debug.Log("Player 2 Enemy won score: " + enemyScore);
            won = 2;
        }
        if(OnScoreChange!=null)
            OnScoreChange(yourScore,enemyScore);
            
        Debug.Log("Player 1 score: " + yourScore);
        Debug.Log("Player 2 Enemy Score: " + enemyScore);
        return won;
    }
}
