using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseREST
{
    [CreateAssetMenu]
    public class FirebaseConfig : ScriptableObject
    {
        public string firebaseDbName;
        public string apiKey;
        public bool development;
    }

}
