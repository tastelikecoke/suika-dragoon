using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int level = 0;
    public FruitManager manager;
    public bool isPopping = false;
    public bool isTouched = true;
    public bool isRat = false;

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        if (animator)
        {
            animator.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // do not execute if on retry.
        if (manager.isFailed) return;
        
        
        if (isPopping) return;
        var contactFruit = col.gameObject.GetComponent<Fruit>();
        if (contactFruit == null)
        {
            if (col.gameObject.name == "Floor")
                isTouched = true;
            return;
        }
        else
        {
            isTouched = true;
        }
        
        if (contactFruit.level == level)
        {
            if (manager)
            {
                /* rat to rat fusion only. I am genius */
                if (isRat != contactFruit.isRat) return;
                
                StartCoroutine(manager.GenerateFruitCR(this, contactFruit));
                this.isPopping = true;
                contactFruit.isPopping = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Death")
            manager.Fail();
    }
    public void Fail()
    {
        var animator = GetComponent<Animator>();
        if (animator)
        {
            
            animator.enabled = true;
            animator.SetTrigger("Shake");
        }
        if(gameObject.GetComponent<CircleCollider2D>() != null)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if(gameObject.GetComponent<PolygonCollider2D>() != null)
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }


    public IEnumerator Pop()
    {
        var animator = GetComponent<Animator>();
        if (animator)
        {
            
            animator.enabled = true;
            animator.SetTrigger("Pop");
        }
        if(gameObject.GetComponent<CircleCollider2D>() != null)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if(gameObject.GetComponent<PolygonCollider2D>() != null)
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
        yield return new WaitForSeconds(1f/12f);
        
        Destroy(gameObject);
    }
}
