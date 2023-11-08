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
    
    public bool alreadyUploaded = false;
    
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
        if (FirebaseDatabase.DefaultInstance == null)
        {
            yield break;
        }

        var GetScores = FirebaseDatabase.DefaultInstance.GetReference("scores").OrderByChild("score").LimitToLast(3).GetValueAsync();
        
        yield return new WaitUntil(() => GetScores.IsCompleted);
        firebaseScores.Clear();
        foreach (var entry in GetScores.Result.Children)
        {
            firebaseScores.Add(new Tuple<string, int>(entry.Child("name").Value.ToString(), Int32.Parse(entry.Child("score").Value.ToString())));
        }
        firebaseScores.Sort((a, b) => a.Item2 - b.Item2);
    }
    public IEnumerator UploadToFirebaseCR(string playerName, int score, TMP_Text waitText)
    {
        if (alreadyUploaded)
        {
            waitText.text = "already uploaded";
            yield break;
        }
        
        if (FirebaseDatabase.DefaultInstance == null)
        {
            yield break;
        }
        
        var newScore = FirebaseDatabase.DefaultInstance.GetReference("scores").Push();

        var SetName = newScore.Child("name").SetValueAsync(playerName);
        yield return new WaitUntil(() => SetName.IsCompleted);
        
        var SetScore = newScore.Child("score").SetValueAsync(score);
        yield return new WaitUntil(() => SetScore.IsCompleted);

        waitText.text = "Done!";
        alreadyUploaded = true;
    }
}
