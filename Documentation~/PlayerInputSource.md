# Player Input Sources

Player input sources are an abstracted input system that allows the player controller to interface with multiple input systems (e.g. Unity Input, Rewired etc.)

Input sources are components that should be added to the same GameObject as the Player Movement component. The Player Movement component will display an error in the console if it can't find an input source when play begins.

The package contains a default implementation of an input source with the Unity Input Source component. This allows you to specify which entries in the Input Manager settings will control each function of the Player Movement it is attached to.

## Writing a Custom Input Source

It is quite easy to write a custom input source. To do so you must make a class inheriting from `PlayerInputSource` and provide an implementation for each input function.

```csharp
class MyInputSource : Valax321.PlayerCharacter.PlayerInputSource
{
    public override float GetForwardAxis()
    {
        // Get your input value here.
        return 1.0f;
    }

    // etc. for the rest of the overidden functions
}
```

This can then be added as a component to your GameObject with the Player Movement component.