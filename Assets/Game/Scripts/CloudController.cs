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
    private bool isDebugOn = false;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private bool isPointerHovering = false;
    [SerializeField]
    private bool isPointerClicked = false;
    [SerializeField]
    private PauseMenu pauseMenu;

    private GameObject equippedFruit = null;
    private GameObject equippedNextNextFruit = null;

    public void SetPointerHover(bool value)
    {
        isPointerHovering = value;
    }
    public void SetPointerClick(bool value)
    {
        isPointerClicked = value;
    }
    private void EquipNextFruit()
    {
        var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitContainer);
        newFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        newFruit.GetComponent<CircleCollider2D>().enabled = false;
        newFruit.transform.rotation = Random.value > 0.5f ? Quaternion.Euler(-tilt) : Quaternion.Euler(tilt);
        equippedFruit = newFruit;
        fruitManager.CheckRatEquipped();
        
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

        if (isPointerHovering)
        {
            UpdateMouse();
        }
    }

    public void UpdateMouse()
    {
        var rb = GetComponent<Rigidbody2D>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
    }
    private void Update()
    {
        // do not execute if on retry.
        if (fruitManager.isFailed) return;
    
        if (equippedFruit == null || equippedFruit.GetComponent<Fruit>().isTouched)
        {
            EquipNextFruit();
        }

        if (Input.GetButtonDown("Fire2") && GameSystem.Instance != null)
        {
            GameSystem.Instance.bgm.mute = !GameSystem.Instance.bgm.mute;
        }
        
        if (Input.GetButtonDown("Cancel") && !pauseMenu.GetComponent<Canvas>().enabled && !fruitManager.retryMenu.GetComponent<Canvas>().enabled)
        {
            StartCoroutine(pauseMenu.ShowCR());
        }

        var fireInput = isPointerClicked || Input.GetButtonDown("Submit");
        if (fireInput && fruitContainer.childCount > 0)
        {
            var equippedRotation = equippedFruit.transform.rotation;
            Destroy(equippedFruit);
            
            var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitRoot);
            /* add jitter */
            newFruit.transform.position = constrainedFruit.position + (Vector3)(Random.insideUnitCircle * 0.01f);
            newFruit.transform.rotation = equippedRotation;
            newFruit.GetComponent<Fruit>().manager = fruitManager;
            
            if(!audioSource.isPlaying)
                audioSource.Play();
            
            //follow velocity. Just don't lol. funny though
            //newFruit.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            equippedFruit = newFruit;
            // set this to allow spam
#if UNITY_EDITOR
            if(isDebugOn)
                equippedFruit.GetComponent<Fruit>().isTouched = true;
#endif
            fruitManager.AssignNextFruit();
        }

        isPointerClicked = false;
    }
}
