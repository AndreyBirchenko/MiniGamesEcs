using Client.Cofigs;
using Client.Views;

using Cysharp.Threading.Tasks;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Client.Systems
{
    public class LoadMiniGameSystem : IEcsInitSystem
    {
        private EcsCustomInject<MenuCatalogConfig> _catalogConfig = default;
        private EcsCustomInject<MenuView> _menuView = default;

        public void Init(EcsSystems systems)
        {
            CreateMenuButtons();
        }

        private void CreateMenuButtons()
        {
            var miniGameButtonPrefab = Resources.Load<MiniGameButtonView>("Menu/MiniGameButtonView");
            var buttonsContainer = _menuView.Value.ButtonsContainer;

            foreach (var miniGameConfig in _catalogConfig.Value.MiniGameConfigs)
            {
                var buttonView = Object.Instantiate(miniGameButtonPrefab, buttonsContainer);
                buttonView.SetText(miniGameConfig.name);
                buttonView.SubscribeButton(() => LoadSceneAsync(miniGameConfig.Scene).Forget());
            }
        }

        private async UniTaskVoid LoadSceneAsync(AssetReference scene)
        {
            var sceneInstance = await scene.LoadSceneAsync(LoadSceneMode.Additive);
            SceneManager.SetActiveScene(sceneInstance.Scene);
        }
    }
}