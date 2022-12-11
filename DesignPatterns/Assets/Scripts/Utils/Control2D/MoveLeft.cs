using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommandPattern
{
    public class MoveLeft : Command
    {
        public override void Execute(Transform objectTransform, bool moveFaster)
        {
            Move(objectTransform, moveFaster);
        }

        public override void Move(Transform objectTransform, bool moveFaster)
        {
            float speed = 1.0f;
            
            if (moveFaster)
            {
                speed = 2.0f;
            }
            
            objectTransform.Translate(-objectTransform.right * moveDistance * speed);
        }
    } 
}
