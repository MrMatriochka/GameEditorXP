using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Shawn.Scripts
{
    public class SpawnParticlesAroundSprite : MonoBehaviour
    {
        [Header("Parameters")]
        public Vector2Int randomAmount = new(5, 10);
        public Transform levelEditorCanvas;
        public float pixelsPerUnit = 32, padding;

        [Header("VFX to instanciate")] 
        public GameObject topVFX; 
        public GameObject rightVFX, bottomVFX, leftVFX;

        private RectTransform rectTransform;
        private Rect rect;

        private Vector3 pos, lossyScale;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            rect = rectTransform.rect;

            pos = rectTransform.position;
            lossyScale = rectTransform.lossyScale;

            Poof();
        }

        public void Poof()
        {
            int amount = Random.Range(randomAmount.x, randomAmount.y);

            for (int i = 0; i < amount; i++)
            {
                bool dirX = RandomBool();
                bool min = RandomBool();
                
                GameObject vfxToInstantiate = dirX switch
                {
                    true => min ? leftVFX : rightVFX,
                    false => min ? bottomVFX : topVFX
                };

                Vector3 vfxScale = vfxToInstantiate.transform.lossyScale;
                //Vector3 multiplier = new Vector3(pixelsPerUnit / lossyScale.x / vfxScale.x, pixelsPerUnit / lossyScale.y / vfxScale.y, 1);
                Vector3 multiplier = new Vector3(pixelsPerUnit / (4 * vfxScale.x), pixelsPerUnit / (4 * vfxScale.y), 1);
                // works if vfxScale.x = 2;
                //multiplier = new Vector3(pixelsPerUnit / 2, pixelsPerUnit / 2, 1);

                float minX = pos.x - padding * multiplier.x;
                float maxX = pos.x + (rect.width + padding) * multiplier.x;
                float minY = pos.y - padding * multiplier.y;
                float maxY = pos.y + (rect.height + padding) * multiplier.y;

                Vector3 randomPosition = new(
                    dirX ? Random.Range(minX, maxX)
                    : min ? minX : maxX,
                    !dirX ? Random.Range(minY, maxY)
                    : min ? minY : maxY );

                vfxToInstantiate.GetComponent<Image>();

                //Instantiate(vfxToInstantiate, randomPosition, Quaternion.identity, levelEditorCanvas);
            }
        }

        bool RandomBool()
        {
            return Random.value > 0.5f;
        }
    }
}
