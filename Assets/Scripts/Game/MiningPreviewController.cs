using UnityEngine;

namespace MudOverload.Game
{
	public class MiningPreviewController : MonoBehaviour
	{
		private static MiningPreviewController Singleton;

        public static Vector2 GetMiningPosition(Vector2 position)
        {
            return new Vector2(Mathf.Floor(position.x) + 0.5f, Mathf.Floor(position.y) + 0.5f);
        }

        public static void SetPosition(Vector2 position)
        {
            if (Singleton == null) return;

            Singleton.transform.position = GetMiningPosition(position);
            Singleton.renderer.color = new Color(1,1,1, 0.02745098f);
        }

        public static void Hide()
        {
            if (Singleton == null) return;

            Singleton.renderer.color = new Color(0,0,0,0);
        }

        private new SpriteRenderer renderer;

        private void Awake()
        {
            Singleton = this;

            renderer = GetComponent<SpriteRenderer>();
        }
    }
}
