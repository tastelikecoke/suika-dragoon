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
    [SerializeField]
    private RetryMenu retryMenu;

    public int totalScore = 0;
    
    private GameObject nextFruit;
    private GameObject nextNextFruit;


    private void Start()
    {
        Retry();
    }
    
    public void Retry()
    {
        totalScore = 0;
        AssignNextFruit();

        if (fruitRoot.childCount > 0)
        {
            foreach (Transform child in fruitRoot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    public void AssignNextFruit()
    {
        int maxFruit = Math.Min(fruitList.Length, 5);
        nextFruit = nextNextFruit;
        nextNextFruit = null;
        if (nextFruit == null)
        {
            nextFruit = fruitList[Random.Range(0, maxFruit)];
        }
        nextNextFruit = fruitList[Random.Range(0, maxFruit)];
    }
    public GameObject GetNextFruit()
    {
        return nextFruit;
    }
    public GameObject GetNextNextFruit()
    {
        return nextNextFruit;
    }
    
    public void GenerateFruit(Fruit fruit1, Fruit fruit2)
    {
        if (fruit1.level != fruit2.level) return;
        if (fruitList.Length <= fruit1.level) return;
        totalScore += fruit1.level * (fruit1.level + 1) / 2;
        var newFruit = Instantiate(fruitList[fruit1.level], fruitRoot);
        newFruit.transform.position = Vector3.Lerp(fruit1.transform.position, fruit2.transform.position, 0.5f);
        newFruit.GetComponent<Fruit>().manager = this;
        
        Destroy(fruit1.gameObject);
        Destroy(fruit2.gameObject);
    }

    public void Fail()
    {
        StartCoroutine(FailCR());
    }
    public IEnumerator FailCR()
    {
        
        foreach (Transform child in fruitRoot.transform)
        {
            var childFruit = child.GetComponent<Fruit>();
            childFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        yield return new WaitForSeconds(1f);
        retryMenu.Show();
    }
}
