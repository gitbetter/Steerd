# Steerd
An AI steering manager and library for Unity. Steerd uses a group priority and blended steering weight hybrid composer to run all the registered steering behaviors and return a weigthed steering output structure.

## Installation

To setup Steerd in your project simply go to _Window -> Package Manager_, click the _+_ icon on the top left and select _Add package from git URL_ to add the Steerd git URL.

## Usage

You can create configurable scriptable objects for each behavior by going to _Assets -> Create -> Steerd_ and picking the steering behavior that suits your needs. You can also extend the `Behavior` base and create your own steering behaviors as described below.

After creating steering behavior scriptable objects, you can reference them directly from scripts and call the `GetSteering` method, which returns a `SteeringOutput` instance with the relevant steering data. Check below for more on the `SteeringOutput` class.

Here's an example that takes in a single behavior and uses it to set the player's rigidbody acceleration upon startup:
```csharp
public class PlayerMovement : MonoBehaviour {
	[SerializeField]
	public Behavior steeringBehavior;

	Rigidbody rb;
	void Start() {
		rb = GetComponent<Rigidbody>();
		Steerd.SteeringOutput steering = steeringBehavior.GetSteering();
		rb.AddForce(steering.linear, ForceMode.Acceleration);
	}
}
```

Steerd also includes a `Composer` implementation as a monobehaviour you can add to any game object. The editor for this component exposes a _Behavior Infos_ array which you can populate with your available steering behaviors, giving each a blending weight which dictates how much that behavior will influence the steering output. The `Composer` also has a `GetSteering` method which you can call from your scripts to get the accumulated steering of all the composed behaviors. Just add a `Composer` component to any of your GameObjects and reference it from any of your scripts.

## SteeringOutput

The `SteeringOutput` class has the following public fields:

### `public Vector3 linear`

    Describes a calculated linear kinematic acceleration

### `public float angular`

    Describes a calculated angular acceleration (a rotation in mathematical terms)

It also has the following public methods:

### `public void Clear()`

    Zeroes out both the `linear` and `angular` fields

Two `SteeringOutput` instances can be added or multiplied together. In both cases, the operation is done component-wise (i.e. `linear` fields are added/multipled together, and the same for multiplication).

## Custom Behaviors

To create custom behaviors, simply derive from `Behavior` and override the `GetSteering` method, which is the bread and butter of the steering behavior implementation:

```csharp
public abstract SteeringOutput GetSteering();
```
Optionally, you can override the `Initialize` method if you need to do any initialization of your own, but remember to call `base.Initialize()` somewhere in your implementation.

The `Behavior` class also includes a static helper `RigidbodyToKinematic` method to extract the `Kinematic` data from a Rigidbody instance.

Additionally, it is advised that you include a `CreateAssetMenu` attribute with each of your behavior subclasses so that they are available within the editor context menu. Here is an example class to get you started:

```csharp
[CreateAssetMenu(fileName = "MySteer", menuName = "Steerd/MySteer", order = 1)]
public class MySteer : Behavior {
    override public SteeringOutput GetSteering() {
        SteeringOutput output = new SteeringOutput();
        // ...
        // Get creative with some output processing here
        // ...
        return output;
    }
}
```

There are also additional, but optional properties that can be overriden for better control of how your steering behaviors are processed:

### `public virtual Flags flags { get; }`

	Available flags are:
	* SINGLE_TARGET: behavior only uses a single target for processing the steering output
	* MULTI_TARGET: behavior can accept a list of targets used to accumulate a steering behavior output
	* PATH_FOLLOWER: behavior does not follow a target but instead follows a `Path`

**Note**: These flags only affect how the editor looks, for now.

### `public virtual Groups group { get; }`

	Available groups are (in descending order of priority):
	* Collide: The highest priority group, meant to denote a steering behavior that is meant to process and act on collisions
	* Approach: A group used for behaviors that close the gap towards a target or path
	* Avoid: The opposite of Approach, this group is used for behaviors that are meant to widen the gap to a target or path
	* Amble: The lowest priority group, meant for default-style wander behaviors that don't necessarily follow a path or target, but must output some usually stochastic steering outcome

**Note**: These groups are used in prioritizing the steering behaviors in the `Composer`.

### `public virtual Kinematic character { get; set; }`

	Override or set this to specify the subject/character that is the focal point of the steering behavior.

### `public virtual Kinematic target { get; set; }`

	Override or set this to specify the target of the steering behavior. This is mostly meant for SINGLE_TARGET behaviors, but you might find other uses for it.

### `public virtual List<Kinematic> targets { get; set; }`

	Override or set this to specify all the targets that influence this steering behavior. This is mostly meant for MULTI_TARGET behaviors, but see if you can get creative.

### `public virtual Path path { get; set; }`

	Override or set this to specify a `Path` for the steering behavior. Meant mostly for PATH_FOLLOWER behaviors, but it might be used in conjunction with a target or multiple targets, although it might be a more modular idea to create separate behaviors when processing targets and paths.