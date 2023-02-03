using MudOverload.Game.Player;
using UnityEngine;

namespace MudOverload.Game
{
	public class TrashPortalController : MonoBehaviour
	{
		private static TrashPortalController Singleton;

        private new SpriteRenderer renderer;

        private void Awake()
        {
            Singleton = this;

            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var player = PlayerController.GetPosition();

            renderer.flipX = player.x < transform.position.x;
        }
    }
}
