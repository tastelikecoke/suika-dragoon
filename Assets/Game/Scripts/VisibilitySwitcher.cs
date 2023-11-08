using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySwitcher : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup localGroup;
    [SerializeField]
    private CanvasGroup globalGroup;

    public void Switch()
    {
        if (localGroup.alpha < 0.01f)
        {
            localGroup.alpha = 1.0f;
            globalGroup.alpha = 0.0f;
        }
        else
        {
            localGroup.alpha = 0.0f;
            globalGroup.alpha = 1.0f;
            
        }
    }
}
