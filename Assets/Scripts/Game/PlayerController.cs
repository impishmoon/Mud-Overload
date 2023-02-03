using UnityEngine;

namespace MudOverload.Game
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController Singleton;

        public static Vector2 GetMiningEffectTargetPosition()
        {
            if (Singleton == null) return new Vector2();

            return Singleton.rigidbody.position;
        }

        [SerializeField]
        private LayerMask everythingExceptPlayerMask;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float miningSpeed = 1f;

        private new Rigidbody2D rigidbody;

        private bool onGround = false;
        private bool walkingRight = false;

        private float miningStart = 0f;
        private Vector2 miningPosition;

        private void Awake()
        {
            Singleton = this;

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            #region Movement
            float horizontalVelocity = 0;
            bool jump = false;

            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
            }

            if (!IsMining())
            {
                if (Input.GetKey(KeyCode.A))
                {
                    var hit = Physics2D.Raycast(rigidbody.position, Vector2.left, 0.55f, everythingExceptPlayerMask);
                    if (!hit.collider)
                    {
                        horizontalVelocity += -speed;
                        walkingRight = false;
                    }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    var hit = Physics2D.Raycast(rigidbody.position, Vector2.right, 0.55f, everythingExceptPlayerMask);
                    if (!hit.collider)
                    {
                        horizontalVelocity += speed;
                        walkingRight = true;
                    }
                }
            }

            rigidbody.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, jump ? speed * Time.fixedDeltaTime : rigidbody.velocity.y);
            #endregion

            #region Mining
            var miningSource = (Vector3)rigidbody.position + new Vector3(0, 0.5f);

            var mousePosition = CameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);

            var miningHit = Physics2D.Raycast(miningSource, mousePosition - miningSource, 3f, everythingExceptPlayerMask);
            if (miningHit.collider)
            {
                var finalMiningHitPoint = miningHit.point + miningHit.normal * -0.1f;

                MiningPreviewController.SetPosition(IsMining() ? miningPosition : finalMiningHitPoint);

                if (Input.GetMouseButton(0) && !IsMining())
                {
                    miningStart = Time.time;

                    miningPosition = finalMiningHitPoint;

                    MiningController.StartMining(miningPosition);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    StopMining();
                }

                if (IsMining())
                {
                    var progress = GetMiningProgress();

                    MiningController.SetProgress(progress);

                    if (progress >= 1)
                    {
                        StopMining();
                        TerrainController.MineTile(miningPosition);
                    }
                }
            }
            else
            {
                StopMining();
                MiningPreviewController.Hide();
            }
            #endregion
        }

        private void FixedUpdate()
        {
            //detecting if we are touching the ground
            var floorHit = Physics2D.Raycast(rigidbody.position, Vector2.down, 1.015f, everythingExceptPlayerMask);
            onGround = floorHit.collider;

            if (onGround)
            {
                //detecting if we should auto-step over tile
                var sideHit = Physics2D.Raycast(rigidbody.position + new Vector2(0, -0.25f), walkingRight ? Vector2.right : Vector2.left, 5f, everythingExceptPlayerMask);
                if (sideHit.collider && sideHit.distance < 0.6f)
                {
                    var source = rigidbody.position + new Vector2(walkingRight ? 0.6f : -0.6f, 0.25f);
                    var upHit = Physics2D.Raycast(source, Vector2.up, 1f, everythingExceptPlayerMask);
                    var downHit = Physics2D.Raycast(source, Vector2.down, 5f, everythingExceptPlayerMask);

                    if (!upHit.collider && downHit.collider && downHit.distance > 0 && downHit.distance < 0.27f)
                    {
                        rigidbody.position += new Vector2(walkingRight ? 0.25f : -0.25f, 1f);
                    }
                }
            }
        }

        private void StopMining()
        {
            miningStart = 0f;

            MiningController.StopMining();
        }

        private bool IsMining()
        {
            return miningStart != 0f;
        }

        private float GetMiningProgress()
        {
            return (Time.time - miningStart) / miningSpeed;
        }
    }
}
