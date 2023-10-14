using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 3f;
    [SerializeField]
    private Transform fruitRoot;
    [SerializeField]
    private Transform constrainedFruit;
    [SerializeField]
    private FruitManager fruitManager;
    
    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = forceMultiplier * new Vector3(horizontalInput, 0f, 0f);

        var fireInput = Input.GetButtonDown("Submit");
        if (fireInput)
        {
            var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitRoot);
            newFruit.transform.position = constrainedFruit.position;
            newFruit.GetComponent<Fruit>().manager = fruitManager;
            
            
        }
    }
}
