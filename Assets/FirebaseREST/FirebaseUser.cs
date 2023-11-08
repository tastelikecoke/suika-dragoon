using System.Collections.Generic;
using UnityEngine;

namespace FirebaseREST
{
    public class FirebaseUser : ScriptableObject, IUser
    {
        public string userId => localId;
        public string token { get; set; }

        public string localId;
        public string email;
        public bool emailVerified;
        public string displayName;
        public List<Dictionary<string, string>> providerUserInfo;
        public string photoUrl;
        public string passwordHash;
        public double passwordUpdatedAt;
        public string validSince;
        public bool disabled;
        public string lastLoginAt;
        public string createdAt;
        public bool customAuth;
    }

}
