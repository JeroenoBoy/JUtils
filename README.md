# JUtils

An library purely aimed to make development in unity easier

## Legenda

- [Attributes](#jutilsattributes)
- [Components](#jutilscomponents)
- [Extensions](#jutilsextensions)
- [Singletons](#jutilssingletons)


## JUtils.Attributes


### Button

The button attribute allows for easy setup of clickable buttons in the unity inspector. These buttons can be applied to functions and run them.

Usage:
```cs
[Button]
private void KillPlayer()
{
    //  Do something
}

[Button("Heal Player")]
private void WeirdName()
{
    //  Do something
}

[Button(clickableInEditor: true)]
private void AccessibleInEditor()
{
    //  Do something
}

//  Added in 1.4.0 

[Button]
private void Damage(int amount)
{
    //  Do something
}

[Button]
private IEnumerator Coroutine()
{
    //  Some coroutine, this is only clickable in playmode
}
```

### CustomName

This allows you to assign a custom name to a property in the inspector.

Usage:
```cs
public class Generator : Monobehaviour
{
    [Header("Terrain")]
    [CustomName("Enabled")]
    [SerializeField] private bool _terrainEnabled;
    [CustomName("Octaves")]
    [SerializeField] private Octave[] _terrainOctaves;
    
    [Header("Caves")]
    [CustomName("Enabled")]
    [SerializeField] private bool _cavesEnabled;
    [CustomName("octaves")]
    [SerializeField] private Octave[] _caveOctaves;
}
```

## Optional

This isn't actually an attribute, but I still think it belongs in this list. This attribute allows you to clearly define optional variable types using the Optional struct

Usage:
```cs
public class BillBoard : Monobehaviour
{    
    [SerializeField] private Optional<Transform> _target; 
    
    private void Update()
    {
        if (_target.enabled) transform.rotation = _target.rotation;
    }
}
```

### ReadOnly

Fields with this attribute cannot be edited via the inspector, they can still be edited by the script.

Usage:
```cs
public class DoNotTouch : Monobehaviour
{
    [ReadOnly]
    [SerializeField] private float _currentForce;
}
```

### Required

This attribute add a notification in the inspecotr when it is not filled in.

Usage:
```cs
public class PlayerMovement : Monobehaviour
{
    [Required]
    [SerializeField] private Animator _animator;
    [Required]
    [SerializeField] private CameraController _camera;
}
```

### ShowWhen

This attribute hides the current field if a certain variable doesn't match a condition.

Conditions can only be of type bool, string, float, int (also works with enums)

**Constructors**
```cs
public ShowWhen(string variable, string value, bool showAsObject = true)
public ShowWhen(string variable, bool value, bool showAsObject = true)
public ShowWhen(string variable, int value, Comparer comparer = Comparer.Equals)
public ShowWhen(string variable, int value, bool showAsObject, Comparer comparer = Comparer.Equals)
public ShowWhen(string variable, float value, Comparer comparer = Comparer.Equals)
public ShowWhen(string variable, float value, bool showAsObject, Comparer comparer = Comparer.Equals)
```

**Comparer enum** {Equals, Greater, Smaller, Or}

Usage:
```cs
public class BillboardCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private bool   _scaleWithDistance = true;
    
    [ShowWhen(nameof(_scaleWithDistance), true, false)]
    [SerializeField] private BillBoardSettings _settings;
}
```

### Unpack

This attribute can be used to unpack a serializable object in the inspector so it looks nicer.

Usage:
```cs
[Serializable]
public struct PlayerMovement : IComponent
{
    public float speed;
    public float jumpHeight;
}


public class PlayerMovementAuthoring : Monobehaviour
{
    [SerializeField, Unpack] private PlayerMovement _data
    
    //  Baker
}


```


## JUtils.Components


### Billboard Camera

This component is a simple bill boarding script with a handful of features 
- Can scale over distance
- Can have a specific anchor position

### Copy Position

This component sets the position of the game object to the target game object.


### Health Component

A simple health component without any default behaviours.

Fields:
```cs
bool isDead {get; set;}
int health { get } // Get the current health of the component
int maxHealth { get } // Get the maxHealth of the component   
```

Methods:
```cs
int Damage(int amount) // Damage the component by a certain amount
int Heal(int amount) // Heal the component by a certain amount
void Kill() // Does A LOT of damage to the health component and kills it
```

Messages the component sends (also gets send in this order)
```cs
void OnDeath()
void OnDamage(int damage)
void OnHeal(int health)
void OnHealthChange(int changedHealth)
```


## JUtils.Extensions

This namespace contains extension methods.

```cs

//  AudioSource

AudioSource audioSource;
void audioSource.RandomPitch(float min, float max) // Sets the pitch to a random range between min and max

//  Color

Color color;
bool color.Equals(Color other, float threshold) // Compares a color with a threshold
bool color.Equals2(Color other, float threshold) // A different way to calculate the threshold.

Color[] colors;
bool colors.TContains(Color target, float threshold) // Check if a element in an Enumerable objects contains the color

//  Component

Component component;
Ray  component.ForwardRay() // Shorthand for creating a way based on the transform of a component / Behaviour / Monobehaviour
bool component.HasLayer(int layer) // Check if the component's gameobject has a layer

//  Coroutines

WaitForSeconds         Coroutines.WaitForSeconds(int amount) // Returns a cached version of the requested WaitForSeconds instance
WaitForSecondsRealtime Coroutines.WaitForSecondsRealtime(int amount) // Returns a cached version of the requested WaitForSecondsRealtime instance
WaitForFixedUpdate     Coroutines.WaitForFixedUpdate() // Returns a cached version of the WaitForFixedUpdate instance
WaitForEndOfFrame      Coroutines.WaitForEndOfFrame() // Returns a cached version of the WaitForEndOfFrame instance
CoroutineCatcher       Coroutines.Catcher(IEnumerator coroutine) // Returns a CoroutineCatcher that can catch errors happening in the coroutine

CoroutineCatcher catcher;
bool catcher.HasThrown(out Exception exception) // Check if the coroutine has thrown an exception
yield return catcher; //  Can be used like this to run the coroutine

//  Enumerable

IEnumerable<Any> enumerator; // Can also be array, list, pretty much everything that inherits IEnumerable<T>
T enumerator.Random() // Returns a random element from the array based on UnityEngine.Random
T enumerator.Random(System.Random randomizer) // Returns a random element based on System.Random

//  Vectors

Vector2 vector2;
Vector3 vector2.ToXZVector3() // Convert this Vector2 to Vector3 where X = X, Y = 0, Z = Y
Vector2 vector2.With(float? x = null, float? y = null) // Set a specific value of the vector
Vector2 vector2.ClampMagnitude(float maxForce) // Clamp the magnitude of the vector
Vector2 vector2.Closest(Vector2 a, Vector2 b) // Get the closest vector from self
Vector2 vector2.Round() // Rounds the position of the vector
Vector2 vector2.Floor() // Floors the position of the vector
Vector2 vector2.Multiply(Vector2 other) // Multiplies sx*ox and sy*oy
Vector2 vector2.Positive(Vector2 other) // Returns the absolute vesion of the vector
Vector2 vector2.Negative(Vector2 other) // Returns the negative version of the vector

Vector3 vector3;
Vector2 vector3.ToXZVector2() // Convert this Vector2 to Vector3 where X = X, Y = 0, Z = Y
Vector3 vector3.With(float? x = null, float? y = null) // Set a specific value of the vector
Vector3 vector3.ClampMagnitude(float maxForce) // Clamp the magnitude of the vector
Vector3 vector3.Closest(Vector2 a, Vector2 b) // Get the closest vector from self
Vector3 vector3.Round() // Rounds the position of the vector
Vector3 vector3.Floor() // Floors the position of the vector
Vector3 vector3.Multiply(Vector2 other) // Multiplies sx*ox and sy*oy
Vector3 vector3.Positive(Vector2 other) // Returns the absolute vesion of the vector
Vector3 vector3.Negative(Vector2 other) // Returns the negative version of the vector
```

Since 1.4.6, the Range operator also got extensions:

Usages: <br>
`A..B`  - Loops from A to B -- <br>
`^A..B` - Loops from A to B-1 -- ^ at the start excludes the end number <br>
`A..^B` - Loops from B-1 to A -- ^ before the second number makes it loop in reverse<br>
`^A..^B` - Loops from A-1 to B -- Loops in reverse and excludes first number<br>

```cs

//  Syntax

RangeEnumerator (..10).GetEnumerator();
RangeEnumerator (10).GetEnumerator();

//  Can be used as following:

using JUtils.Extensions;

foreach (int i in 10..) {
    Debug.Log(i); // Logs 10 > 0
}

foreach (int i in ^10..^0) { // alias: ^10..
    Debug.Log(i); // Logs 9 > 0
}

foreach (int i in ^0..10) {
    Debug.Log(i); // Logs 0 > 9
}

foreach (int i in ..10) {
    Debug.Log(i); // Logs 0 > 10
}

foreach (int i in 20..^10) {
    Debug.Log(i); // Logs 0 > 10
}

foreach (int i in 10) {
    Debug.Log(i); // Logs 0 > 9
}

```


## JUtils.Singletons

### ISingleton\<T>

The singleton standard interface

### SingleTonBehaviour\<T>

Use this to create a new Singleton that also inherits Monobehaviour. The singleton reference also persists on hot reload

### PersistentSingletonBehaviour\<T>

This is a singleton that calls DontDestroyOnLoad(). It doesn't actually use this singleton, it creates a new instance and adds it to a auto generated DDOL singleton object. This means that you can just have all your managers on 1 game object without all of them getting send to the DDOL scene