using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
        
        // Loader is ugly and unnecessary
        
        /*
        StartCoroutine(StartGameCR());
    }
    public IEnumerator StartGameCR()
    {
        GetComponent<Canvas>().enabled = true;
        DontDestroyOnLoad(gameObject);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitUntil(() => operation.isDone);
        GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        */
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            StartGame();
        }
    }
}
