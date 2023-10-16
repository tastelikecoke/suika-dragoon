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

        firstLocalDisplay.gameObject.SetActive(scores.Count >= 1);
        if (firstLocalDisplay.gameObject.activeSelf)
        {
            firstLocalDisplay.Populate(1, scores[^1]);
        }
        
        secondLocalDisplay.gameObject.SetActive(scores.Count >= 2);
        if (secondLocalDisplay.gameObject.activeSelf)
        {
            secondLocalDisplay.Populate(2, scores[^2]);
        }
        
        thirdLocalDisplay.gameObject.SetActive(scores.Count >= 3);
        if (thirdLocalDisplay.gameObject.activeSelf)
        {
            thirdLocalDisplay.Populate(3, scores[^3]);
        }

        if (manager == null)
            manager = FindObjectOfType<FruitManager>();

        if (manager != null)
        {
            currentLocalDisplay.Populate(GameSystem.Instance.GetLocalRank(manager.totalScore), manager.totalScore);
        }
    }
}
