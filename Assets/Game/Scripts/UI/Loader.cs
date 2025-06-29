using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace tastelikecoke.PanMachine
{
    public class Loader : MonoBehaviour
    {

        public void Load()
        {
            StartCoroutine(LoadCR());
        }
        
        public IEnumerator LoadCR()
        {
            GetComponent<Canvas>().enabled = true;
            DontDestroyOnLoad(gameObject);
            
            AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
            yield return new WaitForSeconds(0.5f);
            
            if (!operation.isDone)
            {
                var animator = this.GetComponent<Animator>();
                animator.SetTrigger("Start");
                yield return new WaitUntil(() => operation.isDone);
                animator.SetTrigger("End");
                yield return new WaitForSeconds(0.5f);
            }
            
            Destroy(gameObject);
        }
    }
}
