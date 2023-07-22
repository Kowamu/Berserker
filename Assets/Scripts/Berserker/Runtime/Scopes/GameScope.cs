using Berserker.Models.Players;
using Berserker.Models.Players.Inputs;
using VContainer;
using VContainer.Unity;

namespace Berserker.Scopes
{
    /// <summary>
    /// ゲームシーンのスコープ
    /// </summary>
    public sealed class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlayerInputEventProvider>().As<IPlayerInputEventProvider>();
            builder.RegisterComponentInHierarchy<PlayerStatus>().As<IPlayerStatus>();
        }
    }    
}
