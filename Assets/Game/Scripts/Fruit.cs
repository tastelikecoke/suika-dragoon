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
    public bool isExplosive = false;

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
            {
                if (!isTouched && isExplosive)
                    StartCoroutine(Explode());
                isTouched = true;
            }
            return;
        }
        else
        {
            if (!isTouched && isExplosive)
                StartCoroutine(Explode());
            isTouched = true;
        }
        
        if (contactFruit.level == level)
        {
            if (manager)
            {
                /* rat to rat fusion only. I am genius */
                if (isRat != contactFruit.isRat) return;
                /* I need to stop fusion with already fusing */
                if (contactFruit.isPopping) return;
                
                this.isPopping = true;
                contactFruit.isPopping = true;
                StartCoroutine(manager.GenerateFruitCR(this, contactFruit));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!manager)
            return;
        if (col.gameObject.name == "Death")
            manager.Fail();
    }
    
    public void Fail()
    {
        if(gameObject.GetComponent<CircleCollider2D>() != null)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if(gameObject.GetComponent<PolygonCollider2D>() != null)
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        var animator = GetComponent<Animator>();
        if (animator)
        {
            
            animator.enabled = true;
            animator.SetTrigger("Shake");
        }
    }


    public IEnumerator Pop()
    {
        if(gameObject.GetComponent<CircleCollider2D>() != null)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if(gameObject.GetComponent<PolygonCollider2D>() != null)
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        var animator = GetComponent<Animator>();
        if (animator)
        {
            
            animator.enabled = true;
            animator.SetTrigger("Pop");
        }
        
        //yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f/12f);
        
        Destroy(gameObject);
    }

    public IEnumerator Explode()
    {
        var animator = GetComponent<Animator>();
        if (animator)
        {
            animator.enabled = true;
            animator.SetTrigger("Ohno");
        }
        yield return new WaitForSeconds(2f);
        manager.GenerateExplosion(this);
        yield return Pop();
    }
}
