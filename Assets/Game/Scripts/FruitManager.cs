using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Handles all the fruits.  Fruits are items that drop on the gameplay space
    /// Since this game is based on Suika Game.
    /// </summary>
    public class FruitManager : MonoBehaviour
    {
        [Header("Fruit Settings")]
        public GameObject[] fruitList;
        [SerializeField]
        private Transform fruitRoot;
        [SerializeField]
        public FruitPool fruitPool;
        [SerializeField]
        public RetryMenu retryMenu;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private TMP_Text buildNumberText;

        #region Special Fruit Variables
        
        [Header("Rat")] [Tooltip("Chance of spawning a fruit that doesn't fuse unless it's a rat")]
        [SerializeField]
        private float ratChance = 5f;
        [SerializeField]
        private GameObject ratFruit;
        [SerializeField]
        private AudioSource ratAudioSource;

        [Header("Pomu")] [Tooltip("Chance of spawning a fruit that has an abnormal collider")]
        [SerializeField]
        private float pomuChance = 5f;
        [SerializeField]
        private GameObject pomuFruit;
        [SerializeField]
        private AudioSource pomuAudioSource;

        [Header("Explosive Grenade")] [Tooltip("Chance of spawning a fruit that explodes")]
        [SerializeField]
        private float explosionChance = 5f;
        [SerializeField]
        private GameObject explosionFruit;
        [SerializeField]
        private AudioSource explosionSource;
        [SerializeField]
        private GameObject explosion;
        [SerializeField]
        private float explosionPower = 1000f;

        [Header("Rosebuds")] [Tooltip("Chance of spawning a heavier fruit")]
        [SerializeField]
        private float rosebudChance = 5f;
        [SerializeField]
        private GameObject rosebudFruit;
        [SerializeField]
        private AudioSource rosebudAudioSource;

        [Header("Pentomos")] [Tooltip("Chance of spawning a spinning fruit")]
        [SerializeField]
        private AudioSource pentomoAudioSource;
        [SerializeField]
        private GameObject pentomoFruit;
        [SerializeField]
        private float pentomoChance = 5f;
        
        #endregion

        [Header("I did it")] [Tooltip("Sound when final fruit is created")]
        [SerializeField]
        private AudioSource ididitAudioSource;

        /* fruit instances active */
        private GameObject nextFruit;
        private GameObject nextNextFruit;

        /* mostly accessed by other scripts */
        public int totalScore = 0;
        public bool isFailed = false;
        public bool dontFallFirst = false;
        public bool isUploadedAlready = false;
        public Texture2D screenshot = null;
        public Action<int> OnScoreChanged;
        
        private void Start()
        {
            buildNumberText.text = Application.version;
            Retry();
            AssignNextFruit();
        }

        public void Retry()
        {
            totalScore = 0;
            OnScoreChanged?.Invoke(totalScore);

            isUploadedAlready = false;
            dontFallFirst = true;
            isFailed = false;

            fruitPool.ResetAll();

            StartCoroutine(WaitTilStartCR());
        }

        public IEnumerator WaitTilStartCR()
        {
            yield return new WaitForSeconds(0.5f);
            dontFallFirst = false;
        }

        public void AssignNextFruit()
        {
            int maxFruit = Math.Min(fruitList.Length, 5);
            nextFruit = nextNextFruit;

            nextNextFruit = null;
            if (nextFruit == null)
            {
                nextFruit = fruitList[Random.Range(0, maxFruit)];
                var nextFruitScript = nextFruit.GetComponent<Fruit>();
                
                if (nextFruitScript.level == 1)
                {
                    if (ratChance > Random.Range(0f, 100f))
                    {
                        nextFruit = ratFruit;
                    }
                }
                else if (nextFruitScript.level == 3)
                {
                    if (explosionChance > Random.Range(0f, 100f))
                    {
                        nextFruit = explosionFruit;
                    }
                }

            }

            nextNextFruit = fruitList[Random.Range(0, maxFruit)];
            var nextNextFruitScript = nextNextFruit.GetComponent<Fruit>();
            
            if (nextNextFruitScript.level == 1)
            {
                if (ratChance > Random.Range(0f, 100f))
                {
                    nextNextFruit = ratFruit;
                }
            }
            else if (nextNextFruitScript.level == 3)
            {
                if (explosionChance > Random.Range(0f, 100f))
                {
                    nextNextFruit = explosionFruit;
                }
            }
        }

        public void CheckRatEquipped()
        {
            if (nextFruit == ratFruit)
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
            for (int i = 0; i < numColliders; i++)
            {
                var newCollider = colliders[i];
                if (newCollider == null) continue;
                Vector3 forceAngle = (newCollider.transform.position - newExplosion.transform.position).normalized;
                if (newCollider.attachedRigidbody != null)
                    newCollider.attachedRigidbody.AddForce(forceAngle * explosionPower);
            }

            explosionSource.Play();
            yield return new WaitForSeconds(1f);
            Destroy(newExplosion);
        }

        public IEnumerator GenerateFruitCR(Fruit fruit1, Fruit fruit2)
        {
            if (isFailed) yield break;
            if (fruit1.level != fruit2.level) yield break;
            if (fruitList.Length <= fruit1.level) yield break;

            if (!audioSource.isPlaying)
                audioSource.Play();

            StartCoroutine(fruit1.Pop());
            StartCoroutine(fruit2.Pop());

            //yield return new WaitForSeconds(3f);
            yield return new WaitForSeconds(1f / 12f);

            totalScore += fruit1.level * (fruit1.level + 1) / 2;
            OnScoreChanged.Invoke(totalScore);
            
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

            if (fruit1.level + 1 == 8)
            {
                // 5% rosebud Chance
                if (pentomoChance > Random.Range(0f, 100f))
                {
                    spawningFruit = pentomoFruit;
                    pentomoAudioSource.Play();
                }
            }

            if (fruit1.level + 1 == 11)
            {
                ididitAudioSource.Play();
            }

            var newFruit = fruitPool.GetObject(spawningFruit, fruitRoot);
            newFruit.transform.position = Vector3.Lerp(fruit1.transform.position, fruit2.transform.position, 0.5f);
            newFruit.transform.rotation = Quaternion.Lerp(fruit1.transform.rotation, fruit2.transform.rotation, 0.5f);

            newFruit.GetComponent<Fruit>().manager = this;
        }

        public void Fail()
        {
            StartCoroutine(FailCR());
        }
        public IEnumerator FailCR()
        {
            if (isFailed)
                yield break;
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
}