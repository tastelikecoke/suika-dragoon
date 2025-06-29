using System;
using System.Collections;
using UnityEngine;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Script to manage fruit physics and specific fruit behavior
    /// </summary>
    public class Fruit : MonoBehaviour
    {
        public FruitManager manager;

        [Header("Fruit Settings")]
        public string fruitID = "";
        public int level = 0;
        public bool isPopping = false;
        public bool isTouched = true;
        public bool isRat = false;
        public bool isExplosive = false;
        public bool isRosebud = false;
        
        [NonSerialized]
        public bool isHidden = false;
        [NonSerialized]
        public FruitPool pool;

        
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Collider2D[] _colliders;

        private Quaternion _initialRotation;
        private bool[] _initialColliderStates;
        private RigidbodyType2D _bodyType;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator)
            {
                _animator.enabled = false;
            }

            _rigidbody = GetComponent<Rigidbody2D>();
            
            _colliders = new Collider2D[3];
            _colliders[0] = GetComponent<CircleCollider2D>();
            _colliders[1] = GetComponent<PolygonCollider2D>();
            _colliders[2] = GetComponent<CapsuleCollider2D>();

            _initialRotation = transform.rotation;
            _initialColliderStates = new bool[3];
            for (int i = 0; i < 3; i++)
            {
                if (_colliders[i] != null)
                    _initialColliderStates[i] = _colliders[i].enabled;
            }

            _bodyType = _rigidbody.bodyType;
            isHidden = false;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnCollide(col.gameObject);
        }
        private void OnCollide(GameObject otherObject)
        {
            if (isHidden) return;
            if (!manager)
                return;
            // do not execute if on retry.
            if (manager.isFailed) return;

            if (isPopping) return;
            var contactFruit = otherObject.GetComponent<Fruit>();
            if (contactFruit == null)
            {
                if (otherObject.name == "Floor")
                {
                    if (!isTouched && isExplosive)
                        manager.StartCoroutine(Explode());
                    isTouched = true;
                }

                return;
            }
            else
            {
                if (!isTouched && isExplosive)
                    manager.StartCoroutine(Explode());
                isTouched = true;
            }

            if (contactFruit.level == level)
            {
                if (manager)
                {
                    /* allow rat to rat fusion only */
                    if (isRat != contactFruit.isRat) return;
                    
                    /* Need to stop fusion with already fusing */
                    if (contactFruit.isPopping) return;

                    this.isPopping = true;
                    contactFruit.isPopping = true;
                    manager.StartCoroutine(manager.GenerateFruitCR(this, contactFruit));
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isHidden) return;
            if (!manager)
                return;
            if (col.gameObject.name == "Death")
            {
                manager.Fail();
            }
            else
            {
                OnCollide(col.gameObject);
            }
        }

        public void SetColliders(bool enableState)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_colliders[i] != null)
                    _colliders[i].enabled = enableState;
            }
        }

        public void SetAsNonMoving()
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            SetColliders(false);
        }
        public void Fail()
        {
            SetColliders(false);
            _rigidbody.bodyType = RigidbodyType2D.Static;
            if (_animator)
            {
                _animator.enabled = true;
                _animator.SetTrigger("Shake");
            }
        }
        
        public IEnumerator Pop()
        {
            SetColliders(false);
            _rigidbody.bodyType = RigidbodyType2D.Static;
            if (_animator)
            {
                _animator.enabled = true;
                _animator.SetTrigger("Pop");
            }

            /* wait for the pop animation */
            yield return new WaitForSeconds(1f / 12f);

            Hide();
        }

        public IEnumerator Explode()
        {
            if (_animator)
            {
                _animator.enabled = true;
                _animator.SetTrigger("Ohno");
            }

            yield return new WaitForSeconds(2f);
            manager.GenerateExplosion(this);
            yield return Pop();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            transform.SetParent(pool.transform);
            isHidden = true;
        }
        
        public void ResetValues(Transform parentTransform)
        {
            isHidden = false;
            gameObject.SetActive(true);
            if (_animator)
            {
                _animator.SetTrigger("Reset");
                _animator.enabled = false;
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (_colliders[i] != null)
                    _colliders[i].enabled = _initialColliderStates[i];
            }
            
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.velocity = Vector3.zero;
            transform.rotation = _initialRotation;
            isPopping = false;
            isTouched = false;
            
            transform.SetParent(parentTransform);
            transform.position = parentTransform.position;
            transform.localScale = parentTransform.localScale;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}