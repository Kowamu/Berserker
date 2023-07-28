using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerMovement2D : MonoBehaviour
    {
        [SerializeField]
        private CharacterController2D _controller;
        
        [SerializeField]
        private float _moveSpeed = 10f;
        
        [SerializeField]
        private int _playerIndex = 0;

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => Move((-_controller.Position).normalized))
                .AddTo(this);
        }

        private void Move(Vector2 move)
        {
            _controller.Velocity = move * _moveSpeed;
            _controller.Move(Time.deltaTime);
        }
        
        private Vector2 GetInput()
        {
            var keyboard = Keyboard.current;
            var input = Vector2.zero;

            if (_playerIndex == 0)
            {
                if (keyboard.dKey.isPressed) input.x += 1f;
                if (keyboard.aKey.isPressed) input.x -= 1f;
                if (keyboard.wKey.isPressed) input.y += 1f;
                if (keyboard.sKey.isPressed) input.y -= 1f;
            }
            else if (_playerIndex == 1)
            {
                if (keyboard.rightArrowKey.isPressed) input.x += 1f;
                if (keyboard.leftArrowKey.isPressed) input.x -= 1f;
                if (keyboard.upArrowKey.isPressed) input.y += 1f;
                if (keyboard.downArrowKey.isPressed) input.y -= 1f;
            }

            return input.normalized;
        }
    }
}