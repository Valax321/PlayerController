# Player Controller System Documentation

The player controller system in this package is a fairly complete player controller implementation built on top of Unity's built-in Character Controller. It contains the basic set of features seen in many first person games, such as jumping, crouching, sprinting and sliding on steep slopes.

## Components
The controller system relies on several main components for function.

- [Player Movement](PlayerMovement.md): handles the actual player character movement.
- [Swimming Volume](SwimmingVolume.md): sets up volumes for swimming.
- [Player Input Source](PlayerInputSource.md): Abstracted input system.