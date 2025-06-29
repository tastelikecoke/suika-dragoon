using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Script to keep track of the score for every scene. Also maintains the BGM instance
    /// </summary>
    public class GameSystem : MonoBehaviour
    {
        public List<int> localScores;
        
        public AudioMixerSnapshot unmuteSnapshot;
        public AudioMixerSnapshot muteSnapshot;
        public bool isMute = false;
        public Action OnMuteChanged;

        /// <summary> original name is from Dragoon Drop release </summary>
        public const string SaveFileForLocalRankingKey = "dragoon_drop_save_file_for_local_ranking";
        
        public static GameSystem Instance = null;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            string storedScores = PlayerPrefs.GetString(SaveFileForLocalRankingKey, null);
            localScores = new List<int>();
            if (!string.IsNullOrEmpty(storedScores))
            {
                var scores = storedScores.Split(" ");
                foreach(var score in scores)
                {
                    int newScore = 0;
                    Int32.TryParse(score, out newScore);
                    if (newScore != 0)
                    {
                        localScores.Add(newScore);
                    }
                }
            }
            DontDestroyOnLoad(this);
        }

        public void UploadLocalScore(int score)
        {
            localScores.Add(score);
            localScores.Sort();

            string storedScore = "";
            foreach(var scoreInt in localScores)
            {
                if (storedScore == "") storedScore = scoreInt.ToString();
                else storedScore = storedScore + " " + scoreInt.ToString();
            }

            PlayerPrefs.SetString(SaveFileForLocalRankingKey, storedScore);
            PlayerPrefs.Save();
        }

        public int GetLocalRank(int score)
        {
            var result = localScores.BinarySearch(score);
            if (result < 0) return localScores.Count + 1 - (~result);
            return localScores.Count - result + 1;
        }

    }

}