using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float forwardSpeed = 0.1f;
        private float startForwardSpeed;
        private Rigidbody Rig;

        [SerializeField]
        private float ForcePositionOffset = 0.2f;

        [SerializeField]
        private GameObject playerCube;
        [SerializeField]
        private GameObject camera;
        private Vector3 cameraStartLocalPosition;
        private Vector3 CameraTargetPosition;
        [SerializeField]
        private float cameraMovmentSpeed;
        [SerializeField]
        private float cameraPullBackMultiplier = 0.05f;

        public SerialController serialController;

        private float oldRightForce = 0.0f;
        private float oldLeftForce = 0.0f;
        [SerializeField] private float _paddleRotationRange = 1;
        [SerializeField] private Transform _rightPaddle;
        [SerializeField] private Transform _leftPaddle;


        void Start()
        {
            cameraStartLocalPosition = camera.transform.localPosition;

            serialController = GetComponentInChildren<SerialController>();

            startForwardSpeed = forwardSpeed;

            Rig = GetComponent<Rigidbody>();
        }

        void Update()
        {
            UpdatePaddles();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Rig.AddForceAtPosition(playerCube.transform.forward * forwardSpeed, playerCube.transform.position + (playerCube.transform.right * ForcePositionOffset));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Rig.AddForceAtPosition(playerCube.transform.forward * forwardSpeed, playerCube.transform.position + (playerCube.transform.right * -ForcePositionOffset));
            }

            updateCameraPosition();
        }

        private void updateCameraPosition()
        {
            CameraTargetPosition = new Vector3(cameraStartLocalPosition.x, cameraStartLocalPosition.y, cameraStartLocalPosition.z - (Vector3.Magnitude(Rig.velocity) * cameraPullBackMultiplier));

            if (CameraTargetPosition.z < camera.transform.localPosition.z)
            {
                camera.transform.localPosition = new Vector3(cameraStartLocalPosition.x, cameraStartLocalPosition.y, camera.transform.localPosition.z - (cameraMovmentSpeed * Time.deltaTime));
            } else if (camera.transform.localPosition.z <= cameraStartLocalPosition.z)
            {
                camera.transform.localPosition = new Vector3(cameraStartLocalPosition.x, cameraStartLocalPosition.y, camera.transform.localPosition.z + (cameraMovmentSpeed * Time.deltaTime));
            } else
            {
            }
        }


        private void UpdatePaddles()
        {
            if (Vector3.Magnitude(Rig.velocity) > 2.75f)
            {
                forwardSpeed = startForwardSpeed * ((Vector3.Magnitude(Rig.velocity) - 2.75f) / 7.25f);
            }
            else
            {
                forwardSpeed = startForwardSpeed;
            }


            string message = serialController.ReadSerialMessage();

            if (message == null)
                return;

            // Check if the message is plain data or a connect/disconnect event.
            if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
                Debug.Log("Connection established");
            else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
                Debug.Log("Connection attempt failed or disconnection detected");
            else
            {
                if (message.Length == 0)
                    return;

                int Newforce = int.Parse(message.Split(';')[1]);
                
                if(int.Parse(message.Split(';')[0]) == 2)
                {
                    updateRightPaddle(int.Parse(message.Split(';')[1]));
                }
                    
                if(int.Parse(message.Split(';')[0]) == 1)
                {
                    updateLeftPaddle(int.Parse(message.Split(';')[1]));
                }
            }
        }

        private void updateRightPaddle(int NewForce)
        {
            UpdatePaddleGraphics(_leftPaddle,NewForce);
            if ((NewForce - oldRightForce) > 0)
            {
                float force = (NewForce - oldRightForce) * 0.01f;
                Rig.AddForceAtPosition(playerCube.transform.forward * (forwardSpeed * force), playerCube.transform.position + (playerCube.transform.right * ForcePositionOffset));
            }

            oldRightForce = NewForce;
        }

        private void UpdatePaddleGraphics(Transform rightPaddle, int newForce)
        {
            rightPaddle.localEulerAngles = Vector3.up * (newForce - 50) * _paddleRotationRange;
        }

        private void updateLeftPaddle(int NewForce)
        {
            UpdatePaddleGraphics(_rightPaddle,NewForce);
            if ((NewForce - oldLeftForce) > 0)
            {
                float force = (NewForce - oldLeftForce) * 0.01f;
                Rig.AddForceAtPosition(playerCube.transform.forward * (forwardSpeed * force), playerCube.transform.position + (playerCube.transform.right * -ForcePositionOffset));
            }


            oldLeftForce = NewForce;
        }
    }
}