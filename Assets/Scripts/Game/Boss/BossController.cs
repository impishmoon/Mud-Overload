using MudOverload.Game.Player;
using UnityEngine;

namespace MudOverload.Game.Boss
{
    public class BossController : MonoBehaviour
    {
        private static BossController Singleton;

        public enum Limbs
        {
            LEFT_HAND,
            LEFT_LEG,
            RIGHT_LEG,
        }

        public static void KillLimb(Limbs limb)
        {
            if (Singleton == null) return;

            if (limb == Limbs.LEFT_HAND)
            {
                Singleton.hasLeftHand = false;
            }
            else if (limb == Limbs.LEFT_LEG)
            {
                Singleton.hasLeftLeg = false;
            }
            else if (limb == Limbs.RIGHT_LEG)
            {
                Singleton.hasRightLeg = false;
            }

            Singleton.animator.DestroyLimb(limb);
            Singleton.nextAttackStageChange = Time.time;

            if (!Singleton.hasLeftHand && !Singleton.hasLeftLeg && !Singleton.hasRightLeg)
            {
                Singleton.isDead = true;
                print("dead");
                //TODO: trigger victory;
            }
        }

        public static bool HasLimb(Limbs limb)
        {
            if (Singleton == null) return false;

            if (limb == Limbs.LEFT_LEG)
            {
                return Singleton.hasLeftLeg;
            }
            else if (limb == Limbs.LEFT_HAND)
            {
                return Singleton.hasLeftHand;
            }
            else if (limb == Limbs.RIGHT_LEG)
            {
                return Singleton.hasRightLeg;
            }

            return false;
        }

        [SerializeField]
        private LayerMask onlyTiles;
        [SerializeField]
        private float aimSpeed = 6;

        [SerializeField]
        private BossBatteryBarController batteryBar1;
        [SerializeField]
        private BossBatteryBarController batteryBar2;
        [SerializeField]
        private BossBatteryBarController batteryBar3;

        private BossAnimator animator;
        private FlashingSpritesManager flashingSpritesManager;

        private int attackCounter = 3;
        private BossAnimator.FiringStage attackStage = BossAnimator.FiringStage.NONE;
        private float nextAttackStageChange;

        private Vector2 lerpedAttackPosition;

        private bool hasLeftHand = true;
        private bool hasLeftLeg = true;
        private bool hasRightLeg = true;

        private bool isDead = false;

        private void Awake()
        {
            Singleton = this;

            animator = GetComponent<BossAnimator>();
            flashingSpritesManager = GetComponent<FlashingSpritesManager>();

            nextAttackStageChange = Time.time + 2;
        }

        private void Update()
        {
            var playerTarget = Physics2D.Raycast(PlayerController.GetPosition(), Vector2.down, 100f, onlyTiles);

            if (Time.time > nextAttackStageChange)
            {
                if (!isDead)
                {
                    if (attackStage == BossAnimator.FiringStage.NONE)
                    {
                        if (attackCounter < 3)
                        {
                            attackCounter++;
                            UpdateBatteryDisplay();

                            nextAttackStageChange = Time.time + 1.25f;
                        }
                        else
                        {
                            flashingSpritesManager.SetFlashing(false, false, false, false);

                            lerpedAttackPosition = playerTarget.point;

                            nextAttackStageChange = Time.time + 3;
                            SetAttackStage(BossAnimator.FiringStage.AIMING);
                        }
                    }
                    else if (attackStage == BossAnimator.FiringStage.AIMING)
                    {
                        nextAttackStageChange = Time.time + 0.5f + Vector2.Distance(transform.position, playerTarget.point) / 50f;
                        SetAttackStage(BossAnimator.FiringStage.SHOOTING);

                        attackCounter--;
                        UpdateBatteryDisplay();
                    }
                    else if (attackStage == BossAnimator.FiringStage.SHOOTING)
                    {
                        if (attackCounter == 0)
                        {
                            flashingSpritesManager.SetRandom(hasLeftHand, false, hasLeftLeg, hasRightLeg);

                            nextAttackStageChange = Time.time + 3f;
                            SetAttackStage(BossAnimator.FiringStage.NONE);
                        }
                        else
                        {
                            animator.firingTarget = playerTarget.point;

                            nextAttackStageChange = Time.time + 1.5f;
                            SetAttackStage(BossAnimator.FiringStage.AIMING);
                        }
                    }
                }
                else
                {
                    attackCounter = 0;
                    UpdateBatteryDisplay();
                }
            }

            if(attackStage == BossAnimator.FiringStage.AIMING)
            {
                lerpedAttackPosition = Vector2.MoveTowards(lerpedAttackPosition, playerTarget.point, aimSpeed * Time.deltaTime);

                animator.firingTarget = lerpedAttackPosition;
            }
        }

        private void SetAttackStage(BossAnimator.FiringStage stage)
        {
            attackStage = stage;
            animator.firingStage = stage;
        }

        private void UpdateBatteryDisplay()
        {
            batteryBar1.SetVisible(attackCounter >= 1);
            batteryBar2.SetVisible(attackCounter >= 2);
            batteryBar3.SetVisible(attackCounter >= 3);
        }
    }
}
