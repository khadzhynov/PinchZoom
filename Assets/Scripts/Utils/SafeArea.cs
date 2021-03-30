using UnityEngine;

namespace BlastGame.Utils
{
    public class SafeArea : MonoBehaviour
    {
        private void Start()
        {
            Align();
        }

        public void Align()
        {
            var safeArea = Screen.safeArea;
#if UNITY_EDITOR
            //----- Simulate notch in editor
            safeArea.height -= 60;
            safeArea.width -= 30;
            safeArea.x = 15;
#endif

            if (safeArea != new Rect(0, 0, Screen.width, Screen.height))
            {
                var rectTransform = transform as RectTransform;
                Vector2 minAnchor = Vector2.zero;
                Vector2 maxAnchor = Vector2.zero;

                minAnchor.x = (float)safeArea.x / (float)Screen.width;
                maxAnchor.x = (float)(safeArea.x + safeArea.width) / (float)Screen.width;
                minAnchor.y = (float)safeArea.y / (float)Screen.height;
                maxAnchor.y = (float)(safeArea.y + safeArea.height) / (float)Screen.height;
                rectTransform.anchorMin = minAnchor;
                rectTransform.anchorMax = maxAnchor;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}