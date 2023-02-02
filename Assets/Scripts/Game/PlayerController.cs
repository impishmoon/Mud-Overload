using UnityEngine;

namespace MudOverload.Game
{
	public class PlayerController : MonoBehaviour
	{
        [SerializeField]
        private float speed;
        
		private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontalVelocity = 0;

            if (Input.GetKey(KeyCode.A))
            {
                horizontalVelocity += -speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalVelocity += speed;
            }

            rigidbody.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, rigidbody.velocity.y);

        }
    }
}
