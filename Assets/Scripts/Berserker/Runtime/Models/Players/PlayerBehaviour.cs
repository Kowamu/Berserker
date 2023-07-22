using Berserker.Models.Players.Inputs;
using Common.Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace Berserker.Models.Players
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        private PlayerCore _core;
        private PlayerStatus _status;
        private Transform _transform;
        private CapsuleCollider _collider;
        private Rigidbody _rigidbody;

        /// <summary>
        /// プレイヤーのコア
        /// </summary>
        protected PlayerCore Core => _core;
        
        /// <summary>
        /// プレイヤーのステータス
        /// </summary>
        protected PlayerStatus Status => _status;
        
        /// <summary>
        /// プレイヤーのトランスフォーム
        /// </summary>
        protected Transform Transform => _transform;
        
        /// <summary>
        /// プレイヤーのコライダー
        /// </summary>
        protected CapsuleCollider Collider => _collider;
        
        /// <summary>
        /// プレイヤーのRigidbody
        /// </summary>
        protected Rigidbody Rigidbody => _rigidbody;
        
        /// <summary>
        /// プレイヤーの入力イベント
        /// </summary>
        protected IPlayerInputEventProvider InputEventProvider { get; private set; }

        [Inject]
        public void Construct(IPlayerInputEventProvider inputEventProvider)
        {
            InputEventProvider = inputEventProvider;
        }

        protected virtual void Awake()
        {
            Assert.IsTrue(this.TryGetComponentInChildren(out _core, true), $"{nameof(PlayerCore)}がアタッチされていません。");
            Assert.IsTrue(this.TryGetComponentInChildren(out _status, true), $"{nameof(PlayerStatus)}がアタッチされていません。");
            _transform = transform;
            Assert.IsTrue(this.TryGetComponentInChildren(out _collider, true), $"{nameof(CapsuleCollider)}がアタッチされていません。");
            Assert.IsTrue(this.TryGetComponentInChildren(out _rigidbody, true), $"{nameof(Rigidbody)}がアタッチされていません。");
            Assert.IsNotNull(InputEventProvider, $"{nameof(InputEventProvider)}が注入されていません。");
        }
    }
}