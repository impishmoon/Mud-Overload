using UnityEngine;

namespace MudOverload.Game
{
	public class MiningController : MonoBehaviour
	{
		private static MiningController Singleton;

        public static void StartMining(Vector2 position)
        {
            if (Singleton == null) return;

            Singleton.transform.position = MiningPreviewController.GetMiningPosition(position);
            Singleton.renderer.color = Color.white;
        }

        public static void SetProgress(float progress)
        {
            if (Singleton == null) return;

            Singleton.renderer.color = new Color(1, 1, 1, progress);
        }

        public static void StopMining()
        {
            if (Singleton == null) return;

            Singleton.renderer.color = new Color(0, 0, 0, 0);
        }

        private new SpriteRenderer renderer;

        private void Awake()
        {
            Singleton = this;

            renderer = GetComponent<SpriteRenderer>();

            renderer.color = new Color(0, 0, 0, 0);
        }
    }
}
