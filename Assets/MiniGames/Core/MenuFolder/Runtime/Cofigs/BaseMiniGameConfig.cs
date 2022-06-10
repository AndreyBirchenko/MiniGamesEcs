using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Client.Cofigs
{
    public abstract class BaseMiniGameConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference Scene { get; private set; }
    }
}