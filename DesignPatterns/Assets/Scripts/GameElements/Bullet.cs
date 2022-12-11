using UnityEngine;
using ObserverPattern;

namespace GameElements
{
    public class Bullet : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("COLLIDED!");
            if (collision.gameObject.name == "OnSphere")
            {
                Observable.onStateToggledOn?.Invoke();
            }

            if (collision.gameObject.name == "OffSphere")
            {
                Observable.onStateToggledOff?.Invoke();
            }
        }
    }

}