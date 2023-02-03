using UnityEngine;

namespace MudOverload.Game.MiningEffect
{
	public class MiningEffectController : MonoBehaviour
	{
		[SerializeField]
		private float effectTime = 1f;

		[SerializeField]
		private float spinTime = 10f;

		private new SpriteRenderer renderer;

		private float effectStart;
		private Vector2 source;
		private Vector2 target;


		public void Activate(Sprite sprite, Vector2 source, Vector2 target)
        {
			gameObject.SetActive(true);

			renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = sprite;

			effectStart = Time.time;
			this.source = source;
			this.target = target;
        }

		public void Deactivate()
        {
			gameObject.SetActive(false);
        }

        private void Update()
        {
			float progress = Mathf.InverseLerp(effectStart, effectStart + effectTime, Time.time);
			transform.position = Vector2.Lerp(source, target, progress) + new Vector2(0, Mathf.Sin(progress * Mathf.PI));
			transform.Rotate(new Vector3(0,0,spinTime * Time.deltaTime));

			if(progress >= 1)
            {
				Destroy(gameObject);
            }
        }
    }
}
