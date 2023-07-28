using System;
using Common.Maths;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Berserker.Models.Players
{
    /// <summary>
    /// プレイヤーの移動を制御する
    /// </summary>
    public sealed class PlayerMovement : PlayerBehaviour
    {
        [Serializable]
        private class MovementParameters
        {
            [SerializeField]
            public float MaxSpeed = 10f;
            [SerializeField]
            public float Acceleration = 10f;
            [SerializeField]
            public float Deceleration = 10f;
            [SerializeField]
            public float Brake = 10f;
            [SerializeField]
            public float Turnaround = 10f;
        }
        
        public struct RaycastResult
        {
            public bool IsHit;
            public Vector3 Point;
            public Vector3 Normal;
            public float Distance;
        }
        
        [SerializeField]
        private MovementParameters _moveParams;
        
        [SerializeField]
        private float _gravity = 10f;
        
        [SerializeField]
        private float _stepHeight = 0.5f;
        
        [SerializeField]
        private float _stepRadius = 0.5f;
        
        [SerializeField]
        private float _slopeLimit = 45f;
        
        private Vector3 _horizontalVelocity;
        private Vector3 _verticalVelocity;
        private RaycastResult _groundRaycastResult;
        private bool _isGrounded;

        private void Start()
        {
            this.UpdateAsObservable()
                .WithLatestFrom(InputEventProvider.Move, (_, move) => move)
                .Subscribe(move => Move(move, Time.deltaTime))
                .AddTo(this);
        }
        
        /// <summary>
        /// 移動を行う
        /// </summary>
        /// <param name="moveInput">移動入力値</param>
        /// <param name="deltaTime">デルタタイム</param>
        private void Move(Vector2 moveInput, float deltaTime)
        {
            _groundRaycastResult = RaycastToGround();
            _isGrounded = _groundRaycastResult.IsHit && Vector3.Angle(Transform.up, _groundRaycastResult.Normal) <= _slopeLimit;
            Status.Velocity.Value = ControlVelocity(moveInput, deltaTime);
            Transform.position += Status.Velocity.Value * deltaTime;
            StickToGround();
        }

        /// <summary>
        /// 移動速度を制御する
        /// </summary>
        /// <param name="moveInput">移動入力値</param>
        /// <param name="deltaTime">経過時間</param>
        private Vector3 ControlVelocity(Vector2 moveInput, float deltaTime, bool withGravity = true)
        {
            var targetHorizontalVelocity = Transform.localRotation * (moveInput.XZToXYZ() * _moveParams.MaxSpeed);

            if (moveInput.IsAlmostZero())
            {
                // 移動入力がない場合は停止
                CalculateVelocity(_moveParams.Brake);
            } 
            else if (Vector3.Dot(_horizontalVelocity, moveInput) < 0f)
            {
                // 進行方向と移動入力が逆の場合は折り返し
                CalculateVelocity(_moveParams.Turnaround);
            }
            else if (_horizontalVelocity.sqrMagnitude <= targetHorizontalVelocity.sqrMagnitude)
            {
                // 速度が入力値より小さい場合は加速
                CalculateVelocity(_moveParams.Acceleration);
            }
            else
            {
                // 速度が入力値より大きい場合は減速
                CalculateVelocity(_moveParams.Deceleration);
            }

            if (withGravity)
            {
                if (_isGrounded) _verticalVelocity = Vector3.zero;
                else _verticalVelocity += new Vector3(0f, -_gravity, 0f) * deltaTime;
            }

            return Vector3.ProjectOnPlane(_horizontalVelocity, _groundRaycastResult.Normal) + Transform.TransformDirection(_verticalVelocity);

            // 速度を計算して代入するローカルメソッド
            void CalculateVelocity(float acceleration)
            {
                _horizontalVelocity += (targetHorizontalVelocity - _horizontalVelocity) * (acceleration * deltaTime);
            }
        }
        
        private void StickToGround()
        {
            if (!_isGrounded) return;
            
            var up = Transform.up;
            var footPosition = GetFootPosition();
            
            var distance = _stepHeight + _stepRadius + _stepRadius + 0.01f;
            var centerPosition = footPosition + up * distance;
            
            var isHit = Physics.SphereCast(centerPosition, _stepRadius, -up, out var raycastResult, distance);

            if (isHit)
            {
                var dist = Vector3.Dot(-up, raycastResult.point - centerPosition);
                Transform.position += up * (distance - dist + 0.01f + 100000);
            }
        }
        
        private Vector3 GetFootPosition()
        {
            var colliderBottom = Collider.center - Transform.up * (Collider.height * 0.5f);
            return Transform.TransformPoint(colliderBottom);
        }
        
        private RaycastResult RaycastToGround()
        {
            const float threshold = 0.01f;
            var up = Transform.up;
            var origin = GetFootPosition() + up * (_stepRadius + threshold);
            var maxDistance = _stepHeight + _stepRadius + threshold;
            var direction = -up;
            var ray = new Ray(origin, direction);
            
            var isHit = Physics.SphereCast(ray, _stepRadius, out var raycastResult, maxDistance);

            if (isHit)
            {
                Transform.position = raycastResult.point + up * (_stepHeight + 0.01f);
            }
            
            return new RaycastResult
            {
                IsHit = isHit,
                Distance = raycastResult.distance,
                Point = raycastResult.point,
                Normal = raycastResult.normal
            };
        }
    }
}