using System;
using UniRx;
using UnityEngine.InputSystem;

namespace Common.Extensions
{
    /// <summary>
    /// InputActionの拡張メソッドを定義する
    /// </summary>
    public static class InputActionExtensions
    {
        /// <summary>
        /// InputAction.performedをIObservableに変換する
        /// </summary>
        /// <param name="action">入力アクション</param>
        /// <typeparam name="TValue">入力値の型</typeparam>
        /// <returns>入力値のIObservable</returns>
        public static IObservable<TValue> OnPerformedAsObservable<TValue>(this InputAction action)
            where TValue : struct
        {
            return Observable.FromEvent<InputAction.CallbackContext>(
                    h => action.performed += h,
                    h => action.performed -= h)
                .Select(context => context.ReadValue<TValue>());
        }
    }
}