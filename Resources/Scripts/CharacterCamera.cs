namespace BansheeEngine
{
	public class CharacterCamera : Component
	{
        public Camera characterCamera;
        public float mouseSensitivity = 1.5f;
        
        private float yaw;
        private float pitch;

        private Vector2 currentRotation;
		
		private void OnUpdate()
		{
            //Get the mouse delta.
            Vector2 mouseDelta = new Vector2(Input.GetAxisValue(InputAxis.MouseX), Input.GetAxisValue(InputAxis.MouseY));

            //Update the rotation.
            yaw += mouseDelta.x * mouseSensitivity;
            pitch += mouseDelta.y * mouseSensitivity;

            //Clamp the up-down rotation.
            pitch = MathEx.Clamp(pitch, -90, 90);

            //Rotate the whole character on the Y axis. This is the left-right rotation.
            Quaternion bodyRotation = Quaternion.FromEuler(new Vector3(0f, yaw, 0f));
            SceneObject.LocalRotation = Quaternion.Slerp(SceneObject.LocalRotation, bodyRotation, Time.FrameDelta / 0.015f);

            //Rotate the camera on the X axis. This is the up-down rotation.
            Quaternion camRotation = Quaternion.FromEuler(new Vector3(pitch, 0f, 0f));
            characterCamera.SceneObject.LocalRotation = Quaternion.Slerp(characterCamera.SceneObject.LocalRotation, camRotation, Time.FrameDelta / 0.015f);
        }
		
	}
}