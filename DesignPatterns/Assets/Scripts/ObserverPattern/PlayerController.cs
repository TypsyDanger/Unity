using UnityEngine;
using Control3D;

namespace ObserverPattern
{
    public class PlayerController : MonoBehaviour
    {
        public Transform playerObject;
        public GameObject bulletPrefab;
        private CommandPattern.Command aKey, sKey, dKey, wKey, spaceKey;
        private bool modifierApplied = false;
        private float cameraX;
        private float cameraY;
        
        Vector2 rotation = Vector2.zero;
        public float speed = 3;

        
        void Start()
        {
            Cursor.visible = false;
            initDefaultControls();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                modifierApplied = true;
            }
            else
            {
                modifierApplied = false;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                aKey.Execute(playerObject, modifierApplied);
            }

            if (Input.GetKey(KeyCode.S))
            {
                sKey.Execute(playerObject, modifierApplied);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                dKey.Execute(playerObject, modifierApplied);
            }

            if (Input.GetKey(KeyCode.W))
            {
                wKey.Execute(playerObject, modifierApplied);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                spaceKey.Execute(playerObject, modifierApplied);
            }
            
            // Mouse Look
            
            cameraX += Input.GetAxis("Mouse X");
            cameraY += Input.GetAxis("Mouse Y");
            
            playerObject.rotation = Quaternion.Euler(-cameraY, cameraX, 0f);

        }

        private void initDefaultControls()
        {
            aKey = new MoveLeft();
            sKey = new MoveBackward();
            dKey = new MoveRight();
            wKey = new MoveForward();
            spaceKey = new FireWeapon(bulletPrefab);
        }
    
    }

}
