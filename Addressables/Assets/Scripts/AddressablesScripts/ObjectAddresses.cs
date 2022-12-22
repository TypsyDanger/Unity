using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AddressablesScripts
{
    public static class ObjectAddresses
    {
        public static readonly List<AddressableItem> Elements = new List<AddressableItem>
        {
            new AddressableItem("BLOCK_PREFAB", "Prefabs/Block.prefab")
        };

        public static string GetByName(string theName)
        {
           return Elements.Find(x => (x.Name == theName)).Value;
        }
    }
}