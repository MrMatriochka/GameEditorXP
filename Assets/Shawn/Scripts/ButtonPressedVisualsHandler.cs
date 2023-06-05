using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.Scripts
{
    public class ButtonPressedVisualsHandler : MonoBehaviour
    {
        public BuildingManager buildingManager;
        public Image sourceImage;
        public Sprite hoveredSprite;
        public Sprite pressedSprite;
        // public GameObject placementPoofFX;
        // public GameObject levelEditorCanvas;

        private bool hovered = false;
        private bool active = false;
        private bool bm_couldPlace = false;

        private Sprite sourceBaseSprite;
    
        // Start is called before the first frame update
        void Start()
        {
            sourceBaseSprite = sourceImage.sprite;
        }

        private void Update()
        {
            bool pendingExists = buildingManager.pendingObj != null;
            
            if (!active && hovered && pendingExists)
            {
                active = true;
                sourceImage.sprite = pressedSprite;
                MouseIconHandler.Instance.SetCursorHandHold();
            }

            if (active && !pendingExists)
            {
                active = false;
                sourceImage.sprite = sourceBaseSprite;
                MouseIconHandler.Instance.SetCursorDefault();
                if (bm_couldPlace)
                {
                    // Instantiate(placementPoofFX, Input.mousePosition, Quaternion.identity, levelEditorCanvas.transform);
                }
            }

            bm_couldPlace = pendingExists && buildingManager.canPlace;
        }

        public void SetMouseEntered()
        {
            hovered = true;
            if (!active)
            {
                sourceImage.sprite = hoveredSprite;
                MouseIconHandler.Instance.SetCursorHandOpen();
            }
        }
    
        public void SetMouseExited()
        {
            hovered = false;
            if (!active)
            {
                sourceImage.sprite = sourceBaseSprite;
                MouseIconHandler.Instance.SetCursorDefault();
            }
        }
    }
}
