using UnityEngine;

namespace MudOverload.Game.Boss {
    public class BossAnimator : MonoBehaviour
    {
        [Header("Others")]
        [SerializeField]
        private float animationLerpSpeed = 1;
        [SerializeField]
        private float animationSpeed = 7.5f;
        [SerializeField]
        private float movementSpeed = 5f;
        [SerializeField]
        private float displaySpeedLerp = 10;
        [SerializeField]
        private GameObject groundHitEffect;

        #region Inputs
        [Header("Inputs")]
        [SerializeField]
        private Transform upper;
        [SerializeField]
        private Transform head;
        [SerializeField]
        private Transform chest;
        [SerializeField]
        private Transform leftShoulder;
        [SerializeField]
        private Transform leftArm;
        [SerializeField]
        private Transform leftHand;
        [SerializeField]
        private Transform rightShoulder;
        [SerializeField]
        private LineRenderer rightShoulderLineRenderer;
        [SerializeField]
        private Transform rightArm;
        [SerializeField]
        private Transform rightHand;
        [SerializeField]
        private Transform pp;
        [SerializeField]
        private Transform leftKnee;
        [SerializeField]
        private Transform leftFoot;
        [SerializeField]
        private Transform rightKnee;
        [SerializeField]
        private Transform rightFoot;
        #endregion

        private SpriteRenderer leftFootRenderer;
        private SpriteRenderer rightFootRenderer;
        private SpriteRenderer headRenderer;
        private SpriteRenderer chestRenderer;

        private TransformKeeper[] keepers;

        private Vector2 lastPosition;

        private float displaySpeed = 0;

        [HideInInspector]
        public float sleep = 0;

        [HideInInspector]
        private bool punchHitGround;

        //Firing animation data
        public enum FiringStage
        {
            NONE,
            AIMING,
            SHOOTING,
        }
        [HideInInspector]
        public FiringStage firingStage = FiringStage.NONE;
        [HideInInspector]
        public Vector2 firingTarget = new Vector2(7.5f, -5);

        private void Awake()
        {
            keepers = GetComponentsInChildren<TransformKeeper>();

            leftFootRenderer = leftFoot.GetComponent<SpriteRenderer>();
            rightFootRenderer = rightFoot.GetComponent<SpriteRenderer>();
            headRenderer = head.GetComponent<SpriteRenderer>();
            chestRenderer = chest.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            //calculating movement speed
            var positionDifference = (Vector2)transform.position - lastPosition;

            //lerping display movement speed to prevent jitterness
            displaySpeed = Mathf.Lerp(displaySpeed, positionDifference.x, displaySpeedLerp * Time.deltaTime);

            //flipping what needs to be flipped depending on which direction we are moving
            leftFootRenderer.flipX = positionDifference.x < 0;
            rightFootRenderer.flipX = positionDifference.x < 0;
            headRenderer.flipX = positionDifference.x < 0;
            chestRenderer.flipX = positionDifference.x < 0;

            //bobbing up and down
            upper.localPosition = new Vector3(Mathf.Sin(Time.time * animationSpeed) * (0.01f * (1 - sleep)), Mathf.Cos(Time.time * animationSpeed) * (0.05f * (1 - sleep)));

            //head facing down when sleeping
            head.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(head.localEulerAngles.z, sleep * (headRenderer.flipX ? 45 : -45), animationLerpSpeed * Time.deltaTime));

            //moving knees and legs when we are moving
            rightKnee.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * animationSpeed) * 45 * displaySpeed * movementSpeed);
            rightFoot.rotation = Quaternion.identity;
            leftKnee.localEulerAngles = new Vector3(0, 0, Mathf.Cos(Time.time * animationSpeed) * 45 * displaySpeed * movementSpeed);
            leftFoot.rotation = Quaternion.identity;

            //storing lastPosition for figuring out movement speed later on
            lastPosition = transform.position;

            if (firingStage == FiringStage.NONE)
            {
                rightShoulder.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rightShoulder.localEulerAngles.z, 0, animationLerpSpeed * Time.deltaTime));
                rightArm.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rightArm.localEulerAngles.z, 0, animationLerpSpeed * Time.deltaTime));
                rightHand.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rightHand.localEulerAngles.z, 0, animationLerpSpeed * Time.deltaTime));

                ResetPunchHand();
            }
            else
            {
                var direction = (firingTarget - (Vector2)rightShoulder.position).normalized;
                rightShoulder.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rightShoulder.localEulerAngles.z, Mathf.Atan2(direction.x, direction.y) * -Mathf.Rad2Deg + 180, animationLerpSpeed * Time.deltaTime));

                if (firingStage == FiringStage.SHOOTING)
                {
                    rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, rightArm.InverseTransformPoint(firingTarget), animationLerpSpeed * Time.deltaTime);
                    rightArm.localPosition = Vector3.Lerp(rightHand.localPosition, rightShoulder.InverseTransformPoint(((Vector2)rightShoulder.position + firingTarget) / 2), animationLerpSpeed * Time.deltaTime);

                    if(Vector2.Distance(rightHand.position, firingTarget) < 0.25 && !punchHitGround)
                    {
                        punchHitGround = true;

                        var effect = Instantiate(groundHitEffect);
                        effect.transform.position = firingTarget;
                    }
                }
                else
                {
                    ResetPunchHand();
                    punchHitGround = false;
                }
            }

            rightShoulderLineRenderer.SetPositions(new Vector3[] {
                rightShoulder.position,
                rightHand.TransformPoint(new Vector3(0,0.5f,0)),
            });
        }

        private void ResetPunchHand()
        {
            rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, new Vector3(0.07000017f, -1.28f), animationLerpSpeed * Time.deltaTime);
            rightArm.localPosition = Vector3.Lerp(rightHand.localPosition, new Vector3(0.02999997f, -1.48f), animationLerpSpeed * Time.deltaTime);
        }
    }
}
