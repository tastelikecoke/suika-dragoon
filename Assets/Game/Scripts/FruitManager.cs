using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private AudioSource ratAudioSource;
    [SerializeField]
    private GameObject ratFruit;
    [SerializeField]
    private float ratChance = 5f;
    [SerializeField]
    private AudioSource pomuAudioSource;
    [SerializeField]
    private GameObject pomuFruit;
    [SerializeField]
    private float pomuChance = 5f;
    [SerializeField]
    private TMP_Text buildNumberText;
    [SerializeField, Header("Grenade")]
    private GameObject explosionFruit;
    [SerializeField]
    private float explosionChance = 5f;
    [SerializeField]
    private AudioSource explosionSource;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float explosionPower = 1000f;
    [SerializeField, Header("Rosebuds")]
    private AudioSource rosebudAudioSource;
    [SerializeField]
    private GameObject rosebudFruit;
    [SerializeField]
    private float rosebudChance = 5f;

    public int totalScore = 0;
    
    private GameObject nextFruit;
    private GameObject nextNextFruit;
    
    public bool isFailed = false;
    public bool isUploadedAlready = false;
    public Texture2D screenshot = null;


    private void Start()
    {
        buildNumberText.text = Application.version;
        Retry();
    }
    
    public void Retry()
    {
        totalScore = 0;
        isFailed = false;
        isUploadedAlready = false;
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
            if (nextFruit.GetComponent<Fruit>().level == 1)
            {
                // 5% rat Chance
                if (ratChance > Random.Range(0f, 100f))
                {
                    nextFruit = ratFruit;
                }
            }
            else if (nextFruit.GetComponent<Fruit>().level == 3)
            {
                // 5% grenade Chance
                if (explosionChance > Random.Range(0f, 100f))
                {
                    nextFruit = explosionFruit;
                }
            }
            
        }
        nextNextFruit = fruitList[Random.Range(0, maxFruit)];
        if (nextNextFruit.GetComponent<Fruit>().level == 1)
        {
            // 5% rat Chance
            if (ratChance > Random.Range(0f, 100f))
            {
                nextNextFruit = ratFruit;
            }
        }
        else if (nextNextFruit.GetComponent<Fruit>().level == 3)
        {
            // 5% grenade Chance
            if (explosionChance > Random.Range(0f, 100f))
            {
                nextNextFruit = explosionFruit;
            }
        }
    }

    public void CheckRatEquipped()
    {
        if(nextFruit == ratFruit)
            ratAudioSource.Play();
    }
    
    public GameObject GetNextFruit()
    {
        return nextFruit;
    }
    public GameObject GetNextNextFruit()
    {
        return nextNextFruit;
    }
    public void GenerateExplosion(Fruit fruit)
    {
        StartCoroutine(GenerateExplosionCR(fruit));
    }
    public IEnumerator GenerateExplosionCR(Fruit fruit)
    {
        var newExplosion = Instantiate(explosion);
        newExplosion.transform.position = fruit.transform.position;

        yield return null;
        Collider2D[] colliders = new Collider2D[20];
        int numColliders = Physics2D.OverlapCircleNonAlloc(newExplosion.transform.position, 100.0f, colliders);
        for(int i = 0; i < numColliders; i++)
        {
            var newCollider = colliders[i];
            if (newCollider == null) continue;
            Vector3 forceAngle = (newCollider.transform.position - newExplosion.transform.position).normalized;
            if(newCollider.attachedRigidbody != null)
                newCollider.attachedRigidbody.AddForce(forceAngle * explosionPower);
        }

        explosionSource.Play();
        yield return new WaitForSeconds(1f);
        Destroy(newExplosion);
    }

    public IEnumerator GenerateFruitCR(Fruit fruit1, Fruit fruit2)
    {
        if (fruit1.level != fruit2.level) yield break;
        if (fruitList.Length <= fruit1.level) yield break;
        
        if(!audioSource.isPlaying)
            audioSource.Play();
        
        StartCoroutine(fruit1.Pop());
        StartCoroutine(fruit2.Pop());
        
        //yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f/12f);
        
        totalScore += fruit1.level * (fruit1.level + 1) / 2;
        var spawningFruit = fruitList[fruit1.level];
        if (fruit1.level + 1 == 6)
        {
            // 5% pomu Chance
            if (pomuChance > Random.Range(0f, 100f))
            {
                spawningFruit = pomuFruit;
                pomuAudioSource.Play();
            }
        }
        if (fruit1.level + 1 == 7)
        {
            // 5% rosebud Chance
            if (rosebudChance > Random.Range(0f, 100f))
            {
                spawningFruit = rosebudFruit;
                rosebudAudioSource.Play();
            }
        }
        var newFruit = Instantiate(spawningFruit, fruitRoot);
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
            childFruit.Fail();
        }
        yield return new WaitForSeconds(3f);
        
        retryMenu.Show(isHighScore);
    }
}
