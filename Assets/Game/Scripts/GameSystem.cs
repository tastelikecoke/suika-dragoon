using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public List<int> localScores;
    public AudioSource bgm;

    public static GameSystem Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        string storedScores = PlayerPrefs.GetString("dragoon_drop_save_file_for_local_ranking", null);
        localScores = new List<int>();
        if (!string.IsNullOrEmpty(storedScores))
        {
            var scores = storedScores.Split(" ");
            foreach(var score in scores)
            {
                int newScore = 0;
                Int32.TryParse(score, out newScore);
                if (newScore != 0)
                {
                    localScores.Add(newScore);
                }
            }
        }
        DontDestroyOnLoad(this);
    }

    public void UploadLocalScore(int score)
    {
        localScores.Add(score);
        localScores.Sort();

        string storedScore = "";
        foreach(var scoreInt in localScores)
        {
            if (storedScore == "") storedScore = scoreInt.ToString();
            else storedScore = storedScore + " " + scoreInt.ToString();
        }

        PlayerPrefs.SetString("dragoon_drop_save_file_for_local_ranking", storedScore);
        PlayerPrefs.Save();
    }

    public int GetLocalRank(int score)
    {
        var result = localScores.BinarySearch(score);
        if (result < 0) return localScores.Count + 1 - (~result);
        return localScores.Count - result + 1;
    }

}
