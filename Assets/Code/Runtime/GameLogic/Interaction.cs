using System;
using System.Collections;
using UnityEngine;

namespace IslandGame.GameLogic
{
    [RequireComponent(typeof(Collider))]
    public abstract class Interaction : MonoBehaviour
    {
        private Collider _collider;

        private const float interactionCooldown = 3;
        private float _lastInteractionTime=float.MinValue;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerInventory = other.GetComponentInChildren<PlayerInventory>();
            
            if(playerInventory==null)
                return;

            if (_lastInteractionTime + interactionCooldown < Time.time)
            {
                StartCoroutine(Interact(playerInventory));
                _lastInteractionTime = Time.time;
            }
        }

        protected abstract IEnumerator Interact(PlayerInventory playerInventory);
    }
}
