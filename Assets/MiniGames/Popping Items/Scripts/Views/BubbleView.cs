using Core.Services.Toolbar.Components.Events;

using DG.Tweening;

using Leopotam.EcsLite;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Services.Toolbar.Views
{
    [RequireComponent(typeof(Collider2D))]
    public class BubbleView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _spriteObject;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private TextMeshPro _textMeshPro;
        
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
        public EcsWorld EcsEventWorld { get; set; }

        private float _animDuration = 0.4f;
        private Vector3 _initScale;
        private Sequence _sequence;

        private void OnValidate()
        {
            if (_collider2D != null)
                return;
            
            _collider2D = GetComponent<Collider2D>();
        }

        private void Awake()
        {
            _initScale = _spriteObject.transform.localScale;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var entity = EcsEventWorld.NewEntity();
            ref var eventComponent = ref EcsEventWorld.GetPool<ClickEvent<BubbleView>>().Add(entity);
            eventComponent.View = this;
            eventComponent.PointerEventData = eventData;
        }

        public void Initialize(string answerText)
        {
            _spriteObject.SetActive(true);
            _spriteObject.transform.localScale = _initScale;
            _collider2D.enabled = true;
            _textMeshPro.text = answerText;
        }

        public void PlayPopAnimation()
        {
            var tr = _spriteObject.transform;
            _sequence = GetSequence();
            _sequence
                .AppendCallback(() => _collider2D.enabled = false)
                .Append(tr.DOScale(tr.localScale * 0.8f, _animDuration / 2))
                .Append(tr.DOScale(tr.localScale * 1.25f, _animDuration / 2))
                .AppendCallback(() => _spriteObject.SetActive(false))
                .AppendCallback(() => _particleSystem.Play());
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