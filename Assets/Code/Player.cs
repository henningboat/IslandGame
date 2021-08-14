using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float forwardSpeed = 0.1f;
        private Rigidbody Rig;        

        [SerializeField]
        private GameObject playerCube;
        [SerializeField]
        private GameObject camera;
        private Vector3 cameraStartLocalPosition;
        [SerializeField]
        private float cameraPullBackMultiplier = 0.05f;

        public SerialController serialController;

        private float oldRightForce = 0.0f;
        private float oldLeftForce = 0.0f;


        void Start()
        {
            cameraStartLocalPosition = camera.transform.localPosition;

            serialController = GetComponentInChildren<SerialController>();

            print(serialController);

            Rig = GetComponent<Rigidbody>();
        }

        void Update()
        {
            UpdatePaddles();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Rig.AddForceAtPosition(playerCube.transform.forward * forwardSpeed, playerCube.transform.position + (playerCube.transform.right * 0.5f));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Rig.AddForceAtPosition(playerCube.transform.forward * forwardSpeed, playerCube.transform.position + (playerCube.transform.right * -0.5f));
            }

            camera.transform.localPosition = new Vector3(cameraStartLocalPosition.x, cameraStartLocalPosition.y, cameraStartLocalPosition.z - (Vector3.Magnitude(Rig.velocity) * cameraPullBackMultiplier));
     
        }


        private void UpdatePaddles()
        {
            //---------------------------------------------------------------------
            // Receive data
            //---------------------------------------------------------------------

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
                int Newforce = int.Parse(message.Split(';')[1]);

                
                if(int.Parse(message.Split(';')[0]) == 1)
                {
                    print("right");
                    updateRightPaddle(int.Parse(message.Split(';')[1]));
                }
                    
                if(int.Parse(message.Split(';')[0]) == 2)
                {
                    print("Left");
                    updateLeftPaddle(int.Parse(message.Split(';')[1]));
                }
            }
        }

        private void updateRightPaddle(int NewForce)
        {
            if ((NewForce - oldRightForce) > 0)
            {
                float force = (NewForce - oldRightForce) * 0.01f;
                print("Force on Left: " + force);
                Rig.AddForceAtPosition(playerCube.transform.forward * (forwardSpeed * force), playerCube.transform.position + (playerCube.transform.right * 0.5f));
            }

            oldRightForce = NewForce;
        }

        private void updateLeftPaddle(int NewForce)
        {
            if ((NewForce - oldLeftForce) > 0)
            {
                float force = (NewForce - oldLeftForce) * 0.01f;
                print("Force on Left: " + force);
                Rig.AddForceAtPosition(playerCube.transform.forward * (forwardSpeed * force), playerCube.transform.position + (playerCube.transform.right * -0.5f));
            }

            oldLeftForce = NewForce;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(playerCube.transform.position + (playerCube.transform.right * 0.5f), playerCube.transform.forward * forwardSpeed);
            Gizmos.DrawRay(playerCube.transform.position + (playerCube.transform.right * -0.5f), playerCube.transform.forward * forwardSpeed);

            /*
            Gizmos.DrawRay(new Vector3(playerCube.transform.position.x - 0.5f, playerCube.transform.position.y, playerCube.transform.position.z), playerCube.transform.forward * forwardSpeed);
            Gizmos.DrawRay(new Vector3(playerCube.transform.position.x + 0.5f, playerCube.transform.position.y, playerCube.transform.position.z), playerCube.transform.forward * forwardSpeed);
            */
        }
    }
}