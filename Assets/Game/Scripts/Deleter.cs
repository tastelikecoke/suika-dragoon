using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    public void OnPress()
    {
        PlayerPrefs.DeleteKey("dragoon_drop_save_file_for_local_ranking");
        PlayerPrefs.Save();
        
        if(GameSystem.Instance)
            GameSystem.Instance.localScores.Clear();
    }
}
