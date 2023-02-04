using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class BossCameraTarget : MonoBehaviour
	{
        private void OnEnable()
        {
            BossCameraController.AddNonPlayerTarget(transform);
        }

        private void OnDisable()
        {
            BossCameraController.RemoveNonPlayerTarget(transform);
        }
    }
}
