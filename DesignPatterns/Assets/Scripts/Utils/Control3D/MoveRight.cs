using CommandPattern;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Control3D
{
    public class MoveRight : Command
    {
        public override void Execute(Transform objectTransform, bool modifierApplied = false)
        {
            objectTransform.position += objectTransform.right * Time.deltaTime * (moveDistance * (modifierApplied ? 2 : 1));
        }
    }
}