using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// UI for upload score option
    /// </summary>
    public class UploadScoreMenu : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private TMP_Text waitText;
        private FruitManager manager;
        [SerializeField, TextArea]
        private string badWords;
        [SerializeField]
        internal Canvas canvas;

        /// <summary>
        /// Detect bad words on the score upload and prevent them
        /// </summary>
        public bool IsBadWord(string word)
        {
            if (word.Length < 2) return true;

            var rawRegex = badWords.Split("\n");
            for (int i = 0; i < rawRegex.Length; i++)
            {
                var pattern = new Regex(rawRegex[i], RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                if (pattern.IsMatch(word))
                {
                    return true;
                }
            }

            return false;
        }

        public void PressUpload()
        {
            waitText.text = "Syncing...";
            if (manager == null)
                manager = FindObjectOfType<FruitManager>();

            if (manager != null)
            {
                var value = inputField.text;

                if (IsBadWord(value))
                {
                    waitText.text = "Invalid name.";
                    return;
                }

                if (manager.isUploadedAlready)
                {
                    waitText.text = "Already uploaded.";
                    return;
                }

                FirebaseRestSystem.Instance.AddScore(value, manager.totalScore, dbr =>
                {
                    manager.isUploadedAlready = true;
                    waitText.text = "Done!";
                });
            }
        }
        
        /// <summary>
        /// transition back to Retry menu
        /// </summary>
        public void PressSkip()
        {
            canvas.enabled = false;
            foreach (var selectable in GetComponentsInChildren<Selectable>())
            {
                selectable.interactable = false;
            }
        }
        
        /// <summary>
        /// transition to Upload score menu
        /// </summary>
        public void Show()
        {
            canvas.enabled = true;
            foreach (var selectable in GetComponentsInChildren<Selectable>())
            {
                selectable.interactable = true;
            }

            waitText.text = "";
        }
    }
}