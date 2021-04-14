# frAgile
A platformer game created in Unity.

**Tuttep Tanwattanakul**
Focus mainly on coding on advanced topics like Mobile controls, New Input Systems, game logic. I've contributed most of the coding and doing the gameplay to the point that it feels good. Right now we've encountered a mountain of bugs.

**Warissara Wadsaengsri**
Focus mainly on finding assets, sounds, and doing simple coding like animations and some simple game logic. I've contributed the animations and animation coding plus some simple game logic like jumping and double jumping.

**What we've done**
- Mobile controls (Prototype)
- A simple level mostly for testing purposes.
- The character can run, jump, wall jump, wall slide and double jump at this moment.
- Tile rule to speedup the level designing process and save time on making the levels look good.
- Added footsteps sound.
- Added Background music.
- Added main menu.
- Tested on 2 iPhone devices at different resolution.
- ~~Implemented a script from MIT to help with different mobile resolutions. (WIP as the aspect ratio is not the same across devices)~~ *Unused due to conflicts with Cinemachine*
- Added Spikes
- Added a basic game loop once the player has died
- Added login and Register
- *NEW => Implemented Cinemachine with smooth camera tracking*
- *NEW => Improved movement mechanics*

**What we will do within this term**
- Reimplement the joystick for movement, if possible.
- Implement dashing, climbing ladders, and bouncing off from a projectile.
- Add Pause button and return to main menu while in game.
- Create a demo level to play.
- Tidyup UIs
- Add some transitions between scenes
- Add some particle effects for when the player die or the player jump
- Add time-based hazards like flood.

# Dev Logs

Log #3 - 14/4/2021
- Implemented better wall jumping and wall sliding mechanics
- Added particle effects when the player walk and jump

Log #2 - 13/4/2021
- Implemented cinemachine to move with the player
- Fixed goofy movement to use RigidBody2D
- Fixed double jump
- Added additional functionality when tap and holding jump
- Fixed falling and jumping animation

Log #1 - 7/4/2021
- Firebase Login and Register System is now implemented except the UI
- Added spike as a prefab which has a simple reset level function if triggered
