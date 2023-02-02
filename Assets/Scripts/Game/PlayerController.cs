using UnityEngine;

namespace MudOverload.Game
{
	public class PlayerController : MonoBehaviour
	{
        [SerializeField]
        private LayerMask everythingExceptPlayerMask;

        [SerializeField]
        private float speed;
        
		private new Rigidbody2D rigidbody;

        private bool onGround = false;

        private bool walkingRight = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontalVelocity = 0;
            bool jump = false;

            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                horizontalVelocity += -speed;
                walkingRight = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalVelocity += speed;
                walkingRight = true;
            }

            rigidbody.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, jump ? speed * Time.fixedDeltaTime : rigidbody.velocity.y);

        }

        private void FixedUpdate()
        {
            //detecting if we are touching the ground
            var floorHit = Physics2D.Raycast(rigidbody.position, Vector2.down, 5f, everythingExceptPlayerMask);
            onGround = floorHit.distance <= 1.015f;

            //detecting if we should auto-step over tile
            var sideHit = Physics2D.Raycast(rigidbody.position + new Vector2(0,-0.25f), walkingRight ? Vector2.right : Vector2.left, 5f, everythingExceptPlayerMask);
            Debug.DrawLine(rigidbody.position + new Vector2(0, -0.25f), sideHit.point, Color.red);
            if(sideHit.distance < 0.6f)
            {
                var source = rigidbody.position + new Vector2(walkingRight ? 0.6f : -0.6f, 0.25f);
                var downHit = Physics2D.Raycast(source, Vector2.down, 5f, everythingExceptPlayerMask);
                Debug.DrawLine(source, downHit.point, Color.blue);
                print(downHit.distance);

                if(downHit.distance < 0.27f)
                {
                    rigidbody.position += new Vector2(0, 0.5f);
                }
            }
            /*if (hit)
            {
                Debug.DrawLine(rigidbody.position, hit.point);
            }*/
        }
    }
}
