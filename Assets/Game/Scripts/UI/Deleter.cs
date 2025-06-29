using UnityEngine;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Script for delete local save file button
    /// </summary>
    public class Deleter : MonoBehaviour
    {
        public void OnPress()
        {
            PlayerPrefs.DeleteKey(GameSystem.SaveFileForLocalRankingKey);
            PlayerPrefs.Save();

            if (GameSystem.Instance)
                GameSystem.Instance.localScores.Clear();
        }
    }
}