using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class ControlManager : MonoBehaviour
    {
        public Transform objectTransform;
        private bool moveFaster;
        private Command buttonA, buttonS, buttonD, buttonW;
        
        void Start()
        {
            buttonA = new MoveLeft();
            buttonS = new MoveDown();
            buttonD = new MoveRight();
            buttonW = new MoveUp();
        }

        void Update()
        {
            CheckInput();
        }
        
        public void CheckInput() {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveFaster = true;
            }
            else
            {
                moveFaster = false;
            }
            
            if(Input.GetKeyDown(KeyCode.A)) 
            {
                buttonA.Execute(objectTransform, moveFaster);
                return;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                buttonS.Execute(objectTransform, moveFaster);
                return;
            }
            
            if(Input.GetKeyDown(KeyCode.D)) 
            {
                buttonD.Execute(objectTransform, moveFaster);
                return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                buttonW.Execute(objectTransform, moveFaster);
                return;
            }
        }
    }
}