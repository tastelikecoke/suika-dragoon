using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    [SerializeField]
    private FruitManager _fruitManager;

    public void Update()
    {
        score.text = _fruitManager.totalScore.ToString();
    }
}
