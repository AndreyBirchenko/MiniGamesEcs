﻿using System;
using System.Threading;

using Client.Cofigs;
using Client.Views;

using Core.Services.Toolbar;

using Cysharp.Threading.Tasks;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;

namespace Client.Systems
{
    public class LoadMiniGameSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsCustomInject<MenuCatalogConfig> _catalogConfig = default;
        private EcsCustomInject<MenuView> _menuView = default;
        private EcsCustomInject<EndGameService> _endGameService = default;
        private EcsCustomInject<ToolbarService> _toolbarService = default;
        private EcsCustomInject<FadeScreenView> _fadeScreen = default;

        private BaseMiniGameConfig _currentMiniGameConfig;
        private CancellationTokenSource _ctsOnDestroy = new CancellationTokenSource();

        public void Init(IEcsSystems systems)
        {
            CreateMenuButtons();
            SubscribeButtons();
        }

        public void Destroy(IEcsSystems systems)
        {
            _ctsOnDestroy?.Cancel();
            _ctsOnDestroy?.Dispose();
        }

        private void CreateMenuButtons()
        {
            var miniGameButtonPrefab = Resources.Load<MiniGameButtonView>("Menu/MiniGameButtonView");
            var buttonsContainer = _menuView.Value.ButtonsContainer;

            foreach (var miniGameConfig in _catalogConfig.Value.MiniGameConfigs)
            {
                var buttonView = Object.Instantiate(miniGameButtonPrefab, buttonsContainer);
                buttonView.SetText(miniGameConfig.name.Replace("Config", string.Empty));
                buttonView.SubscribeButton(() => HandleMiniGameButtonAsync(miniGameConfig).Forget());
            }
        }

        private void SubscribeButtons()
        {
            _endGameService.Value.SubscribeQuitButton(() => HandleEndGameQuitButtonAsync().Forget());
            _endGameService.Value.SubscribeRestartButton(() => HandleEndGameRestartButtonAsync().Forget());
            _toolbarService.Value.SubscribeBackButton(() => HandleToolbarBackButton().Forget());
        }

        private async UniTask LoadSceneAsync(BaseMiniGameConfig miniGameConfig)
        {
            _currentMiniGameConfig = miniGameConfig;
            var sceneInstance = await miniGameConfig.Scene
                .LoadSceneAsync(LoadSceneMode.Additive)
                .WithCancellation(_ctsOnDestroy.Token);

            SceneManager.SetActiveScene(sceneInstance.Scene);

            _menuView.Value.gameObject.SetActive(false);
        }

        private async UniTask UnloadCurrentSceneAsync(bool clearCurrentMiniGameConfig = true)
        {
            await _currentMiniGameConfig.Scene
                .UnLoadScene()
                .WithCancellation(_ctsOnDestroy.Token);

            if (clearCurrentMiniGameConfig)
                _currentMiniGameConfig = null;
        }

        private async UniTaskVoid HandleMiniGameButtonAsync(BaseMiniGameConfig miniGameConfig)
        {
            await _fadeScreen.Value.Fade(1);
            await LoadSceneAsync(miniGameConfig);
            await _fadeScreen.Value.Fade(0);
        }

        private async UniTaskVoid HandleEndGameQuitButtonAsync()
        {
            await _fadeScreen.Value.Fade(1);

            await UnloadCurrentSceneAsync();
            HideServicesViews();

            await _fadeScreen.Value.Fade(0);
        }

        private async UniTaskVoid HandleEndGameRestartButtonAsync()
        {
            await _fadeScreen.Value.Fade(1);

            await UnloadCurrentSceneAsync(false);
            HideServicesViews();
            await LoadSceneAsync(_currentMiniGameConfig);

            await _fadeScreen.Value.Fade(0);
        }

        private async UniTaskVoid HandleToolbarBackButton()
        {
            await _fadeScreen.Value.Fade(1);

            await UnloadCurrentSceneAsync();
            HideServicesViews();

            await _fadeScreen.Value.Fade(0);
        }

        private void HideServicesViews()
        {
            _menuView.Value.gameObject.SetActive(true);
            _endGameService.Value.HideEndGamePopup();
            _toolbarService.Value.Reset();
        }
    }
}