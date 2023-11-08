using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UploadScoreMenu : MonoBehaviour
{
    [SerializeField]
    private FruitManager _fruitManager;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TMP_Text waitText;
    private FruitManager manager;
    
    public void PressUpload()
    {
        waitText.text = "Syncing...";
        if (manager == null)
            manager = FindObjectOfType<FruitManager>();

        if (manager != null)
        {
            var value = inputField.text;
            FirebaseRestSystem.Instance.AddScore(value, manager.totalScore, dbr =>
            {
                waitText.text = "Done!";
            });
        }
    }
    public void PressSkip()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = false;
        foreach (var selectable in GetComponentsInChildren<Selectable>())
        {
            selectable.interactable = false;
        }
    }
    public void Show()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = true;
        foreach (var selectable in GetComponentsInChildren<Selectable>())
        {
            selectable.interactable = true;
        }

        waitText.text = "";
    }
}
