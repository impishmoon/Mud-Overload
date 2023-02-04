using MudOverload.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MudOverload.Game
{
	public class CameraController : MonoBehaviour
	{
		private static CameraController Singleton;

        [SerializeField]
        private float cameraLerpSpeed = 1f;

        public static Camera GetCamera()
        {
            if (Singleton == null) return null;

            return Singleton.camera;
        }

        private new Camera camera;

        private void Awake()
        {
            Singleton = this;

            camera = GetComponent<Camera>();

            transform.position = GetPosition();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0) return;
            transform.position = Vector3.Lerp(transform.position, GetPosition(), cameraLerpSpeed * Time.deltaTime);
        }

        private Vector3 GetPosition()
        {
            var playerPosition = PlayerController.GetPosition();

            return new Vector3(playerPosition.x, playerPosition.y, -10);
        }
    }
}
