using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryMenu : MonoBehaviour
{
    [SerializeField]
    private FruitManager _fruitManager;
    [SerializeField]
    private RawImage screenshot;
    [SerializeField]
    private GameObject highScoreText;
    [SerializeField]
    private GameObject normalText;
    public void PressRetry()
    {
        _fruitManager.Retry();
        /// transition
        this.GetComponent<Canvas>().enabled = false;
    }
    public void PressReturnToTitle()
    {
        SceneManager.LoadScene("Launcher");
    }

    public void Show(bool isHighScore = false)
    {
        /// transition
        this.GetComponent<Canvas>().enabled = true;
        screenshot.texture = _fruitManager.screenshot;
        highScoreText.SetActive(isHighScore);
        normalText.SetActive(!isHighScore);
    }
    public void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            if (Input.GetButtonDown("Submit"))
            {
                PressRetry();
            }
            
            if (Input.GetButtonDown("Cancel"))
            {
                PressReturnToTitle();
            }
        }
        
    }
}
