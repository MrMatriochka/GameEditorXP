using System.Collections;
using UnityEngine;

namespace Shawn.Scripts
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float seconds = 1.0f;
    
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }

        private void Start()
        {
            StartCoroutine(Timer());
        }
    }
}
