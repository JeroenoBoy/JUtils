using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.UI
{
    [UxmlElement]
    public partial class AnimatedElement : UpdatingVisualElement
    {
        [UxmlAttribute] private float _showDuration = .2f;
        [UxmlAttribute] private float _showDelay = 0f;
        [UxmlAttribute] private JEase _showEase;
        [UxmlAttribute] private float _hideDuration = .2f;
        [UxmlAttribute] private float _hideDelay = 0f;
        [UxmlAttribute] private JEase _hideEase;

        [UxmlAttribute] private bool _animateOpacity;
        [UxmlAttribute] private float _hideOpacity;
        [UxmlAttribute] private bool _animateOffset;
        [UxmlAttribute] private Vector2 _hideOffset;

        private StyleFloat _defaultOpacity;
        private StyleTranslate _defaultPosition;


        public AnimatedElement()
        {
            style.width = new Length(100f, LengthUnit.Percent);
            style.height = new Length(100f, LengthUnit.Percent);
            style.position = Position.Absolute;

            style.opacity = 1;
            style.translate = new Translate(0, 0, 0);

            RegisterCallback<AttachToPanelEvent>(HandleAttachToPanel);
        }


        public void ShowInstant()
        {
            if (_animateOpacity) {
                style.opacity = new StyleFloat(_defaultOpacity.value) { keyword = _defaultOpacity.keyword };
            }

            if (_animateOffset) {
                style.translate = _defaultPosition;
            }
        }


        public Task Show()
        {
            return Show(cancellationToken);
        }


        public Task Show(CancellationToken token)
        {
            List<Task> tasks = new(2);

            if (_animateOpacity) {
                Task task = AsyncSequence
                   .Delay(_showDelay, token)
                   .Tween(
                        style.opacity.value, _defaultOpacity.value, _showDuration, _showEase, token,
                        value => style.opacity = new StyleFloat(value) { keyword = _defaultOpacity.keyword }
                    );

                tasks.Add(task);
            }

            if (_animateOffset) {
                Task task = AsyncSequence
                   .Delay(_showDelay, token)
                   .Tween(
                        style.translate.value, _defaultPosition.value, _showDuration, _showEase, token,
                        value => style.translate = new StyleTranslate(value)
                    );

                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }


        public void HideInstant()
        {
            if (_animateOpacity) {
                style.opacity = new StyleFloat(_hideOpacity) { keyword = _defaultOpacity.keyword };
            }

            if (_animateOffset) {
                style.translate = new Translate(
                    new Length(_hideOffset.x + _defaultPosition.value.x.value, _defaultPosition.value.x.unit),
                    new Length(_hideOffset.y + _defaultPosition.value.y.value, _defaultPosition.value.y.unit), _defaultPosition.value.z
                );
            }
        }


        public Task Hide()
        {
            return Hide(cancellationToken);
        }


        public Task Hide(CancellationToken token)
        {
            List<Task> tasks = new(2);

            if (_animateOpacity) {
                Task task = AsyncSequence
                   .Delay(_showDelay, token)
                   .Tween(
                        style.opacity.value, _hideOpacity, _showDuration, _showEase, token,
                        value => style.opacity = new StyleFloat(value) { keyword = _defaultOpacity.keyword }
                    );

                tasks.Add(task);
            }

            if (_animateOffset) {
                Translate targetTranslate = new(
                    new Length(_hideOffset.x + _defaultPosition.value.x.value, _defaultPosition.value.x.unit),
                    new Length(_hideOffset.y + _defaultPosition.value.y.value, _defaultPosition.value.y.unit), _defaultPosition.value.z
                );

                Task task = AsyncSequence
                   .Delay(_showDelay, token)
                   .Tween(
                        style.translate.value, targetTranslate, _showDuration, _showEase, token,
                        value => style.translate = new StyleTranslate(value)
                    );

                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }


        private void HandleAttachToPanel(AttachToPanelEvent evt)
        {
            _defaultOpacity = style.opacity;
            _defaultPosition = style.translate;

            if (Application.isPlaying) {
                HideInstant();
            }
        }
    }
}