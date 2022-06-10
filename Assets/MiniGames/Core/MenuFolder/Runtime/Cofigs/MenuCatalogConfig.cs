using UnityEngine;

namespace Client.Cofigs
{
    [CreateAssetMenu(menuName = "MiniGames/MenuCatalogConfig", order = 0)]
    public class MenuCatalogConfig : ScriptableObject
    {
        [field: SerializeField] public BaseMiniGameConfig[] MiniGameConfigs { get; private set; }
    }
}