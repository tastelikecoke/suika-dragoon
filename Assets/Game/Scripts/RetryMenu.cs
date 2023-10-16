using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryMenu : MonoBehaviour
{
    [SerializeField]
    private FruitManager _fruitManager;
    [SerializeField]
    private RawImage screenshot;
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
        screenshot.texture = _fruitManager.screenshot;
    }
}
