using UnityEngine;

namespace MudOverload.Game.Boos
{
	public class BossAnimator : MonoBehaviour
	{
		public enum CurrentAnimation
        {
			IDLE,
        }

		[Header("Others")]
		[SerializeField]
		private float speed = 2f;

		#region Inputs
		[Header("Inputs")]
		[SerializeField]
		private Transform upper;
		[SerializeField]
		private Transform head;
		[SerializeField]
		private Transform leftShoulder;
		[SerializeField]
		private Transform leftArm;
		[SerializeField]
		private Transform leftHand;
		[SerializeField]
		private Transform rightShoulder;
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

		private TransformKeeper[] keepers;

        private void Awake()
        {
			keepers = GetComponentsInChildren<TransformKeeper>();

		}

        private void Update()
        {
			var animation = CurrentAnimation.IDLE;

			if(animation == CurrentAnimation.IDLE)
            {
				upper.localPosition = new Vector3(Mathf.Sin(Time.time * speed) * 0.025f, Mathf.Cos(Time.time * speed) * 0.25f);
            }
		}

		private void ResetAll()
        {
			foreach(var keeper in keepers)
            {
				keeper.Reset();
            }
        }
    }
}
