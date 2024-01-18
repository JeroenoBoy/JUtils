# JUtils
A unity utilities library, this contains many handy extensions, components and data structures with custom editors

Documentation: https://jeroenoboy.github.io/JUtils/

## What is JUtils?

JUtils adds handy extensions, classes and components which makes writing code faster and easier.

Please not that when using some JUtils components, it might be better to use OnEnable and OnDisable insteaad of Awake and Destroy.

Here is an example script written with JUtils extensions

```csharp
namespace Example
{
    public class EnemySpawner : Monobehaviour
    {
        [SerializeField, Required] private ObjectPool _objectPool;    // Shows an error in the inspector when the field is not assigned
        [SerializeField] private Optinal<Transform> _optionalParent; //  Mark this variable as not being required, has a toggle in the inspector to make it more clear
        
        [Header("Spawn Settings")]
        [SerializeField, CustomName("Interval")] private MinMax _spawnInterval;
        [SerializeField, CustomName("Amount")] private MinMaxInt _spawnAmount;
        [SerializeField, CustomName("Radius")] private float _spawnRadius;
        [SerializeField] private Weighted<EnemyData>[] _randomEnemyData; // A helper struct for doing easy weighted randomness
        
        private Transform _parent;
        private float _nextSpawn;
        
        
        private void Awake()
        {
            _parent = _optionalParent.enabled ? _optionalParent.value : transform; // More efficient than checking object for null
            _lastSpawnd = Time.time + _spawnInterval.Random()
        }
        
        
        private void Update()
        {
            if (Time.time < _nextSpawn) return;
            _nextSpawn = Time.time + _spawnInterval.Random();
            
            SpawnBatch(_spawnAmount.Random());
        }
        
        
        [Button]
        private void SpawnBatch(int spawnAmount)
        {
            for (int i = spawnAmount; i --> 0;) {
                PoolItem item = _objectPool.SpawnItem(Random.insideUnitCircle.With(y: 0) * _spawnRadius, _parent);
                
                Enemy enemy = item.GetComponent<Enemy>();
                enemy.EnemyData = _randomEnemyData.Random();
                
                StartCoroutine(Routines.Delay(i * .5f, () => enemy.WakeUp())) // You can use this instead of creating a new routine in the script
            }
        }
    }
)
```

## What does JUtils include?

- Many attributes for making things more clear in the inspector
- Clickable buttons in the inspector with support for arguments & coroutines
- Many serializable data structures with custom property drawers
- State machine with type-safe arguments
- Handy common components
- Object pools
- Singletons
- Many handy extensions for vectors, transforms, components & more
- Coroutine helper functions
- Some gizmos utilities for drawing forces & text

## Liscence

JUtils versions **1.9.3 and up** have been licensed under **LGPL-3.0**
JUtils versions < 1.9.0 have been licensed under MIT