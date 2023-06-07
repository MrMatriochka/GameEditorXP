using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shawn.Scripts
{
    public class SpawnPrefabRadially : MonoBehaviour
    {
        public GameObject[] prefabsToInstantiate;
        public float distance = 0;
        public int amount;

        
        [Range(0, 360)] public float groupAngleCorrection = 0;
        [Range(0, 360)] public float instanceAngleCorrection = 0;

        private void Start()
        {
            Transform t = transform;
            Vector3 initPos = t.position;

            for (int i = 0; i < amount; i++)
            {
                float angle = 360 / amount * (i+1) + groupAngleCorrection;
                
                Vector3 position = new Vector3(
                    initPos.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                    initPos.y + distance * Mathf.Sin(angle * Mathf.Deg2Rad),
                    initPos.z);
                GameObject obj = Instantiate(prefabsToInstantiate[Random.Range(0, prefabsToInstantiate.Length)],
                    position, Quaternion.Euler(0, 0, angle), t);

                Transform oT = obj.transform;
                Vector3 rot = oT.localRotation.eulerAngles;
                oT.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z + instanceAngleCorrection);
            }
        }
    }
}