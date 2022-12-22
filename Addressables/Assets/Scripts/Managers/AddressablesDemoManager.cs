using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddressablesScripts;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public class AddressablesDemoManager : MonoBehaviour
    {

        private GameObject BlockObject;
        
        void Start()
        {
            ProcessAddressables();
        }

        private void ProcessAddressables()
        {
            Addressables.LoadAssetAsync<GameObject>(ObjectAddresses.GetByName("BLOCK_PREFAB")).Completed += OnAddressableLoadComplete;
        }

        private void OnAddressableLoadComplete(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> theObject)
        {
            InstantiateGameObject(theObject.Result, new Vector3(0.0f, 0.0f, 0.0f), BlockObject);
        }

        private void InstantiateGameObject(GameObject theGameObject, Vector3 thePosition, GameObject theReference = null)
        {
            if (theReference != null)
            {
                theReference = GameObject.Instantiate(theGameObject, thePosition, Quaternion.identity);
                return;
            }

            GameObject.Instantiate(theGameObject, thePosition, Quaternion.identity);
        }
    }
}