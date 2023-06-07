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
        public GameObject placementPoofFX;
        public Transform fxCanvas;

        private bool hovered = false;
        private bool active = false;
        private bool bmCouldPlace = false;

        private Sprite sourceBaseSprite;
        //private GameObject lastObject;
        private Vector3 decalage;
    
        // Start is called before the first frame update
        void Start()
        {
            sourceBaseSprite = sourceImage.sprite;
        }

        private void Update()
        {
            GameObject pendingObj = buildingManager.pendingObj;
            
            if (!active && hovered && pendingObj)
            {
                active = true;
                sourceImage.sprite = pressedSprite;
                MouseIconHandler.Instance.SetCursorHandHold();
            }

            if (active && !pendingObj)
            {
                active = false;
                sourceImage.sprite = sourceBaseSprite;
                MouseIconHandler.Instance.SetCursorDefault();
                if (bmCouldPlace)
                {
                    Transform objTransform = buildingManager.placedObject[^1].transform;
                    Instantiate(placementPoofFX, objTransform.position, Quaternion.identity, fxCanvas);
                }
            }

            bmCouldPlace = pendingObj && buildingManager.canPlace;
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
