using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class FirebaseSystem : MonoBehaviour
{
    DatabaseReference reference;
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("testname").ValueChanged += HandleTestNameChange;
    }

    private void HandleTestNameChange(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;
        Debug.Log(snapshot.Value);
    }
}
