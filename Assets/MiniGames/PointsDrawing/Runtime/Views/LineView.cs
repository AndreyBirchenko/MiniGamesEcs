using System;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

namespace MiniGames.PointsDrawing.Views
{
    public class LineView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _endingCircle;
        [SerializeField] private Collider2D _collider;

        private Sequence _sequence;
        private List<LineRenderer> _renderers = new List<LineRenderer>();

        public void SetFirstPoint(Vector3 position)
        {
            _lineRenderer.SetPosition(0, position);
        }

        public void SetLastPoint(Vector3 position)
        {
            KillActiveSequence();
            _endingCircle.transform.position = position;
            _lineRenderer.SetPosition(1, position);
        }

        public void AddPoint(Vector3 position)
        {
            var newRenderer = Instantiate(_lineRenderer, transform);
            _renderers.Add(newRenderer);
            newRenderer.SetPosition(0, position);
            
            _lineRenderer.SetPosition(1, position);
            _lineRenderer = newRenderer;
        }

        public void SetToLastPoint(Action call)
        {
            KillActiveSequence();
            var lastPointPosition = _lineRenderer.GetPosition(0);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(DOVirtual.Float(0, 1, 1.5f, x =>
                {
                    var position = Vector3.Lerp(_lineRenderer.GetPosition(1), lastPointPosition, x);
                    _lineRenderer.SetPosition(1, position);
                    _endingCircle.transform.position = position;
                }))
                .AppendCallback(call.Invoke);
        }

        public void SetActive(bool condition)
        {
            _collider.enabled = condition;
            _endingCircle.gameObject.SetActive(condition);
            _lineRenderer.gameObject.SetActive(condition);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            /*var zeroAlphaColor = Color.white; zeroAlphaColor.a = 0;
            var startColor = new Color2(Color.white, Color.white);
            var endColor = new Color2(zeroAlphaColor, zeroAlphaColor);
                
            _sequence = DOTween.Sequence();
            _sequence.Append(_lineRenderer.DOColor(startColor, endColor, 0.5f));
            foreach (var lr in _renderers)
            {
                _sequence.Join(lr.DOColor(startColor, endColor, 0.5f));
            }*/
        }

        private void KillActiveSequence()
        {
            if (_sequence.IsActive() && _sequence.IsPlaying())
            {
                _sequence.Kill();
                _sequence = null;
            }
        }

        private void OnDestroy()
        {
            KillActiveSequence();
        }
    }
}