using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class BossBatteryBarController : MonoBehaviour
	{
        [SerializeField]
        private float animateSpeed = 6f;

		private new SpriteRenderer renderer;

        private bool visible = true;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            renderer.color = Color.Lerp(renderer.color, visible ? new Color(0, 0.7264151f, 0) : new Color(0, 0.284f, 0), animateSpeed * Time.deltaTime);
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
        }
    }
}
