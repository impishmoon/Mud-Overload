using UnityEngine;

namespace MudOverload.UI
{
	public class StartMenuController : MonoBehaviour
	{
        private LerpCanvasGroup lcg;

        private void Awake()
        {
            lcg = GetComponent<LerpCanvasGroup>();
            IsUIOpen.AddUser("startmenu");
        }

        public void StartGame()
        {
            lcg.target = 0;
            IsUIOpen.RemoveUser("startmenu");
        }
    }
}
