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

            if(choices.Count > 0)
            {
                int choiceIndex = Random.Range(0, choices.Count);
                var choice = choices[choiceIndex];

                SetFlashing(choice == "leftHand", choice == "rightHand", choice == "leftLeg", choice == "rightLeg");
            }
        }
    }
}
