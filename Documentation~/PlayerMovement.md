# Player Movement Component

The player movement component is responsible for the primary movement of the character.

## Movement Properties
| Property | Type | Description |
| -------- | ---- | ----------- |
| Walk Speed | Float | The speed the player moves when walking normally. |
| Run Speed | Float | The speed the player moves when sprinting. |
| Crouch Speed | Float | The speed the player moves when crouching. |
| Ground Acceleration | Float | How quickly the player reaches the move speed from a stationary position. |
| Ground Deceleration | Float | How quickly the player returns to stationary when no movement input is applied. |
| Air Acceleration | Float | How quickly the player accelerates in the air. |
| Jump Force | Float | The force applied upwards to the player when jump is pressed. |

## Size Properties
| Property | Type | Description |
| -------- | ---- | ----------- |
| Radius | Float | The radius of the player capsule. |
| Standing Height | Float | The height of the player capsule when not crouching. |
| Crouching Height | Float | The height of the player capsule when crouching. |
| Crouch Blend Speed | Float | How quickly the player's capsule size goes between standing and crouching. |

## Camera Properties
| Property | Type | Description |
| -------- | ---- | ----------- |
| Camera Root | Transform | The Transform used as the camera. This is pitched up and down according to the Y axis mouse movement. |
| Standing Camera Height | Float | The height of the camera from the capsule centre when not crouching. |
| Crouched Camera Height | Float | The height of the camera from the capsule centre when crouching. |
| Look Angles | Float (min-max) | The maximum and minimum pitch of the camera. |

## Programming with Player Movement Component

The Player Movement component broadcasts several events that can be received by your own scripts. More will be added over time.

`OnJumped()` is called when the player jumps.