using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class SpriteRedFlasher : MonoBehaviour
	{
        [SerializeField]
        private float lerpSpeed = 6;
        [SerializeField]
        private float flashSpeed = 10;

        [HideInInspector]
        public float redOverrideA = 0;

        private bool isEnabled;

        private new SpriteRenderer renderer;

        private new Collider2D collider;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            var redFlash = Color.Lerp(Color.red, Color.white, (Mathf.Sin(Time.time * flashSpeed) + 1) / 2);
            var lerpedColor = Color.Lerp(renderer.color, isEnabled ? redFlash : Color.white, lerpSpeed * Time.deltaTime);
            var finalColor = Color.Lerp(lerpedColor, Color.red, redOverrideA);
            if (!collider.isTrigger) finalColor = Color.white;
            renderer.color = finalColor;
        }

        public void SetEnabled(bool isEnabled)
        {
			this.isEnabled = isEnabled;
        }
	}
}
