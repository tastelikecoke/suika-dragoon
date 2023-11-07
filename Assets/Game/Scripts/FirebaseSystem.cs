using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;

public class FirebaseSystem : MonoBehaviour
{
    public List<Tuple<string,int>> firebaseScores;
    
    public static FirebaseSystem Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        firebaseScores = new List<Tuple<string,int>>();
        DontDestroyOnLoad(this);
    }
    
    public IEnumerator RefreshFirebaseCR()
    {
        var GetScores = FirebaseDatabase.DefaultInstance.GetReference("scores").OrderByValue().LimitToLast(3).GetValueAsync();
        
        yield return new WaitUntil(() => GetScores.IsCompleted);
        firebaseScores.Clear();
        foreach (var entry in GetScores.Result.Children)
        {
            firebaseScores.Add(new Tuple<string, int>(entry.Key, Int32.Parse(entry.Value.ToString())));
        }
        firebaseScores.Sort((a, b) => b.Item2 - a.Item2);
    }
}
