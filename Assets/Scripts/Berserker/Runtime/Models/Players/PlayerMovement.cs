using System;
using Common.Maths;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
        
        [SerializeField]
        private MovementParameters _params;

        [SerializeField]
        private float _gravity = 10f;

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
            ControlVelocity(moveInput, deltaTime);
            Transform.position += Status.Velocity.Value * deltaTime;
        }

        /// <summary>
        /// 移動速度を制御する
        /// </summary>
        /// <param name="moveInput">移動入力値</param>
        /// <param name="deltaTime">経過時間</param>
        private void ControlVelocity(Vector2 moveInput, float deltaTime)
        {
            var velocity = Status.Velocity.Value;
            var targetVelocity = Quaternion.LookRotation(Status.Forward.Value) * (moveInput.XZToXYZ() * _params.MaxSpeed);
            
            // 速度を計算して代入するローカルメソッド
            void CalculateVelocity(float acceleration)
            {
                velocity += (targetVelocity - velocity) * (acceleration * deltaTime);
                velocity.ClampMagnitude(_params.MaxSpeed);
            }
            
            if (moveInput.IsAlmostZero())
            {
                // 移動入力がない場合は停止
                CalculateVelocity(_params.Brake);
            } 
            else if (Vector3.Dot(velocity, moveInput) < 0f)
            {
                // 進行方向と移動入力が逆の場合は折り返し
                CalculateVelocity(_params.Turnaround);
            }
            else if (velocity.sqrMagnitude <= targetVelocity.sqrMagnitude)
            {
                // 速度が入力値より小さい場合は加速
                CalculateVelocity(_params.Acceleration);
            }
            else
            {
                // 速度が入力値より大きい場合は減速
                CalculateVelocity(_params.Deceleration);
            }

            Status.Velocity.Value = velocity;
        }
    }
}