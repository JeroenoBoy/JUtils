using System;
using JUtils.Attributes;
using UnityEngine;



namespace JUtils.Components
{
    public class BillboardCamera : MonoBehaviour
    {
        public enum Anchor { Top, Bottom, Left, Right, Center }
        
        [SerializeField] private Camera _camera;
        [SerializeField] private bool   _scaleWithDistance = true;
        
        [ShowWhen(nameof(_scaleWithDistance), true, false)]
        [SerializeField] private BillBoardSettings _settings;

        private Transform _cameraTransform;
        private Vector3   _localPosition, _scale;

        private void Start()
        {
            if (!_camera) _camera = Camera.main;
            _cameraTransform = _camera.transform;
            _localPosition = transform.localPosition;
            _scale = transform.localScale;
        }
        
        private void LateUpdate()
        {
            transform.rotation = _cameraTransform.rotation;

            if (!_scaleWithDistance) return;
            
            //  Get the distance to the camera
            
            float distance = (transform.position - _cameraTransform.position).magnitude;
            Vector3 offset = _settings.offset;

            //  Apply the offset
            
            transform.localPosition = _localPosition * Mathf.Sqrt(distance * _settings.positionMultiplier)
                                      - offset * (distance * _settings.scaleMultiplier);
            transform.localScale    = _scale * (distance * _settings.scaleMultiplier);
        }
        
        
        [System.Serializable]
        public class BillBoardSettings
        {
            public Anchor anchor;
            public float  scaleMultiplier;
            public float  positionMultiplier;
            
            [SerializeField] private float _offset;

            public Vector3 offset => anchor switch
            {
                Anchor.Top    => new Vector3(0, _offset),
                Anchor.Bottom => new Vector3(0, -_offset),
                Anchor.Left   => new Vector3(-_offset, 0),
                Anchor.Right  => new Vector3(_offset, 0),
                Anchor.Center => Vector3.zero,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
