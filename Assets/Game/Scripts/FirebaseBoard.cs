using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseBoard : MonoBehaviour
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
        StartCoroutine(RefreshCR());
    }

    private IEnumerator RefreshCR()
    {
        yield return FirebaseSystem.Instance.RefreshFirebaseCR();
        var scores = FirebaseSystem.Instance.firebaseScores;

        if (scores.Count >= 1)
        {
            firstDisplay.Populate(1, scores[^1].Item2, scores[^1].Item1);
        }
        else
        {
            firstDisplay.Depopulate();
        }

        if (scores.Count >= 2)
        {
            secondDisplay.Populate(2, scores[^2].Item2, scores[^2].Item1);
        }
        else
        {
            secondDisplay.Depopulate();
        }
        
        if (scores.Count >= 3)
        {
            thirdDisplay.Populate(3, scores[^3].Item2, scores[^3].Item1);
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
    }
}
