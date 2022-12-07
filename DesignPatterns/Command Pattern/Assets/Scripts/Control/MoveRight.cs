using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Control
{
    public class MoveLeft : Command
    {
        public override void Execute(Transform playerTransform)
        {
            Move(playerTransform);
        }

        private override void Move(Transform playerTransform)
        {
            playerTransform.Translate(playerTransform.forward * moveDistance);
        }
    } 
}
