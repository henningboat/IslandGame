using System;
using UnityEngine;

namespace IslandGame.InputSystem
{
    public class InputEmulator : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _inputForceCurve;

        private float[] _strokeProgress = new float[2];
        private KeyCode[] inputKeys = new[] {KeyCode.A, KeyCode.D};
        
        private void Update()
        {
            for (int i = 0; i < 2; i++)
            {
                if (Input.GetKey(inputKeys[i]))
                {
                    _strokeProgress[i] += Time.deltaTime;
                }
                else
                {
                    _strokeProgress[i] = 0;
                }
            }
        }

        public float GetInput(bool rightSide)
        {
            return _inputForceCurve.Evaluate(_strokeProgress[rightSide ? 1 : 0]);
        }
    }
}
