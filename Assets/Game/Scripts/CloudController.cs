using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 3f;
    [SerializeField]
    private GameObject fruitPrefab;
    [SerializeField]
    private Transform fruitRoot;
    [SerializeField]
    private Transform constrainedFruit;
    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = forceMultiplier * new Vector3(horizontalInput, 0f, 0f);

        var fireInput = Input.GetButtonDown("Submit");
        if (fireInput)
        {
            var newFruit = Instantiate(fruitPrefab, fruitRoot);
            newFruit.transform.position = constrainedFruit.position;
        }
    }
}
