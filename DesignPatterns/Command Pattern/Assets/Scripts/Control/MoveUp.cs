using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Control
{
    public class MoveRight : Command
    {
        public override void Execute(Transform playerTransform)
        {
            Move(playerTransform);
        }

        public override void Move(Transform playerTransform)
        {
            playerTransform.Translate(playerTransform.right * moveDistance);
        }
    } 
}
