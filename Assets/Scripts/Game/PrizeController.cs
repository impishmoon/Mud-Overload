using UnityEngine;

namespace MudOverload.Game
{
	public class PrizeController : MonoBehaviour
	{
		private static PrizeController Singleton;

        public static bool GetIsHovering()
        {
            if (Singleton == null) return false;

            return Singleton.isHovering;
        }

        private bool isHovering;

        [SerializeField]
        private SpriteRenderer outline;

        private void Awake()
        {
            Singleton = this;
        }

        private void Update()
        {
            var mousePosition = CameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);
            var distance = Vector2.Distance(mousePosition, transform.position);

            isHovering = distance < 1;

            outline.color = new Color(1, 1, 1, Mathf.InverseLerp(4, 1, distance) * (isHovering ? 1 : 0.5f));

            if(isHovering && Input.GetMouseButtonDown(0))
            {
                PrizeTransitionController.Start(true);
            }
        }
    }
}
