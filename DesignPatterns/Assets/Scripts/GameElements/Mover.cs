using UnityEngine;
using ObserverPattern;

namespace GameElements
{
    public class Mover : Observer
    {
        public override void Execute(bool state)
        {
            float xPos = gameObject.transform.position.x;
            if (state)
            {
                gameObject.transform.position = new Vector3(xPos, 2);
                return;
            }

            gameObject.transform.position = new Vector3(xPos, -2);
        }
    }

}