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

        private bool hovered = false;
        private bool active = false;

        private Sprite sourceBaseSprite;
    
        // Start is called before the first frame update
        void Start()
        {
            sourceBaseSprite = sourceImage.sprite;
        }

        private void Update()
        {
            if (!active && hovered && buildingManager.pendingObj != null)
            {
                active = true;
                sourceImage.sprite = pressedSprite;
                MouseIconHandler.Instance.SetCursorHandHold();
            }

            if (active && buildingManager.pendingObj == null)
            {
                active = false;
                sourceImage.sprite = sourceBaseSprite;
                MouseIconHandler.Instance.SetCursorDefault();
            }
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
