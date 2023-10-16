using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField]
    private Vector3 tilt;
    [SerializeField]
    private RectTransform activeArea = null;

    private GameObject equippedFruit = null;
    private GameObject equippedNextNextFruit = null;


    private void EquipNextFruit()
    {
        var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitContainer);
        newFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        newFruit.GetComponent<CircleCollider2D>().enabled = false;
        newFruit.transform.rotation = Random.value > 0.5f ? Quaternion.Euler(-tilt) : Quaternion.Euler(tilt);
        equippedFruit = newFruit;
        
        Destroy(equippedNextNextFruit);
        
        equippedNextNextFruit = Instantiate(fruitManager.GetNextNextFruit(), nextNextFruitRoot);
        equippedNextNextFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        equippedNextNextFruit.GetComponent<CircleCollider2D>().enabled = false;
        
    }

    private void FixedUpdate()
    {
        // do not execute if on retry.
        if (fruitManager.isFailed) return;

        var rb = GetComponent<Rigidbody2D>();
        var horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, 0f);

        if (activeArea != null)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(activeArea, Input.mousePosition, Camera.main))
            {
                UpdateMouse();
            }
        }
    }

    public void UpdateMouse()
    {
        var rb = GetComponent<Rigidbody2D>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Math.Abs(transform.position.x - mousePosition.x) < 0.05f) return;
        if (transform.position.x > mousePosition.x)
        {
            rb.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(-1.0f, 0f, 0f);
        }
        
        if (transform.position.x < mousePosition.x)
        {
            rb.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(1.0f, 0f, 0f);
        }
    }
    private void Update()
    {
        // do not execute if on retry.
        if (fruitManager.isFailed) return;
    
        if (equippedFruit == null || equippedFruit.GetComponent<Fruit>().isTouched)
        {
            EquipNextFruit();
        }

        var fireInput = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit");;
        if (fireInput && fruitContainer.childCount > 0)
        {
            var equippedRotation = equippedFruit.transform.rotation;
            Destroy(equippedFruit);
            
            var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitRoot);
            newFruit.transform.position = constrainedFruit.position;
            newFruit.transform.rotation = equippedRotation;
            newFruit.GetComponent<Fruit>().manager = fruitManager;
            //follow velocity. Just don't lol. funny though
            //newFruit.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            equippedFruit = newFruit;
            equippedFruit.GetComponent<Fruit>().isTouched = true;
            fruitManager.AssignNextFruit();
        }
    }
}
