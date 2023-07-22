using UnityEngine;

namespace Common.Extensions
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInChildren<TComponent>(this MonoBehaviour self, out TComponent component,
            bool includeInactive = false) where TComponent : class
        {
            component = self.GetComponentInChildren<TComponent>(includeInactive);
            return component != null;
        }
    }
}