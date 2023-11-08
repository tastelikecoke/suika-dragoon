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
    
    public static FirebaseRestSystem Instance = null;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        ScoreEntries = new List<ScoreEntry>();
    }
    
    [SerializeField]
    private string databaseName;
    [SerializeField]
    private string rootNode;

    public List<ScoreEntry> ScoreEntries;
    private void Start()
    {
    }
    
    public struct DatabaseResponse<T>
    {
        public bool isError;
        public string errorMessage;
        public T response;
    }

    public void GetTopScores(Action<DatabaseResponse<string>> callback)
    {
        var uri = $"https://{databaseName}.firebaseio.com/{rootNode}.json?orderBy=\"score\"&limitToLast=3";
        
        RestClient.Get(uri)
            .Then(
                response =>
                {
                    ScoreEntries.Clear();
                    Dictionary<string, object> rawScoreEntries = (Dictionary<string, object>)Json.Deserialize(response.Text);
                    foreach (var kv in rawScoreEntries)
                    {
                        Dictionary<string, object> entry = (Dictionary<string, object>)kv.Value;
                        Debug.Log(entry);

                        ScoreEntry formattedEntry = new ScoreEntry();
                        formattedEntry.name = entry["name"].ToString(); 
                        Int32.TryParse(entry["score"].ToString(), out formattedEntry.score);
                        ScoreEntries.Add(formattedEntry);
                    }
                    ScoreEntries.Sort((a, b) => a.score - b.score);
                    var dbr = new DatabaseResponse<string>();
                    dbr.response = response.Text;
                    callback?.Invoke(dbr);
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                    var dbr = new DatabaseResponse<string>();
                    dbr.isError = true;
                    dbr.errorMessage = e.Response;
                    callback?.Invoke(dbr);
                })
            .Done();
    }

    public void AddScore(string name, int score, Action<DatabaseResponse<Dictionary<string, object>>> callback)
    {
        var pushID = FirebasePushIDGenerator.GeneratePushID();
        var uri = $"https://{databaseName}.firebaseio.com/{rootNode}/{pushID}.json";

        var jsonToStore = $"{{\"score\":{score}, \"name\":\"{name}\"}}";
        
        RestClient.Put<Dictionary<string, object>>(uri, jsonToStore)
            .Then(
                response =>
                {
                    var dbr = new DatabaseResponse<Dictionary<string, object>>();
                    dbr.response = response;
                    callback?.Invoke(dbr);
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                    var dbr = new DatabaseResponse<Dictionary<string, object>>();
                    dbr.isError = true;
                    dbr.errorMessage = e.Response;
                    callback?.Invoke(dbr);
                })
            .Done();
    }
}
