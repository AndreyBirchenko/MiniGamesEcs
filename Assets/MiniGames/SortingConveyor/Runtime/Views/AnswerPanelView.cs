using System;
using System.Linq;

using Core.Services;

using DG.Tweening;

using UnityEngine;

namespace Core.Views
{
    [RequireComponent(typeof(Collider2D))]
    public class AnswerPanelView : MonoBehaviour
    {
        [SerializeField] private AnswerView[] _answerViews;
        [SerializeField] private Collider2D _collider;

        private Sequence _sequence;

        private void OnValidate()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void SetAnswerView(ItemType itemType)
        {
            var activeAnswerView = _answerViews.First(x => x.gameObject.activeSelf);
            var newAnswerView = _answerViews.First(x => x.ItemType.Equals(itemType));

            _sequence = GetSequence();
            _sequence
                .AppendCallback(() => _collider.enabled = false)
                .Append(activeAnswerView.transform.DOScale(0, 0.3f))
                .AppendCallback(() => activeAnswerView.gameObject.SetActive(false))
                .AppendCallback(() => newAnswerView.transform.localScale = Vector3.zero)
                .AppendCallback(() => newAnswerView.gameObject.SetActive(true))
                .Append(newAnswerView.transform.DOScale(newAnswerView.InitScale, 0.3f))
                .AppendCallback(() => _collider.enabled = true);
        }

        public bool IsOverlap(Vector2 point)
        {
            return _collider.OverlapPoint(point);
        }

        private Sequence GetSequence()
        {
            if (_sequence != null && _sequence.IsActive()) _sequence.Kill();

            return DOTween.Sequence();
        }

        private void OnDestroy()
        {
            GetSequence();
        }
    }
}