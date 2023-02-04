using UnityEngine;
using UnityEngine.SceneManagement;

namespace MudOverload.UI
{
	public class DeathUIController : MonoBehaviour
	{
        private static DeathUIController Singleton;

        public static void ShowMenu()
        {
            if (Singleton == null) return;

            Singleton.lcg.target = 1;
        }

		private LerpCanvasGroup lcg;

        private void Awake()
        {
            Singleton = this;

            lcg = GetComponent<LerpCanvasGroup>();
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(1);
        }
    }
}
