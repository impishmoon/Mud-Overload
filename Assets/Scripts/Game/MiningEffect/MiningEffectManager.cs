using UnityEngine;

namespace MudOverload.Game.MiningEffect
{
	public class MiningEffectManager : MonoBehaviour
	{
		private static MiningEffectManager Singleton;

		public static void CreateEffect(Sprite sprite, Vector2 source, Vector2 target)
        {
            if (Singleton == null) return;

            var newTemplate = Instantiate(Singleton.template);
            newTemplate.Activate(sprite, source, target);
        }

        [SerializeField]
        private MiningEffectController template;

        private void Awake()
        {
            Singleton = this;

            template.Deactivate();
        }
    }
}
