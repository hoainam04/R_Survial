using UnityEngine;

namespace PROJ.Attributes
{
    public class RigidbodyGravityModule
    {
        private readonly Rigidbody rb;
        private readonly float gravity;
        private readonly float maxFallSpeed;

        public RigidbodyGravityModule(Rigidbody rb, float gravity, float maxFallSpeed)
        {
            this.rb = rb;
            this.gravity = gravity;
            this.maxFallSpeed = maxFallSpeed;
        }

        public void FixedTick(float fixedDeltaTime)
        {
            if (rb == null)
                return;

            Vector3 velocity = rb.linearVelocity;
            velocity.y += gravity * fixedDeltaTime;
            velocity.y = Mathf.Max(velocity.y, maxFallSpeed);
            rb.linearVelocity = velocity;
        }
    }
}