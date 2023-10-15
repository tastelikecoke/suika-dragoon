using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryMenu : MonoBehaviour
{
    [SerializeField]
    private FruitManager _fruitManager;
    public void PressRetry()
    {
        _fruitManager.Retry();
        /// transition
        this.GetComponent<Canvas>().enabled = false;
    }

    public void Show()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = true;
        
    }
}
