using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseREST
{
    public interface IUser
    {
        string userId { get; }
        string token { get; }
    }

}
