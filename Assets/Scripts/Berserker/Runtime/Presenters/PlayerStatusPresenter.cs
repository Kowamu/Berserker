using System;
using Berserker.Models.Players;
using Berserker.Runtime.Views;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace Berserker.Runtime.Presenters
{
    public class PlayerStatusPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlayerStatusView _view;

        private IPlayerStatus _model;
     
        [Inject]
        private void Construct(IPlayerStatus model)
        {
            _model = model;
        }
        
        private void Awake()
        {
            Assert.IsNotNull(_model, $"{nameof(IPlayerStatus)}が注入されていません。");
        }

        private void Start()
        {
            _model.OnVelocityChangeAsObservable()
                .Subscribe(_view.SetVelocity)
                .AddTo(this);
        }
    }
}