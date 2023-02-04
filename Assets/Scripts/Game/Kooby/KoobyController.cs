using MudOverload.Game.Player;
using UnityEngine;

namespace MudOverload.Game.Kooby
{
	public class KoobyController : MonoBehaviour
	{
        [SerializeField]
        private float hoverSpeed = 1;
        [SerializeField]
        private float hoverSize = 1;

        [SerializeField]
        private float moveSpeed = 1;

        private Vector2 targetPosition;

        private void Awake()
        {
            targetPosition = transform.position;
        }

        private void Update()
        {
            var playerPosition = PlayerController.GetPosition();

            targetPosition = Vector2.Lerp(targetPosition, playerPosition + new Vector2(Mathf.Sin(Time.time) * 1, 3.5f + Mathf.Cos(Time.time) * 0.25f), moveSpeed * Time.deltaTime);

            transform.position = targetPosition + new Vector2(0, Mathf.Sin(Time.time * hoverSpeed) * hoverSize);
        }
    }
}
