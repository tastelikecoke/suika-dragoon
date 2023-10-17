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
    public RetryMenu retryMenu;
    [SerializeField]
    private AudioSource audioSource;

    public int totalScore = 0;
    
    private GameObject nextFruit;
    private GameObject nextNextFruit;
    
    public bool isFailed = false;
    public Texture2D screenshot = null;


    private void Start()
    {
        Retry();
    }
    
    public void Retry()
    {
        totalScore = 0;
        isFailed = false;
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
    
    public IEnumerator GenerateFruitCR(Fruit fruit1, Fruit fruit2)
    {
        if (fruit1.level != fruit2.level) yield break;
        if (fruitList.Length <= fruit1.level) yield break;
        
        if(!audioSource.isPlaying)
            audioSource.Play();
        
        StartCoroutine(fruit1.Pop());
        StartCoroutine(fruit2.Pop());
        
        yield return new WaitForSeconds(1f/12f);
        
        totalScore += fruit1.level * (fruit1.level + 1) / 2;
        var newFruit = Instantiate(fruitList[fruit1.level], fruitRoot);
        newFruit.transform.position = Vector3.Lerp(fruit1.transform.position, fruit2.transform.position, 0.5f);
        newFruit.GetComponent<Fruit>().manager = this;
    }

    public void Fail()
    {
        StartCoroutine(FailCR());
    }
    public IEnumerator FailCR()
    {
        isFailed = true;
        var isHighScore = GameSystem.Instance.GetLocalRank(totalScore) == 1;

        if (GameSystem.Instance != null)
        {
            GameSystem.Instance.UploadLocalScore(totalScore);
        }

        yield return new WaitForEndOfFrame();
        screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        screenshot.SetPixels(texture.GetPixels());
        screenshot.Apply();
        
        foreach (Transform child in fruitRoot.transform)
        {
            var childFruit = child.GetComponent<Fruit>();
            childFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        yield return new WaitForSeconds(1f);
        
        retryMenu.Show(isHighScore);
    }
}
