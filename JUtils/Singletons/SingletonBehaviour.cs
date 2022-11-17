using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour, ISingleton<T>, ISerializationCallbackReceiver
        where T : MonoBehaviour, ISingleton<T>
    {
        public static T instance { get; private set; }


        protected virtual void OnEnable()
        {
            if (!instance || instance == this) instance = this as T;
            else
            {
                Destroy(this);
                Debug.LogError("Instance already exists!");
            }
        }


        protected virtual void OnDisable()
        {
            if (instance == this) instance = null;
        }
        

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            if (instance) {
                Debug.LogWarning("Instance already exists, can be ignored when reloading scene");
            }
            else {
                instance = this as T;
            }
        }
    }
}