using MudOverload.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MudOverload.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController Singleton;

        public static Vector2 GetMiningEffectTargetPosition()
        {
            if (Singleton == null) return new Vector2();

            return Singleton.rigidbody.position;
        }

        public static Vector2 GetPosition()
        {
            if (Singleton == null) return new Vector2();

            return Singleton.rigidbody.position;
        }

        [SerializeField]
        private LayerMask onlyTiles;

        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpingForce;
        [Header("this needs to be the same as X of collider's size")]
        [SerializeField]
        private float playerWidth = 1;

        [SerializeField]
        private float miningSpeed = 1f;

        [SerializeField]
        private int maxHeldTiles = 1;

        private new Rigidbody2D rigidbody;

        private bool onGround = false;
        private bool walkingRight = false;
        private bool isWalking = false;

        private float miningStart = 0f;
        private Vector2 miningPosition;
        private List<string> heldTiles = new List<string>();

        private void Awake()
        {
            Singleton = this;

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsUIOpen.AreThereUIUsers()) return;

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
                    if (!IsBlockedBySide(false))
                    {
                        horizontalVelocity += -speed;
                        walkingRight = false;
                    }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if (!IsBlockedBySide(true))
                    {
                        horizontalVelocity += speed;
                        walkingRight = true;
                    }
                }
            }

            isWalking = horizontalVelocity != 0;

            rigidbody.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, jump ? jumpingForce * Time.fixedDeltaTime : rigidbody.velocity.y);
            #endregion

            #region Mining
            var miningSource = (Vector3)rigidbody.position + new Vector3(0, 0.5f);

            var mousePosition = CameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);

            var miningHit = Physics2D.Raycast(miningSource, mousePosition - miningSource, 3f, onlyTiles);
            if (miningHit.collider && !PrizeController.GetIsHovering() && SceneManager.GetActiveScene().buildIndex == 0)
            {
                var finalMiningHitPoint = miningHit.point + miningHit.normal * -0.1f;

                MiningPreviewController.SetPosition(IsMining() ? miningPosition : finalMiningHitPoint);

                if (Input.GetMouseButtonDown(1))
                {
                    var position = miningHit.point + miningHit.normal * 0.5f;

                    var tile = TerrainController.GetTile(position);

                    if (heldTiles.Count > 0)
                    {
                        var lastHeldTile = heldTiles[heldTiles.Count - 1];

                        TerrainController.SetTile(position, lastHeldTile);

                        heldTiles.RemoveAt(heldTiles.Count - 1);
                        PlayerHoldingTilesController.RemoveLastTile();
                    }
                }

                if (Input.GetMouseButton(0) && !IsMining() && heldTiles.Count < maxHeldTiles)
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
                        var tileName = TerrainController.MineTile(miningPosition);
                        heldTiles.Add(tileName);
                    }
                }
            }
            else
            {
                StopMining();
                MiningPreviewController.Hide();
            }
            #endregion

            #region Clearing held tiles onto trash portal
            var portalRegion = TrashPortalController.GetPortalLocation();
            var distance = Vector2.Distance(rigidbody.position, portalRegion);
            if (distance < 3)
            {
                heldTiles.Clear();
                PlayerHoldingTilesController.ClearTiles();
            }
            #endregion
        }

        private void FixedUpdate()
        {
            //detecting if we are touching the ground
            var floorHit = Physics2D.Raycast(rigidbody.position, Vector2.down, 1.5f, onlyTiles);
            onGround = floorHit.collider;

            if (onGround && isWalking)
            {
                //detecting if we should auto-step over tile

                var topHit = Physics2D.Raycast(rigidbody.position + new Vector2(0, 0.5f), walkingRight ? Vector2.right : Vector2.left, 1f, onlyTiles);
                var bottomHit = Physics2D.Raycast(rigidbody.position + new Vector2(0, -0.5f), walkingRight ? Vector2.right : Vector2.left, 1f, onlyTiles);

                if (!topHit.collider && bottomHit.collider)
                {
                    var heightCheck = Physics2D.Raycast(rigidbody.position + new Vector2(walkingRight ? 1f : -1f, 0.75f), Vector2.up, 1f, onlyTiles);
                    if (!heightCheck.collider)
                    {
                        rigidbody.position += new Vector2(walkingRight ? 0.35f : -0.35f, 1f);
                    }
                }
            }

            if(TerrainController.GetTile(rigidbody.position + new Vector2(0,-0.5f)) != null)
            {
                rigidbody.position += new Vector2(0, 1f);
            }
        }

        private bool IsBlockedBySide(bool right)
        {
            var top = Physics2D.Raycast(rigidbody.position + new Vector2(0, 0.75f), right ? Vector2.right : Vector2.left, playerWidth / 2 + 0.025f, onlyTiles);
            var bottom = Physics2D.Raycast(rigidbody.position - new Vector2(0, 0.975f), right ? Vector2.right : Vector2.left, playerWidth / 2 + 0.025f, onlyTiles);

            return top.collider || bottom.collider;
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
