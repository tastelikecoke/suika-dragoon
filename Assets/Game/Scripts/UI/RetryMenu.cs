using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// For UI of Retry menu
    /// </summary>
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
        [SerializeField]
        private UploadScoreMenu uploadScoreMenu;
        [SerializeField]
        private AudioSource tada;
        
        private Canvas _canvas;
        private Selectable[] _selectables;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _selectables = GetComponentsInChildren<Selectable>();
        }
        
        /// <summary>
        /// Hide the retry menu
        /// </summary>
        public void PressRetry()
        {
            _canvas.enabled = false;
            foreach (var selectable in _selectables)
            {
                selectable.interactable = false;
            }

            _fruitManager.Retry();
        }
        public void PressReturnToTitle()
        {
            SceneManager.LoadScene("Launcher");
        }
        public void PressUploadScore()
        {
            uploadScoreMenu.Show();
        }

        /// <summary>
        /// Show the retry menu
        /// </summary>
        public void Show(bool isHighScore = false)
        {
            _canvas.enabled = true;
            foreach (var selectable in _selectables)
            {
                selectable.interactable = true;
            }

            screenshot.texture = _fruitManager.screenshot;
            highScoreText.SetActive(isHighScore);
            normalText.SetActive(!isHighScore);
            if (isHighScore)
                tada.Play();
        }
        public void Update()
        {
            if (uploadScoreMenu.canvas.enabled)
                return;

            if (_canvas.enabled)
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
}