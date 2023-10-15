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
    private Transform fruitContainer;
    [SerializeField]
    private Transform constrainedFruit;
    [SerializeField]
    private Transform nextNextFruitRoot;
    [SerializeField]
    private FruitManager fruitManager;

    private GameObject equippedFruit = null;
    private GameObject equippedNextNextFruit = null;

    private void Start()
    {
    }

    private void EquipNextFruit()
    {
        var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitContainer);
        newFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        newFruit.GetComponent<CircleCollider2D>().enabled = false;
        equippedFruit = newFruit;
        
        Destroy(equippedNextNextFruit);
        
        equippedNextNextFruit = Instantiate(fruitManager.GetNextNextFruit(), nextNextFruitRoot);
        equippedNextNextFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        equippedNextNextFruit.GetComponent<CircleCollider2D>().enabled = false;
        
    }

    private void FixedUpdate()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, 0f);
    }
    private void Update()
    {
        if (equippedFruit == null || equippedFruit.GetComponent<Fruit>().isTouched)
        {
            EquipNextFruit();
        }

        var fireInput = Input.GetButtonDown("Fire1");
        if (fireInput && fruitContainer.childCount > 0)
        {
            Destroy(equippedFruit);
            
            var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitRoot);
            newFruit.transform.position = constrainedFruit.position;
            newFruit.GetComponent<Fruit>().manager = fruitManager;
            //follow velocity. Just don't lol. funny though
            //newFruit.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            equippedFruit = newFruit;
            //equippedFruit.GetComponent<Fruit>().isTouched = true;
            fruitManager.AssignNextFruit();
        }
    }
}
