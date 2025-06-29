using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Tracks score updates
    /// </summary>
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text score;
        [SerializeField]
        private FruitManager fruitManager;

        public void Start()
        {
            OnScoreChanged(fruitManager.totalScore);
            fruitManager.OnScoreChanged += OnScoreChanged;
        }
        public void OnDestroy()
        {
            fruitManager.OnScoreChanged -= OnScoreChanged;
        }

        public void OnScoreChanged(int totalScore)
        {
            score.text = totalScore.ToString();
        }
    }
}