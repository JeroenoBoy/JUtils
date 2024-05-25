using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Bill-boarding component which allows for advanced anchoring and other behaviours
    /// </summary>
    public sealed class BillboardCamera : MonoBehaviour
    {
        public enum Anchor
        {
            Top,
            Bottom,
            Left,
            Right,
            Center
        }


        [SerializeField] private Camera _camera;
        [SerializeField] private bool _scaleWithDistance = true;

        [ShowWhen(nameof(_scaleWithDistance), true, false)]
        [SerializeField] private BillBoardSettings _settings;

        private Transform _cameraTransform;
        private Vector3 _localPosition, _scale;

        private void Start()
        {
            if (_camera == null) _camera = Camera.main;
            if (_camera == null) throw new Exception("Camera was not found");

            Transform transform = this.transform;
            _cameraTransform = _camera.transform;
            _localPosition = transform.localPosition;
            _scale = transform.localScale;
        }

        private void LateUpdate()
        {
            Transform transform = this.transform;
            transform.rotation = _cameraTransform.rotation;

            if (!_scaleWithDistance) return;

            float distance = (transform.position - _cameraTransform.position).magnitude;
            Vector3 offset = _settings.offset;

            transform.localPosition = _localPosition * Mathf.Sqrt(distance * _settings.positionMultiplier) - offset * (distance * _settings.scaleMultiplier);
            transform.localScale = _scale * (distance * _settings.scaleMultiplier);
        }


        [Serializable]
        private class BillBoardSettings
        {
            public Anchor anchor;
            public float scaleMultiplier;
            public float positionMultiplier;

            [SerializeField] private float _offset;

            public Vector3 offset => anchor switch {
                Anchor.Top => new Vector3(0, _offset),
                Anchor.Bottom => new Vector3(0, -_offset),
                Anchor.Left => new Vector3(-_offset, 0),
                Anchor.Right => new Vector3(_offset, 0),
                Anchor.Center => Vector3.zero,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}