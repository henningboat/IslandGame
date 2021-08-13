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



        void Start()
        {
            cameraStartLocalPosition = camera.transform.localPosition;
            Rig = GetComponent<Rigidbody>();
        }

        void Update()
        {
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