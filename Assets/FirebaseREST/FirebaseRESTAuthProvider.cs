using System;
using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using RSG;
using UnityEngine;

namespace FirebaseREST
{
    public class FirebaseRESTAuthProvider : IAuthProvider
    {
        private const string API_ENDPOINT = "https://identitytoolkit.googleapis.com/v1";

        private string apiKey;

        public event Action<AuthResponse> onSignUpComplete;
        public event Action<AuthResponse> onSignInComplete;
        public event Action<ErrorResponse> onError;
        public event Action<RefreshTokenResponse> onRefreshToken;

        public FirebaseRESTAuthProvider(string apiKey)
        {
            this.apiKey = apiKey;
            Promise.UnhandledException += PromiseUnhandledException;
        }

        public void SignUp(string email, string password)
        {
            var payload = $"{{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}}";
            RestClient.Post<AuthResponse>($"{API_ENDPOINT}/accounts:signUp?key={apiKey}", payload)
                .Then(response =>
                {
                    DebugUtility.PrintFields(response);
                    onSignUpComplete?.Invoke(response);
                })
                .Catch(error =>
                {
                    var e = error as RequestException;
                    var errorResponse = new ErrorResponse((error as RequestException));
                    DebugUtility.PrintFields(errorResponse);
                    onError?.Invoke(errorResponse);
                })
                .Done();
        }

        public void SignIn(string email, string password)
        {
            var payload = $"{{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}}";
            RestClient.Post<AuthResponse>($"{API_ENDPOINT}/accounts:signInWithPassword?key={apiKey}", payload)
                .Then(response =>
                {
                    DebugUtility.PrintFields(response);
                    onSignInComplete?.Invoke(response);
                })
                .Catch(error =>
                {
                    var e = error as RequestException;
                    var errorResponse = new ErrorResponse((error as RequestException));
                    DebugUtility.PrintFields(errorResponse);
                    onError?.Invoke(errorResponse);
                })
                .Done();
        }

        public void SignInAnonymously()
        {
            var payload = $"{{\"returnSecureToken\":true}}";
            RestClient.Post<AuthResponse>($"{API_ENDPOINT}/accounts:signUp?key={apiKey}", payload)
                .Then(response =>
                {
                    DebugUtility.PrintFields(response);
                    onSignInComplete?.Invoke(response);
                })
                .Catch(error =>
                {
                    var errorResponse = new ErrorResponse((error as RequestException));
                    DebugUtility.PrintFields(errorResponse);
                    onError?.Invoke(errorResponse);
                })
                .Done();
        }

        public void RefreshToken(string tokenId)
        {
            var payload = $"{{\"grant_type\":\"refresh_token\",\"refresh_token\":\"{tokenId}\"}}";
            RestClient.Post<RefreshTokenResponse>($"{API_ENDPOINT}/token?key={apiKey}", payload)
                .Then(response =>
                {
                    DebugUtility.PrintFields(response);
                    onRefreshToken?.Invoke(response);
                })
                .Catch(error =>
                {
                    var e = error as RequestException;
                    var errorResponse = new ErrorResponse((error as RequestException));
                    DebugUtility.PrintFields(errorResponse);
                    onError?.Invoke(errorResponse);
                })
                .Done();
        }

        private void PromiseUnhandledException(object sender, ExceptionEventArgs e)
        {
            Debug.LogException(e.Exception);
        }

        public struct AuthResponse
        {
            public string idToken;
            public string email;
            public string refreshToken;
            public string expiresIn;
            public string localId;
            public bool registered;
        }

        public struct RefreshTokenResponse
        {
            public string expires_in;
            public string token_type;
            public string refresh_token;
            public string id_token;
            public string user_id;
            public string project_id;
        }
    }
}

