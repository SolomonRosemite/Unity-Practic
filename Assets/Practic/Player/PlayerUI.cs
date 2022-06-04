using UnityEngine;

namespace Practic.Player
{
    public class PlayerUI : MonoBehaviour
    {
        public bool MouseIsLooked { get; private set; }
    
        private void Start() => UpdateLockMouse(true);

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape)) 
                UpdateLockMouse(false);
        }

        public void OnClickBackIntoGame() => UpdateLockMouse(true);
    
        private void UpdateLockMouse(bool lockMouse)
        {
            Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockMouse;
            MouseIsLooked = lockMouse;
        }
    }
}
