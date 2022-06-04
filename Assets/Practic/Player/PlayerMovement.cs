using System;
using UnityEngine;

namespace Practic.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.1f, 100)]
        public float mouseSensitivity = 1;

        public float maxMagnitude = 10;
        public float jumpForce = 45000;
        
        private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, 1.05f);
        
        private Rigidbody _playerRigidbody;
        public Camera playerCamera;
        private PlayerUI _playerUI;

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
            _playerUI = GetComponent<PlayerUI>();
        }

        private void FixedUpdate() => HandlePlayerMovement();

        private void Update() => HandlePlayerCameraMovement();

        private void HandlePlayerCameraMovement()
        {
            if (!_playerUI.MouseIsLooked)
                return;
            
            // Update Pitch
            var (shouldUpdatePitch, pitchDisplacement) = CalculatedMouseDisplacement(Input.GetAxis("Mouse X"));
            if (shouldUpdatePitch)
                PerformRotation(transform,new Vector3(0, pitchDisplacement, 0));
    
            // Update Yaw
            var (shouldUpdateYaw, yawDisplacement) = CalculatedMouseDisplacement(Input.GetAxis("Mouse Y"));
            if (shouldUpdateYaw)
                PerformRotation(playerCamera.transform, -new Vector3(yawDisplacement, 0, 0));
        }

        private void PerformRotation(Transform tf, Vector3 displacement)
        {
            // This is needed to make sure that the player camera does not rotate backwards when looking down or up
            // This will only effect the vertical mouse changes
            var x = (tf.rotation.eulerAngles + displacement).x;
            if (x is not (> 270 or < 90))
                return;
            
            tf.Rotate(displacement);
        }

        private (bool, float) CalculatedMouseDisplacement(float mouseMovement)
        {
            return mouseMovement == 0 
                ? (false, -1) 
                : (true, mouseMovement * mouseSensitivity);
        }

        private void HandlePlayerMovement()
        {
            const float amplifier = 3000 * 20;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) PerformMove(Vector3.forward * amplifier);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) PerformMove(Vector3.left * amplifier);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) PerformMove(Vector3.back * amplifier);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) PerformMove(Vector3.right * amplifier);
            if (Input.GetKey(KeyCode.Space)) PerformJump();
        }

        private void PerformMove(Vector3 force)
        {
            Debug.Log($"X Magnitude: {_playerRigidbody.velocity[0]}");
            Debug.Log($"Z Magnitude: {_playerRigidbody.velocity[2]}");

            // This is not quiet correct. But it will do for now.
            if (Math.Abs(_playerRigidbody.velocity[0]) > maxMagnitude || Math.Abs(_playerRigidbody.velocity[2]) > maxMagnitude)
                return;
            
            _playerRigidbody.AddRelativeForce(force * Time.deltaTime);
        }

        private void PerformJump()
        {
            if (IsGrounded)
                _playerRigidbody.AddForce(Vector3.up * jumpForce * Time.deltaTime);
        }
    }
}
