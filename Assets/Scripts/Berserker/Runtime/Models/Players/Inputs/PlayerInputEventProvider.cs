using System;
using Common.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Berserker.Models.Players.Inputs
{
    /// <summary>
    /// プレイヤーの入力イベントを提供する
    /// </summary>
    public sealed class PlayerInputEventProvider : IPlayerInputEventProvider, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private InputActionAsset _inputActionAsset;

        /// <summary>
        /// 移動の入力値
        /// </summary>
        public IReadOnlyReactiveProperty<Vector2> Move { get; private set; }

        /// <summary>
        /// 視点の入力値
        /// </summary>
        public IReadOnlyReactiveProperty<Vector2> View { get; private set; }

        /// <summary>
        /// ジャンプの入力値
        /// </summary>
        public IReadOnlyReactiveProperty<bool> Jump { get; private set; }
        
        /// <summary>
        /// 入力デバイスがマウスかどうか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsMouseDevice { get; private set; }
        
        public void Initialize()
        {
            SetupInputActions();
        }

        private void SetupInputActions()
        {
            _inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            var playerActionMap = _inputActionAsset.AddActionMap("Player");

            // 移動(Move)
            {
                var moveAction = playerActionMap.AddAction("Move", InputActionType.PassThrough);
                moveAction.AddBinding("<Gamepad>/leftStick");
                moveAction.AddCompositeBinding("DPad")
                    .With("Up", "<Keyboard>/w")
                    .With("Down", "<Keyboard>/s")
                    .With("Left", "<Keyboard>/a")
                    .With("Right", "<Keyboard>/d");

                Move = moveAction.OnPerformedAsObservable<Vector2>()
                    .ToReadOnlyReactiveProperty()
                    .AddTo(_disposables);
            }
            
            // 視点(View)
            {
                var viewAction = playerActionMap.AddAction("View", InputActionType.PassThrough);
                viewAction.AddBinding("<Gamepad>/rightStick", processors: "scaleVector2(x=10,y=10)");
                viewAction.AddBinding("<Mouse>/delta");
                
                View = viewAction.OnPerformedAsObservable<Vector2>()
                    .ToReadOnlyReactiveProperty()
                    .AddTo(_disposables);
            }
            
            // ジャンプ(Jump)
            {
                var jumpAction = playerActionMap.AddAction("Jump", InputActionType.PassThrough);
                jumpAction.AddBinding("<Gamepad>/buttonSouth");
                jumpAction.AddBinding("<Keyboard>/space");
                
                Jump = jumpAction.OnPerformedAsObservable<bool>()
                    .ToReadOnlyReactiveProperty()
                    .AddTo(_disposables);
            }

            // 入力デバイスがマウスかどうか
            {
                IsMouseDevice = Observable.FromEvent<Action<object, InputActionChange>, (object Action, InputActionChange Change)>(
                        action => (x, y) => action((x, y)),
                        action => InputSystem.onActionChange += action,
                        action => InputSystem.onActionChange -= action)
                    .Where(x => x.Change == InputActionChange.ActionPerformed)
                    .Select(x => ((InputAction)x.Action).activeControl.device.displayName == "Mouse")
                    .ToReadOnlyReactiveProperty()
                    .AddTo(_disposables);
            }
            
            _inputActionAsset.Enable();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}