using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    public GameObject[] fruitList;
    [SerializeField]
    private Transform fruitRoot;

    public GameObject GetNextFruit()
    {
        int maxFruit = Math.Min(fruitList.Length, 5);
        var newFruitPrefab = fruitList[Random.Range(0, maxFruit)];
        return newFruitPrefab;
    }

    public void GenerateFruit(Fruit fruit1, Fruit fruit2)
    {
        if (fruit1.level != fruit2.level) return;
        if (fruitList.Length <= fruit1.level) return;
        var newFruit = Instantiate(fruitList[fruit1.level], fruitRoot);
        newFruit.transform.position = Vector3.Lerp(fruit1.transform.position, fruit2.transform.position, 0.5f);
        newFruit.GetComponent<Fruit>().manager = this;
        
        Destroy(fruit1.gameObject);
        Destroy(fruit2.gameObject);
    }
}
