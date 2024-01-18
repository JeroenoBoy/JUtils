using System;
using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// Serialize a System.Type Object
    /// </summary>
    [Serializable]
    public struct SerializableType : ISerializationCallbackReceiver
    {
        public static SerializableType Get<T>() => new(typeof(T));
        
        [SerializeField] private int _assemblyCode;
        [SerializeField] private int _typeCode;

        public Type type;


        public SerializableType(Type type)
        {
            this.type     = type;
            _typeCode     = type?.GetHashCode() ?? -1;
            _assemblyCode = type != null ? type.Assembly.GetHashCode() : -1;
        }
        
        
        public void OnBeforeSerialize()
        {
            _typeCode     = type?.GetHashCode() ?? -1;
            _assemblyCode = type != null ? type.Assembly.GetHashCode() : -1;
        }


        public void OnAfterDeserialize()
        {
            if (_typeCode == -1) type = null;
            else if (_assemblyCode == -1) type = null;
            else type = AssemblyJUtils.GetTypeFromCode(_typeCode, _assemblyCode);
        }
        
        
        public static implicit operator Type(SerializableType serializableType) => serializableType.type;
    }
}