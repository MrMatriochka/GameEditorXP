using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Shawn.Scripts
{
    public class TrashVisualsHandler : MonoBehaviour
    {
        [Category("Dependencies")]
        public BuildingManager bm;
        
        [Category("Parameters")]
        public GameObject[] buttonsToDeactivate;
        public GameObject disappearFXPrefab;
        public bool invisibleOnStart;

        private Image img;
        private Image childImg;

        private bool holding;
        private bool bm_couldPlace;

        // Start is called before the first frame update
        void Start()
        {
            img = GetComponent<Image>();
            childImg = GetComponentInChildren<Image>();
            if (invisibleOnStart)
            {
                img.enabled = false;
                childImg.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            bool pendingExists = bm.pendingObj != null;
            
            if (pendingExists && !holding)
            {
                OnObjectHeld();
                holding = true;
            }
            else if(!pendingExists && holding)
            {
                OnObjectLetGo();
                if(!bm_couldPlace)
                    Instantiate(disappearFXPrefab, Input.mousePosition, Quaternion.identity, transform.parent);
                holding = false;
            }
            
            bm_couldPlace = pendingExists && bm.canPlace;
        }

        void OnObjectLetGo()
        {
            if (invisibleOnStart)
            {
                img.enabled = false;
                childImg.enabled = false;
                foreach (GameObject o in buttonsToDeactivate)
                {
                    o.SetActive(true);
                }
            }
        }

        void OnObjectHeld()
        {
            if (invisibleOnStart)
            {
                img.enabled = true;
                childImg.enabled = true;
                foreach (GameObject o in buttonsToDeactivate)
                {
                    o.SetActive(false);
                }
            }
        }
    }
}
