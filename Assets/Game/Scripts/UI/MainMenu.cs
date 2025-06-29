using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// For UI of Main Scene
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Loader loader;

        /// <summary>
        /// Show the main game scene next
        /// </summary>
        public void StartGame()
        {
            loader.Load();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Submit"))
            {
                StartGame();
            }
        }
    }
}