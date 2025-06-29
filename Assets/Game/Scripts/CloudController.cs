using UnityEngine;
using Random = UnityEngine.Random;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Handles spawning the fruits. Also controls the tongs (aka "Cloud")
    /// Represents the cloud in the original Suika Game.
    /// </summary>
    public class CloudController : MonoBehaviour
    {
        [Header("Fruit Settings")]
        [SerializeField]
        private Transform fruitRoot;
        [SerializeField]
        private Transform fruitContainer;
        [SerializeField]
        private Transform constrainedFruit;
        [SerializeField]
        private Transform nextNextFruitRoot;
        [SerializeField]
        private FruitManager fruitManager;
        [SerializeField]
        private FruitPool fruitPool;

        [Header("Physics Settings")]
        [SerializeField]
        private Vector3 tilt;
        [SerializeField]
        private float forceMultiplier = 3f;
        [SerializeField]
        private bool isDebugOn = false;
        [SerializeField]
        private Rigidbody2D rigidbody2d = null;

        [Header("UI")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private PauseMenu pauseMenu;

        /* Controller values */
        private bool _isPointerHovering = false;
        private bool _isPointerClicked = false;
        private GameObject _equippedFruit = null;
        private GameObject _equippedNextNextFruit = null;

        public void SetPointerHover(bool value)
        {
            _isPointerHovering = value;
        }
        public void SetPointerClick(bool value)
        {
            _isPointerClicked = value;
        }
        private void EquipNextFruit()
        {
            if (_equippedNextNextFruit != null)
            {
                _equippedNextNextFruit.GetComponent<Fruit>().Hide();
                _equippedNextNextFruit = null;
            }
            
            var newFruit = fruitPool.GetObject(fruitManager.GetNextFruit(), fruitContainer);
            var newFruitScript = newFruit.GetComponent<Fruit>();
            newFruitScript.SetAsNonMoving();
            
            newFruit.transform.rotation = Random.value > 0.5f ? Quaternion.Euler(-tilt) : Quaternion.Euler(tilt);
            _equippedFruit = newFruit;
            fruitManager.CheckRatEquipped();

            _equippedNextNextFruit = fruitPool.GetObject(fruitManager.GetNextNextFruit(), nextNextFruitRoot);
            
            var equippedNextNextFruitScript = _equippedNextNextFruit.GetComponent<Fruit>();
            equippedNextNextFruitScript.SetAsNonMoving();
        }

        private void FixedUpdate()
        {
            // do not execute if on retry.
            if (fruitManager.isFailed) return;
            if (fruitManager.dontFallFirst) return;

            var horizontalInput = Input.GetAxis("Horizontal");
            rigidbody2d.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, 0f);

            if (_isPointerHovering)
            {
                UpdateMouse();
            }
        }

        public void UpdateMouse()
        {
            if (fruitManager.dontFallFirst) return;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
        }
        private void Update()
        {
            // do not execute if on retry.
            if (fruitManager.isFailed)
            {
                _isPointerClicked = false;
                return;
            }
            
            if (fruitManager.dontFallFirst) return;

            if (_equippedFruit == null || _equippedFruit.GetComponent<Fruit>().isTouched || _equippedFruit.GetComponent<Fruit>().isHidden)
            {
                EquipNextFruit();
            }

            if (Input.GetButtonDown("Fire2") && GameSystem.Instance != null)
            {
                GameSystem.Instance.isMute = !GameSystem.Instance.isMute;
                GameSystem.Instance.OnMuteChanged?.Invoke();
            }

            if (Input.GetButtonDown("Cancel") && !pauseMenu.GetComponent<Canvas>().enabled && !fruitManager.retryMenu.GetComponent<Canvas>().enabled)
            {
                StartCoroutine(pauseMenu.ShowCR());
            }

            var fireInput = _isPointerClicked || Input.GetButtonDown("Submit");
            if (fireInput && fruitContainer.childCount > 0)
            {
                var equippedRotation = _equippedFruit.transform.rotation;
                if (_equippedFruit != null)
                {
                    _equippedFruit.GetComponent<Fruit>().Hide();
                    _equippedFruit = null;
                }


                var newFruit = fruitPool.GetObject(fruitManager.GetNextFruit(), fruitRoot);
                /* add jitter */
                newFruit.transform.position = constrainedFruit.position + (Vector3)(Random.insideUnitCircle * 0.01f);
                newFruit.transform.rotation = equippedRotation;
                newFruit.GetComponent<Fruit>().manager = fruitManager;

                if (!audioSource.isPlaying)
                    audioSource.Play();

                //Follow velocity of cloud to the fruit would be funny. Disabling this for prod.
                //newFruit.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
                
                _equippedFruit = newFruit;
                
#if UNITY_EDITOR
                // set this to allow spam
                if (isDebugOn)
                    _equippedFruit.GetComponent<Fruit>().isTouched = true;
#endif
                fruitManager.AssignNextFruit();
            }

            _isPointerClicked = false;
        }
    }
}