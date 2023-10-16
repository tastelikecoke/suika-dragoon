using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    private ScoreElementDisplay firstLocalDisplay;
    [SerializeField]
    private ScoreElementDisplay secondLocalDisplay;
    [SerializeField]
    private ScoreElementDisplay thirdLocalDisplay;
    [SerializeField]
    private ScoreElementDisplay currentLocalDisplay;
    private FruitManager manager;
    private void Update()
    {
        if (GameSystem.Instance == null) return;

        var scores = GameSystem.Instance.localScores;

        if (scores.Count >= 1)
        {
            firstLocalDisplay.Populate(1, scores[^1]);
        }
        else
        {
            firstLocalDisplay.Depopulate();
        }
        
        if (scores.Count >= 2)
        {
            secondLocalDisplay.Populate(2, scores[^2]);
        }
        else
        {
            secondLocalDisplay.Depopulate();
        }
        
        if (scores.Count >= 3)
        {
            thirdLocalDisplay.Populate(3, scores[^3]);
        }
        else
        {
            thirdLocalDisplay.Depopulate();
        }


        if (manager == null)
            manager = FindObjectOfType<FruitManager>();

        if (manager != null)
        {
            currentLocalDisplay.Populate(GameSystem.Instance.GetLocalRank(manager.totalScore), manager.totalScore);
        }
    }
}
