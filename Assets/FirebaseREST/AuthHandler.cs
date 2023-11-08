using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseREST
{
    public class AuthHandler : MonoBehaviour
    {
        [SerializeField] private string apiKey;

        [SerializeField] private string email;
        [SerializeField] private string password;
        [SerializeField] private string refreshToken;

        private IAuthProvider auth;

        void Awake()
        {
            auth = new FirebaseRESTAuthProvider(apiKey);
        }

        [InspectorButton]
        private void SignUp()
        {
            auth.SignUp(email, password);
        }

        [InspectorButton]
        private void SignIn()
        {
            auth.SignIn(email, password);
        }

        [InspectorButton]
        private void RefeshToken()
        {
            auth.RefreshToken(refreshToken);
        }
    }
}
