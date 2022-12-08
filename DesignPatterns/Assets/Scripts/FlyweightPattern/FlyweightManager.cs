using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyweightPattern
{
    public class FlyweightManager : MonoBehaviour
    {
        private List<Kit> _nonFlyweightKits = new List<Kit>();
        private List<Kit> _flyweightKits = new List<Kit>();
        void Start()
        {
            _nonFlyweightKits = createNonFlyweightKits();
            _flyweightKits = createFlyweightKits();
        }

        private List<Kit> createNonFlyweightKits()
        {
            List<Kit> theReturnKits = new List<Kit>();

            ScoutKit ScoutAKit = new ScoutKit();
            ScoutKit ScoutBKit = new ScoutKit();
            ScoutKit ScoutCKit = new ScoutKit();
            OrienteerKit OrienteerAKit = new OrienteerKit();
            OrienteerKit OrienteerBKit = new OrienteerKit();
            EngineerKit EngineerAKit = new EngineerKit();

            theReturnKits.Add(ScoutAKit);
            theReturnKits.Add(ScoutBKit);
            theReturnKits.Add(ScoutCKit);
            theReturnKits.Add(OrienteerAKit);
            theReturnKits.Add(OrienteerBKit);
            theReturnKits.Add(EngineerAKit);

            return theReturnKits;
        }

        private List<Kit> createFlyweightKits()
        {
            List<Kit> theReturnKits = new List<Kit>();

            ScoutKit sharedScoutKit = new ScoutKit();
            OrienteerKit sharedOrienteerKit = new OrienteerKit();
            EngineerKit sharedEngineerKit = new EngineerKit();
            
            ScoutKit ScoutAKit = sharedScoutKit;
            ScoutKit ScoutBKit = sharedScoutKit;
            ScoutKit ScoutCKit = sharedScoutKit;
            OrienteerKit OrienteerAKit = sharedOrienteerKit;
            OrienteerKit OrienteerBKit = sharedOrienteerKit;
            EngineerKit EngineerAKit = sharedEngineerKit;

            theReturnKits.Add(ScoutAKit);
            theReturnKits.Add(ScoutBKit);
            theReturnKits.Add(ScoutCKit);
            theReturnKits.Add(OrienteerAKit);
            theReturnKits.Add(OrienteerBKit);
            theReturnKits.Add(EngineerAKit);

            return theReturnKits;
        }


    }
}