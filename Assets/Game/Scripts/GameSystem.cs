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
        localScores = new List<int>();
        DontDestroyOnLoad(this);
    }

    public void UploadLocalScore(int score)
    {
        localScores.Add(score);
        localScores.Sort();
    }

    public int GetLocalRank(int score)
    {
        var result = localScores.BinarySearch(score);
        if (result < 0) return localScores.Count + 1 - (~result);
        return localScores.Count - result + 1;
    }

}
