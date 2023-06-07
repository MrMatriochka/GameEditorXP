using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Shawn.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class SetAnimIndex : MonoBehaviour
    {
        public string propertyName;
        public Vector2Int randomIndexRange;

        private void Start()
        {
            GetComponent<Animator>().SetInteger(Animator.StringToHash(propertyName), Random.Range(randomIndexRange.x, randomIndexRange.y + 1));
        }
    }
}
