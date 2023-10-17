using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Show()
    {
        /// transition
        this.GetComponent<Canvas>().enabled = true;
    }
}
