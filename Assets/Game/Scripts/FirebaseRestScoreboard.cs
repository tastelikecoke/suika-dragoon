using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseRestScoreboard : MonoBehaviour
{
    [SerializeField]
    private ScoreElementDisplay firstDisplay;
    [SerializeField]
    private ScoreElementDisplay secondDisplay;
    [SerializeField]
    private ScoreElementDisplay thirdDisplay;
    [SerializeField]
    private ScoreElementDisplay currentDisplay;
    
    private FruitManager manager;

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        FirebaseRestSystem.Instance.GetTopScores(dbr =>
        {
            var scores = FirebaseRestSystem.Instance.ScoreEntries;

            if (scores.Count >= 1)
            {
                firstDisplay.Populate(1, scores[^1].score, scores[^1].name);
            }
            else
            {
                firstDisplay.Depopulate();
            }

            if (scores.Count >= 2)
            {
                secondDisplay.Populate(2, scores[^2].score, scores[^2].name);
            }
            else
            {
                secondDisplay.Depopulate();
            }
        
            if (scores.Count >= 3)
            {
                thirdDisplay.Populate(3, scores[^3].score, scores[^3].name);
            }
            else
            {
                thirdDisplay.Depopulate();
            }
        
            if (manager == null)
                manager = FindObjectOfType<FruitManager>();
        
            if (manager != null)
            {
                currentDisplay.Populate(Int32.MinValue, manager.totalScore, "You");
            }
        });
    }
}
