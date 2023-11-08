using System;
using System.Collections;
using System.Collections.Generic;
using FirebaseREST;
using Proyecto26;
using TMPro;
using UnityEngine;

public class FirebaseRestSystem : MonoBehaviour
{
    [Serializable]
    public struct ScoreEntry
    {
        public string name;
        public int score;
    }
    
    [SerializeField]
    private string databaseName;
    [SerializeField]
    private string rootNode;
    [SerializeField]
    private TMP_Text testText;

    [SerializeField]
    private List<ScoreEntry> _scoreEntries;
    private void Start()
    {
        _scoreEntries = new List<ScoreEntry>();
        AddScore("Jimmy", 50);
        GetTopScores();
    }

    private void GetTopScores()
    {
        var uri = $"https://{databaseName}.firebaseio.com/{rootNode}.json?orderBy=\"score\"&limitToLast=3";
        
        RestClient.Get(uri)
            .Then(
                response =>
                {
                    Debug.Log(response.Text);
                    _scoreEntries.Clear();
                    Dictionary<string, object> rawScoreEntries = (Dictionary<string, object>)Json.Deserialize(response.Text);
                    foreach (var kv in rawScoreEntries)
                    {
                        Dictionary<string, object> entry = (Dictionary<string, object>)kv.Value;
                        Debug.Log(entry);

                        ScoreEntry formattedEntry = new ScoreEntry();
                        formattedEntry.name = entry["name"].ToString(); 
                        Int32.TryParse(entry["score"].ToString(), out formattedEntry.score);
                        _scoreEntries.Add(formattedEntry);
                    }
                    testText.text = _scoreEntries[0].name;
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                })
            .Done();
    }

    private void AddScore(string name, int score)
    {
        var pushID = FirebasePushIDGenerator.GeneratePushID();
        var uri = $"https://{databaseName}.firebaseio.com/{rootNode}/{pushID}.json";

        var jsonToStore = $"{{\"score\":\"{score}\", \"name\":\"{name}\"}}";
        
        RestClient.Put<Dictionary<string, object>>(uri, jsonToStore)
            .Then(
                response =>
                {
                    
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                })
            .Done();
    }
}
