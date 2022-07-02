using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEditor;

using UnityEngine;

namespace MiniGames.PointsDrawing.Views
{
    [AddComponentMenu(nameof(ItemView) + " in Points Drawing")]
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private GameObject _viewObject;
        [SerializeField] private float _dotRadius;
        [field: SerializeField] public Transform[] Dots { get; private set; }

        private float _animationDuration = 0.5f;
        private Vector3 _initScale;
        private Sequence _sequence;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Dots is null)
                return;

            foreach (var dot in Dots)
            {
                if (dot is null)
                    continue;

                Handles.DrawWireDisc(dot.position, Vector3.back, _dotRadius);
            }
        }
#endif

        public void Construct()
        {
            _initScale = _viewObject.transform.localScale;
            _viewObject.transform.localScale = Vector3.zero;
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            token.Register(() => GetSequence());
            
            _sequence = GetSequence();
            _sequence
                .Append(_viewObject.transform.DOScale(_initScale, _animationDuration));
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration), cancellationToken: token);
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            token.Register(() => GetSequence());
            
            _sequence = GetSequence();
            _sequence
                .Append(_viewObject.transform.DOScale(_initScale * 1.25f, _animationDuration / 2))
                .Append(_viewObject.transform.DOScale(Vector3.zero, _animationDuration / 2));
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration), cancellationToken: token);
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

        private void OnDestroy()
        {
            
        }
    }
}