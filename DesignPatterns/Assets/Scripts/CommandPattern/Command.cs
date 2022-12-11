using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommandPattern
{
    public abstract class Command
    {
        protected float moveDistance = 0.5f;
        protected bool modifierApplied = false;
        public abstract void Execute(Transform objectTransform, bool moveFaster = false);
        public virtual void Move(Transform objectTransform, bool moveFaster = false) {}
    }
}
