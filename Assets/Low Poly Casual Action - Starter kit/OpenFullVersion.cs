using UnityEngine;

namespace LowPolyCasualActionKit
{
    public class OpenFullVersion : MonoBehaviour
    {
        public string fullVersionUrl = "https://assetstore.unity.com/packages/slug/401604";

        public void OpenLink()
        {
            if (!string.IsNullOrEmpty(fullVersionUrl))
            {
                Application.OpenURL(fullVersionUrl);
            }
        }
    }
}