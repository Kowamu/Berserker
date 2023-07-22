using System;
using UnityEngine;

namespace Berserker.Models.Players
{
    public interface IPlayerStatus
    {
        /// <summary>
        /// 移動速度が変更されたことを通知する
        /// </summary>
        IObservable<Vector3> OnVelocityChangeAsObservable();

        /// <summary>
        /// 移動方向が変更されたことを通知する
        /// </summary>
        IObservable<Vector3> OnForwardChangeAsObservable();
        
        /// <summary>
        /// 視線の向きが変更されたことを通知する
        /// </summary>
        IObservable<Vector3> OnViewChangeAsObservable();
    }
}