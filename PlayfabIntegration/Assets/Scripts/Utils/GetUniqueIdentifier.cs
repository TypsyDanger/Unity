using UnityEngine.Device;

namespace Utils
{
    public static class GetUniqueIdentifier
    {
        public static string generate()
        {
            string uniqueId = "";
            
            #if UNITY_EDITOR
            uniqueId = SystemInfo.deviceUniqueIdentifier + "_unity-ide";
            #endif

            return uniqueId;
        }
    }
}