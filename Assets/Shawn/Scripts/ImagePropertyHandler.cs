using UnityEngine;
using UnityEngine.UI;

namespace Shawn.Scripts
{
    public class ImagePropertyHandler : MonoBehaviour
    {
        public Image img;

        public Color col;
        // Start is called before the first frame update

        public void SetColor()
        {
            img.color = col;
        }
    }
}
