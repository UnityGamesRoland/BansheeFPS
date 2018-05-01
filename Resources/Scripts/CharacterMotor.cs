namespace BansheeEngine
{
    public class CharacterMotor : Component
    {
        public float moveSpeed = 6f;
        public float gravity = 20f;
        public float jumpHeight = 1f;
        public bool canMove = true;

        private Vector2 currentMoveSpeed;
        private float currentGravityAmount;

        private bool isGrounded;
        private CharacterController controller;

        private void OnInitialize()
        {
            //Initialization.
            controller = SceneObject.GetComponent<CharacterController>();
        }

        private void OnUpdate()
        {
            //Get the input direction and normalize it. For some reason the input is inverted, so we take the negative of the axis to get the correct values.
            Vector2 inputDir = new Vector2(-MovementUtilities.GetHorizontalInput(), -MovementUtilities.GetVerticalInput());
            inputDir.Normalize();

            //Zero out the input direction if we cannot move.
            if (!canMove) inputDir = Vector2.Zero;

            //Pass the input direction to the movement handler method, and invoke it.
            Move(inputDir);
        }

        private void Move(Vector2 inputDir)
        {
            //Get the target move speed.
            Vector2 targetMoveSpeed = new Vector2(moveSpeed * inputDir.x, moveSpeed * inputDir.y);

            //Interpolate the move speed to get smooth movement.
            currentMoveSpeed.x = MathEx.Lerp(currentMoveSpeed.x, targetMoveSpeed.x, Time.FrameDelta / 0.04f);
            currentMoveSpeed.y = MathEx.Lerp(currentMoveSpeed.y, targetMoveSpeed.y, Time.FrameDelta / 0.04f);

            //Apply gravity.
            currentGravityAmount -= gravity * Time.FrameDelta;

            //Jumping.
            if (Input.IsButtonDown(ButtonCode.Space) && isGrounded) currentGravityAmount = MovementUtilities.CalculateJumpVelocity(gravity, jumpHeight);

            //Calculate the move velocity.
            Vector3 moveVelocity = SceneObject.Forward * currentMoveSpeed.y + SceneObject.Right * currentMoveSpeed.x + Vector3.YAxis * currentGravityAmount;

            //Move the player and catch the collision flag.
            CharacterCollisionFlag collisionInfo = controller.Move(moveVelocity * Time.FrameDelta);

            //Pass the collision flag to the collision handler method, and invoke it. 
            ProcessCollisionInfo(collisionInfo);
        }

        private void ProcessCollisionInfo(CharacterCollisionFlag collisionInfo)
        {
            //Collision above the player.
            if (collisionInfo == CharacterCollisionFlag.Up)
            {
                //Reset the velocity and update the grounded state.
                currentGravityAmount = 0f;
                isGrounded = false;
            }

            //Collision on the sides.
            else if (collisionInfo == CharacterCollisionFlag.Sides)
            {
                //Update the grounded state. It may look weird, but we have to set it to true because otherwise we cannot jump while we are moving against a wall.
                isGrounded = true;
            }

            //Collision under the player.
            else if (collisionInfo == CharacterCollisionFlag.Down)
            {
                //Reset the velocity and update the grounded state.
                currentGravityAmount = 0f;
                isGrounded = true;
            }

            //No collision. Update the grounded state.
            else isGrounded = false;
        }
    }
}