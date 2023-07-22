using Common.Maths;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Berserker.Models.Players
{
    public class PlayerView : PlayerBehaviour
    {
        [SerializeField]
        private Transform _cameraOrientation;
        
        [SerializeField]
        private Vector2 _sensitivity = new(1f, 1f);

        private float pitch;
        private float yaw;
        
        private void Start()
        {
            var eulerAngle = Transform.eulerAngles;
            yaw = eulerAngle.y;
            pitch = eulerAngle.x;
            
            this.LateUpdateAsObservable()
                .WithLatestFrom(InputEventProvider.View, (_, view) => view)
                .Subscribe(view => View(view, 1f))
                .AddTo(this);
        }
        
        private void View(Vector2 viewInput, float deltaTime)
        {
            // マウスデバイス以外の場合はデルタタイムを無視する
            deltaTime = InputEventProvider.IsMouseDevice.Value ? deltaTime : 1f;
            
            // 入力値を回転に加算する
            pitch = Mathf.Clamp(pitch - viewInput.y * _sensitivity.y * deltaTime, -89.99f, 89.99f);
            yaw = Math.Repeat(yaw + viewInput.x * _sensitivity.x * deltaTime, 360f);

            var yawRotation = Quaternion.Euler(0f, yaw, 0f);
            var viewRotation = Quaternion.Euler(pitch, yaw, 0f);

            Transform.rotation = yawRotation;
            _cameraOrientation.rotation = viewRotation;
            
            Status.Forward.Value = yawRotation * Vector3.forward;
            Status.ViewDirection.Value = viewRotation * Vector3.forward;
        }
    }
}