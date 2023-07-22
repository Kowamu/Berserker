using System;
using UniRx;
using UnityEngine;

namespace Berserker.Models.Players
{
    public class PlayerStatus : PlayerBehaviour, IPlayerStatus
    {
        /// <summary>
        /// 移動速度
        /// </summary>
        public IReactiveProperty<Vector3> Velocity => _velocity;
        private readonly ReactiveProperty<Vector3> _velocity = new();

        /// <summary>
        /// 移動方向
        /// </summary>
        public IReactiveProperty<Vector3> Forward => _forward;
        private readonly ReactiveProperty<Vector3> _forward = new();
        
        /// <summary>
        /// 視線の向き
        /// </summary>
        public IReactiveProperty<Vector3> ViewDirection => _viewDirection;
        private readonly ReactiveProperty<Vector3> _viewDirection = new();

        /// <summary>
        /// 移動速度が変更されたことを通知する
        /// </summary>
        IObservable<Vector3> IPlayerStatus.OnVelocityChangeAsObservable() => _velocity;

        /// <summary>
        /// 移動方向が変更されたことを通知する
        /// </summary>
        IObservable<Vector3> IPlayerStatus.OnForwardChangeAsObservable() => _forward;
        
        /// <summary>
        /// 視線の向きが変更されたことを通知する
        /// </summary>
        IObservable<Vector3> IPlayerStatus.OnViewChangeAsObservable() => _viewDirection;

        private void OnDestroy()
        {
            _velocity.Dispose();
            _forward.Dispose();
            _viewDirection.Dispose();
        }
    }
}