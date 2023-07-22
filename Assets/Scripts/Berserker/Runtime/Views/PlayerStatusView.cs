using Common.Maths;
using TMPro;
using UnityEngine;

namespace Berserker.Runtime.Views
{
    public class PlayerStatusView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _horizontalSpeedText;
        
        [SerializeField]
        private TMP_Text _verticalSpeedText;
        
        private void Start()
        {
            SetVelocity(Vector3.zero);
        }

        public void SetVelocity(Vector3 velocity)
        {
            var horizontalSpeed = velocity.XYZToXZ().magnitude;
            var verticalSpeed   = velocity.y;
            
            _horizontalSpeedText.text = $"HSpeed: {horizontalSpeed:00.00}";
            _verticalSpeedText.text   = $"VSpeed: {verticalSpeed:00.00}";
        }
    }
}