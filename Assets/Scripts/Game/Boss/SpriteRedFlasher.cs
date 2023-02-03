using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class SpriteRedFlasher : MonoBehaviour
	{
        [SerializeField]
        private float lerpSpeed = 6;
        [SerializeField]
        private float flashSpeed = 10;

		private bool isEnabled;

        private new SpriteRenderer renderer;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var redFlash = Color.Lerp(Color.red, Color.white, (Mathf.Sin(Time.time * flashSpeed) + 1) / 2);
            renderer.color = Color.Lerp(renderer.color, isEnabled ? redFlash : Color.white, lerpSpeed * Time.deltaTime);
        }

        public void SetEnabled(bool isEnabled)
        {
			this.isEnabled = isEnabled;
        }
	}
}
