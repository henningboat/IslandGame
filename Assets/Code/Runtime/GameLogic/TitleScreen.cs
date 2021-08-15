using System;
using DG.Tweening;
using UnityEngine;

namespace IslandGame.GameLogic
{
    public class TitleScreen : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
        }

        private bool _triggered;
        
        private void Update()
        {
            if (!_triggered && _rigidbody.velocity.magnitude > 0.1f)
            {
                GetComponent<CanvasGroup>().DOFade(0, 2);
            }
        }
    }
}
