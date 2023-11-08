using System;
using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using RSG;
using UnityEngine;

namespace FirebaseREST
{
    public class FirebaseDatabaseHandler
    {
        private string projectId;
        private string idToken;
        private string idTokenKey => string.IsNullOrEmpty(idToken) ? null : $"?auth={idToken}";
        private string databaseUrl => $"https://{projectId}.firebaseio.com";
        private bool isDevelopment;

        private string rootNode => isDevelopment ? "staging" : "prod";

        public static readonly Dictionary<string, string> Timestamp = new Dictionary<string, string>
        {
            { ".sv", "timestamp" }
        };

        public FirebaseDatabaseHandler(string projectId, string idToken, bool isDevelopment)
        {
            this.projectId = projectId;
            this.idToken = idToken;
            this.isDevelopment = isDevelopment;
        }

        public void PutData<T>(string node, T data, Action<DatabaseResponse<T>> callback, bool addTimestamp = true)
        {
            // add timestamp
            // var uri = $"{databaseUrl}/{rootNode}/{node}.json{idTokenKey}";
            var uri = $"{databaseUrl}/{rootNode}/{node}.json";
            Debug.Log(uri);

            var json = Json.Serialize(data);
            if (addTimestamp)
                json = json.Substring(0, json.Length - 1) + ", \"timestamp\": {\".sv\": \"timestamp\"}}";

            Debug.Log($"{uri} -> {json}");

            RestClient.Put<T>(uri, json)
            .Then(
                response =>
                {
                    var dbr = new DatabaseResponse<T>();
                    dbr.response = response;
                    callback?.Invoke(dbr);
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                    var dbr = new DatabaseResponse<T>();
                    dbr.isError = true;
                    dbr.errorMessage = e.Response;
                    callback?.Invoke(dbr);
                })
            .Done();
        }

        public void PatchData<T>(string node, T data, Action<DatabaseResponse<T>> callback, bool addTimestamp = true)
        {
            // add timestamp
            // var uri = $"{databaseUrl}/{rootNode}/{node}.json{idTokenKey}";
            var uri = $"{databaseUrl}/{rootNode}/{node}.json";
            Debug.Log(uri);

            var json = Json.Serialize(data);
            if (addTimestamp)
                json = json.Substring(0, json.Length - 1) + ", \"timestamp\": {\".sv\": \"timestamp\"}}";

            Debug.Log($"{uri} -> {json}");

            RestClient.Patch<T>(uri, json)
            .Then(
                response =>
                {
                    var dbr = new DatabaseResponse<T>();
                    dbr.response = response;
                    callback?.Invoke(dbr);
                })
            .Catch(
                error =>
                {
                    var e = error as RequestException;
                    var dbr = new DatabaseResponse<T>();
                    dbr.isError = true;
                    dbr.errorMessage = e.Response;
                    callback?.Invoke(dbr);
                })
            .Done();
        }

        public void GetData<T>(string node, Action<DatabaseResponse<T>> callback)
        {
            var uri = $"{databaseUrl}/{rootNode}/{node}.json";
            
            Debug.Log($"{uri} -> ?");
            RestClient.Get<T>(uri)
                .Then(
                    response =>
                    {
                        var dbr = new DatabaseResponse<T>();
                        dbr.response = response;
                        callback?.Invoke(dbr);
                    })
                .Catch(
                    error =>
                    {
                        var e = error as RequestException;
                        var dbr = new DatabaseResponse<T>();
                        dbr.isError = true;
                        dbr.errorMessage = e.Response;
                        callback?.Invoke(dbr);
                    })
                .Done();
        }
        public struct DatabaseResponse<T>
        {
            public bool isError;
            public string errorMessage;
            public T response;
        }
    }
}
