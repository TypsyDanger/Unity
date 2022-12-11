using System;
using GameElements;
using Unity.VisualScripting;
using UnityEngine;

namespace ObserverPattern
{
    public class Observable : MonoBehaviour
    {
        public static Action onStateToggledOn;
        public static Action onStateToggledOff;
       
        public ObserverList observers;

        public void Start()
        {
            onStateToggledOn += togglePowerOn;
            onStateToggledOff += togglePowerOff;
        }
        
        public void togglePowerOn()
        {
            onTogglePower(true);
        }

        public void togglePowerOff()
        {
            onTogglePower(false);
        }
        public void onTogglePower(bool powerState)
        {
            foreach (GameObject observerItem in observers.observerList)
            {
                observerItem.GetComponent<Mover>().Execute(powerState);
            }
        }
    }

}