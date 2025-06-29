using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// For UI of Pause
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        private Canvas _canvas;
        private Selectable[] _selectables;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _selectables = GetComponentsInChildren<Selectable>();
        }
        
        /// <summary>
        /// Show the pause menu over the game
        /// </summary>
        public void Show()
        {
            _canvas.enabled = true;
            foreach (var selectable in _selectables)
            {
                selectable.interactable = true;
            }
        }
        
        /// <summary>
        /// Hide the pause menu, back to the game
        /// </summary>
        public void Hide()
        {
            _canvas.enabled = false;
            foreach (var selectable in _selectables)
            {
                selectable.interactable = false;
            }
        }
        public void Update()
        {
            if (_canvas.enabled)
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
}