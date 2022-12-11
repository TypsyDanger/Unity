using UnityEngine;

namespace ObserverPattern
{
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Execute(bool state);
    }
}