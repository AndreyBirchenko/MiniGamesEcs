using System;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using PoppingItems.Services;

using UnityEngine;
using UnityEngine.UI;

namespace Client.Views
{
    public class FadeScreenView : MonoBehaviour
    {
        [SerializeField] private Image _fadeScreen;

        private Sequence _sequence;

        public async UniTask Fade(int to)
        {
            _sequence = GetSequence();
            _sequence
                .Append(_fadeScreen.DOFade(to, Constants.FadeScreenTime));

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.FadeScreenTime));
        }

        private Sequence GetSequence()
        {
            if (_sequence.IsActive() && _sequence.IsPlaying())
            {
                _sequence.Kill();
                _sequence = null;
            }

            return DOTween.Sequence();
        }
    }
}