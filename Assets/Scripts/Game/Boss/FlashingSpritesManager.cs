using System.Collections.Generic;
using UnityEngine;

namespace MudOverload.Game.Boss
{
    public class FlashingSpritesManager : MonoBehaviour
    {
        [SerializeField]
        private SpriteRedFlasher leftShoulder;
        [SerializeField]
        private SpriteRedFlasher leftArm;
        [SerializeField]
        private SpriteRedFlasher leftHand;
        [SerializeField]
        private SpriteRedFlasher rightShoulder;
        [SerializeField]
        private SpriteRedFlasher rightArm;
        [SerializeField]
        private SpriteRedFlasher rightHand;

        [SerializeField]
        private SpriteRedFlasher leftKnee;
        [SerializeField]
        private SpriteRedFlasher leftFoot;
        [SerializeField]
        private SpriteRedFlasher rightKnee;
        [SerializeField]
        private SpriteRedFlasher rightFoot;

        private HighlightSpriteOnHover highlightLeftShoulder;
        private HighlightSpriteOnHover highlightLeftArm;
        private HighlightSpriteOnHover highlightLeftHand;
        private HighlightSpriteOnHover highlightRightShoulder;
        private HighlightSpriteOnHover highlightRightArm;
        private HighlightSpriteOnHover highlightRightHand;

        private HighlightSpriteOnHover highlightLeftKnee;
        private HighlightSpriteOnHover highlightLeftFoot;
        private HighlightSpriteOnHover highlightRightKnee;
        private HighlightSpriteOnHover highlightRightFoot;

        private void Awake()
        {
            highlightLeftShoulder = leftShoulder.GetComponent<HighlightSpriteOnHover>();
            highlightLeftArm = leftArm.GetComponent<HighlightSpriteOnHover>();
            highlightLeftHand = leftHand.GetComponent<HighlightSpriteOnHover>();
            highlightRightShoulder = rightShoulder.GetComponent<HighlightSpriteOnHover>();
            highlightRightArm = rightArm.GetComponent<HighlightSpriteOnHover>();
            highlightRightHand = rightHand.GetComponent<HighlightSpriteOnHover>();

            highlightLeftKnee = leftKnee.GetComponent<HighlightSpriteOnHover>();
            highlightLeftFoot = leftFoot.GetComponent<HighlightSpriteOnHover>();
            highlightRightKnee = rightKnee.GetComponent<HighlightSpriteOnHover>();
            highlightRightFoot = rightFoot.GetComponent<HighlightSpriteOnHover>();
        }

        public void SetFlashing(bool leftHand, bool rightHand, bool leftLeg, bool rightLeg)
        {
            leftShoulder.SetEnabled(leftHand);
            leftArm.SetEnabled(leftHand);
            this.leftHand.SetEnabled(leftHand);

            rightShoulder.SetEnabled(rightHand);
            rightArm.SetEnabled(rightHand);
            this.rightHand.SetEnabled(rightHand);

            leftKnee.SetEnabled(leftLeg);
            leftFoot.SetEnabled(leftLeg);

            rightKnee.SetEnabled(rightLeg);
            rightFoot.SetEnabled(rightLeg);


            highlightLeftShoulder.SetEnabled(leftHand);
            highlightLeftArm.SetEnabled(leftHand);
            highlightLeftHand.SetEnabled(leftHand);

            highlightRightShoulder.SetEnabled(rightHand);
            highlightRightArm.SetEnabled(rightHand);
            highlightRightHand.SetEnabled(rightHand);

            highlightLeftKnee.SetEnabled(leftLeg);
            highlightLeftFoot.SetEnabled(leftLeg);

            highlightRightKnee.SetEnabled(rightLeg);
            highlightRightFoot.SetEnabled(rightLeg);
        }

        public void SetRandom(bool leftHand, bool rightHand, bool leftLeg, bool rightLeg)
        {
            var choices = new List<string>();
            if (leftHand)
            {
                choices.Add("leftHand");
            }
            if (rightHand)
            {
                choices.Add("rightHand");
            }
            if (leftLeg)
            {
                choices.Add("leftLeg");
            }
            if (rightLeg)
            {
                choices.Add("rightLeg");
            }

            if (choices.Count > 0)
            {
                int choiceIndex = Random.Range(0, choices.Count);
                var choice = choices[choiceIndex];

                SetFlashing(choice == "leftHand", choice == "rightHand", choice == "leftLeg", choice == "rightLeg");
            }
        }
    }
}
