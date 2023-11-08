using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseREST
{
    public interface IAuthProvider
    {
        void SignUp(string email, string password);
        void SignIn(string email, string password);
        void RefreshToken(string tokenId);
    }

}
