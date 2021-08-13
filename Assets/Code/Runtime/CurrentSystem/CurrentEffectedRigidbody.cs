using System;
using UnityEngine;

namespace IslandGame.CurrentSystem
{
    public class CurrentEffectedRigidbody : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(CurrentManager.Instance.SampleCurrentAtPosition(transform.position));
        }
    }
}
