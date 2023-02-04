using UnityEngine;

namespace MudOverload.Game.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] idleSprites;
        [SerializeField]
        private Sprite[] runningSprites;
        [SerializeField]
        private Sprite[] miningSprites;

        private new SpriteRenderer renderer;
        private Rigidbody2D rb;

        private int frameIndex = 0;
        private int animationIndex = 0;
        private int lastAnimationIndex = 0;
        private float lastChangeFrame = 0;

        [SerializeField]
        private float animationSpeed = 0.1f;

        public bool isMining = false;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var diff = rb.velocity;
            var hspeed = diff.x;

            if (isMining)
            {
                animationIndex = 2;
                ResetFrameIndexIfNeeded();

                if (Time.time > lastChangeFrame + animationSpeed) AdvanceFrameIndex(miningSprites);
            }
            else if (hspeed != 0)
            {
                animationIndex = 1;
                ResetFrameIndexIfNeeded();

                if (Time.time > lastChangeFrame + animationSpeed) AdvanceFrameIndex(runningSprites);

                renderer.flipX = hspeed < 0;
            }
            else
            {
                animationIndex = 0;
                ResetFrameIndexIfNeeded();

                if (Time.time > lastChangeFrame + animationSpeed) AdvanceFrameIndex(idleSprites);
            }

            lastAnimationIndex = animationIndex;
        }

        private void ResetFrameIndexIfNeeded()
        {
            if(animationIndex != lastAnimationIndex)
            {
                frameIndex = 0;
            }
        }

        private void AdvanceFrameIndex(Sprite[] sprites)
        {
            frameIndex++;
            if(frameIndex > sprites.Length - 1) frameIndex = 0;
            renderer.sprite = sprites[frameIndex];
            lastChangeFrame = Time.time;
        }
    }
}
