using UnityEngine;

namespace MudOverload.Game.Boos
{
	public class TransformKeeper : MonoBehaviour
	{
        private Vector3 position;
        private Vector3 scale;

        private void Awake()
        {
            position = transform.localPosition;
            scale = transform.localScale;
        }

        public void Reset()
        {
            transform.localPosition = position;
            transform.localScale = scale;
        }
    }
}
