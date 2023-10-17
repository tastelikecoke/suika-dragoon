using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public void Show()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = true;
        foreach (var selectable in GetComponentsInChildren<Selectable>())
        {
            selectable.interactable = true;
        }
    }
    public void Hide()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = false;
        foreach (var selectable in GetComponentsInChildren<Selectable>())
        {
            selectable.interactable = false;
        }
    }
    public void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            if (Input.GetButtonDown("Submit"))
            {
                StartCoroutine(HideCR());
            }
            
            if (Input.GetButtonDown("Cancel"))
            {
                StartCoroutine(HideCR());
            }
        }
        
    }
    public IEnumerator ShowCR()
    {
        yield return null;
        Show();
    }
    public IEnumerator HideCR()
    {
        yield return null;
        Hide();
    }
    
}
