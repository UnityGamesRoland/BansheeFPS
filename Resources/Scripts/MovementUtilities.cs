namespace BansheeEngine
{
	public static class MovementUtilities
	{
		public static float GetHorizontalInput()
        {
            //Virtual axis for the horizontal (left-right) movement.
            if (Input.IsButtonHeld(ButtonCode.D, 0)) return 1;
            else if (Input.IsButtonHeld(ButtonCode.A, 0)) return -1;
            else return 0;
        }

        public static float GetVerticalInput()
        {
            //Virtual axis for the vertical (forward-backward) movement.
            if (Input.IsButtonHeld(ButtonCode.W, 0)) return 1;
            else if (Input.IsButtonHeld(ButtonCode.S, 0)) return -1;
            else return 0;
        }

        public static float CalculateJumpVelocity(float gravity, float jumpHeight)
        {
            //Calculates the jump height based on the given paramaters.
            return MathEx.Sqrt(2 * gravity * jumpHeight);
        }
    }
}