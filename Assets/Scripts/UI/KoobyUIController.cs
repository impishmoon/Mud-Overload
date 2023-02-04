using TMPro;
using UnityEngine;

namespace MudOverload.UI
{
	public class KoobyUIController : MonoBehaviour
	{
		private static KoobyUIController Singleton;

        public static void ShowUI(string text)
        {
            if (Singleton == null) return;

            Singleton.lcg.target = 1;
            Singleton.text.text = text;

            IsUIOpen.AddUser("kooby");
        }

        [SerializeField]
        private LerpCanvasGroup lcg;
        [SerializeField]
        private TMP_Text text;

        private void Awake()
        {
            Singleton = this;
        }

        private void Update()
        {
            if(lcg.target == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    lcg.target = 0;
                    IsUIOpen.RemoveUser("kooby");
                }
            }
        }
    }
}
