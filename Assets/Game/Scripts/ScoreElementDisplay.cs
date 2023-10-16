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
    
    public void Populate( int rank, int score)
    {
        this.score.text = score.ToString();
        this.rank.text = rank.ToString();
    }
    public void Depopulate()
    {
        this.score.text = "----";
        this.rank.text = "--";
    }
}
