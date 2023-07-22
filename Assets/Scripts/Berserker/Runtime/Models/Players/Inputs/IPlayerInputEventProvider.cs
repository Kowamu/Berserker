using UniRx;
using UnityEngine;

namespace Berserker.Models.Players.Inputs
{
    /// <summary>
    /// プレイヤーの入力イベントを提供するインターフェース
    /// </summary>
    public interface IPlayerInputEventProvider
    {
        /// <summary>
        /// 移動の入力値
        /// </summary>
        IReadOnlyReactiveProperty<Vector2> Move { get; }
        
        /// <summary>
        /// 視点の入力値
        /// </summary>
        IReadOnlyReactiveProperty<Vector2> View { get; }

        /// <summary>
        /// ジャンプの入力値
        /// </summary>
        IReadOnlyReactiveProperty<bool> Jump { get; }
        
        /// <summary>
        /// 入力デバイスがマウスかどうか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsMouseDevice { get; }
    }
}