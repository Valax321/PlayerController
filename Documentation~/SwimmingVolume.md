# Swimming Volume Component

The swimming volume component is used to define areas that the player uses swimming movement within. Volumes must have an attached collider of any sort and be on a player that collides with the player's layer.

# Properties
| Property | Type | Description |
| -------- | ---- | ----------- |
| Depth | Int | Defines which volume takes priority when multiple overlap. |
| Swim Speed | Float | The speed at which players swim through this volume. |
| Swim Acceleration | Float | The acceleration value for players in this volume. See [Player Movement](PlayerMovement.md) for more information. |
| Swim Deceleration | Float | The deceleration for players in this volume. See [Player Movement](PlayerMovement.md) for more information. |