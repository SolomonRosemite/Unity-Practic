using TMPro;
using UnityEngine;

namespace Practic.Common
{
    public class FPSCounter : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;
        
        private void Start() => _textMeshPro = GetComponent<TextMeshProUGUI>();
        
        private void Update() => _textMeshPro.SetText($"FPS:{1.0f / Time.deltaTime}");
    }
}
