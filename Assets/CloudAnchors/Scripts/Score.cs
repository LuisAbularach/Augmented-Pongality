using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Score : NetworkBehaviour
{
    [SyncVar]
    public int yourScore;
    [SyncVar]
    public int enemyScore;
    // Start is called before the first frame update

    public delegate void ScoreChange(int yourScore,int enemyScore);
    public static event ScoreChange OnScoreChange;
    public bool update;
    void Start()
    {
        update = false;
        yourScore = enemyScore = 0;
    }

    /* void Update()
    {
        if(update)
            CheckScoreUpdates();
    }*/

    public int ChangeScore(int whoWon) //removed static
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
            
        RpcCheckScoreUpdates();
        Debug.Log("Player 1 score: " + yourScore);
        Debug.Log("Player 2 Enemy Score: " + enemyScore);
        return won;
    }

    [ClientRpc]
    public void RpcCheckScoreUpdates()
    {
        //Client needs to check to see if their score is same as host
        if(OnScoreChange!=null)
            OnScoreChange(yourScore,enemyScore);
    }
}
