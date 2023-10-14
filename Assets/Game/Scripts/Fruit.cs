using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int level = 0;
    public FruitManager manager;
    public bool isPopping = false;
    public bool isTouched = false;
    private void OnCollisionEnter2D(Collision2D col)
    {
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
                manager.GenerateFruit(this, contactFruit);
                this.isPopping = true;
                contactFruit.isPopping = true;
            }
        }
    }
}
