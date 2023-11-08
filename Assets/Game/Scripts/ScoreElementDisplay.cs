using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreElementDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    [SerializeField]
    private TMP_Text rank;
    [SerializeField]
    private TMP_Text playerName;
    
    public void Populate(int rank, int score, string playerName=null)
    {
        this.score.text = score.ToString();
        
        if(rank != Int32.MinValue)
            this.rank.text = rank.ToString();
        else
            this.rank.text = "--";
        
        if (playerName != null && this.playerName != null)
            this.playerName.text = playerName;
    }
    public void Depopulate()
    {
        this.score.text = "----";
        this.rank.text = "--";
        this.playerName.text = "---";
    }
}
