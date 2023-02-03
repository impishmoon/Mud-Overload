using UnityEngine;

namespace MudOverload.Game
{
	public class CameraController : MonoBehaviour
	{
		private static CameraController Singleton;

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
        }
    }
}
