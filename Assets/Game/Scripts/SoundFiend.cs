using UnityEngine;
using UnityEngine.UI;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Handles BGM enabler disabler.
    /// </summary>
    public class SoundFiend : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;
        private void Start()
        {
            if (GameSystem.Instance)
            {
                toggle.SetIsOnWithoutNotify(GameSystem.Instance.isMute);
                GameSystem.Instance.OnMuteChanged += OnMuteChanged;
            }
        }

        public void OnMuteChanged()
        {
            if (GameSystem.Instance)
            {
                toggle.SetIsOnWithoutNotify(GameSystem.Instance.isMute);
                if(GameSystem.Instance.isMute)
                    GameSystem.Instance.muteSnapshot.TransitionTo(0.5f);
                else
                    GameSystem.Instance.unmuteSnapshot.TransitionTo(0.5f);
            }
        }

        public void Flick()
        {
            if (GameSystem.Instance)
            {
                GameSystem.Instance.isMute = !GameSystem.Instance.isMute;
                if(GameSystem.Instance.isMute)
                    GameSystem.Instance.muteSnapshot.TransitionTo(0.5f);
                else
                    GameSystem.Instance.unmuteSnapshot.TransitionTo(0.5f);
            }
        }
    }
}