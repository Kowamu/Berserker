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
    }
}