using UnityEngine;
using UnityEngine.SceneManagement;

namespace MudOverload.Game
{
    public class PrizeTransitionController : MonoBehaviour
    {
        private static PrizeTransitionController Singleton;

        private static bool FadeOutAnimation = false;
        private static float MaxSize = 25;

        public static void Start(bool transitionAtEnd)
        {
            if (Singleton == null) return;

            Singleton.animationStart = Time.time;
            Singleton.transitionToNextLevelAtEnd = transitionAtEnd;

            Singleton.outline.enabled = true;
            Singleton.colored.enabled = true;

            Singleton.transform.parent = null;

            if (!transitionAtEnd)
            {
                Singleton.transform.position = CameraController.GetCamera().ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
                Singleton.transform.localScale = Vector2.one * MaxSize;
            }
        }

        [SerializeField]
        private float animationSpeed = 15;
        [SerializeField]
        private SpriteRenderer outline;
        [SerializeField]
        private SpriteRenderer colored;

        private float animationStart;
        private bool transitionToNextLevelAtEnd;

        private void Awake()
        {
            Singleton = this;

            outline.enabled = false;
            colored.enabled = false;

            if (FadeOutAnimation)
            {
                FadeOutAnimation = false;
                Start(false);
            }
        }

        private void Update()
        {
            if (animationStart == 0) return;

            print(transitionToNextLevelAtEnd);

            if (transitionToNextLevelAtEnd)
            {
                transform.position = Vector2.Lerp(transform.position, CameraController.GetCamera().ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2)), animationSpeed * Time.deltaTime);
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * MaxSize, animationSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.localScale.x - MaxSize) < 1)
                {
                    FadeOutAnimation = true;
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                transform.position = (Vector2)CameraController.GetCamera().ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, animationSpeed * 2 * Time.deltaTime);
                if (transform.localScale.x < 0.1f)
                {
                    outline.enabled = false;
                    colored.enabled = false;

                    animationSpeed = 0;
                }
            }
        }
    }
}
