# frAgile
A platformer game created in Unity. Adapted and improved from GameDev.tv's 2D Unity course lectures.

[**Alpha Build Download here (Android only!)**](https://drive.google.com/file/d/18CDpixyDLu_hu3gzmogiqANV6D6OnPYw/view?usp=sharing)

**Tuttep Tanwattanakul**
Focus mainly on coding on advanced topics like Mobile controls, New Input Systems, game logic. I've contributed most of the coding and doing the gameplay to the point that it feels good. Right now we've encountered a mountain of bugs.

**Warissara Wadsaengsri**
Focus mainly on finding assets, musics, sounds, and doing simple coding like animations and some simple game logic. I've contributed the animations and animation coding plus some simple game logic like jumping and double jumping. I've also done most of the UIs in the game to be mobile-friendly.

**Project Summary**
- About 100+ hours spent on this project
- The best feature about this project is the movement mechanics, it feels smooth and easy to control on mobile.
- If we have more time, we will add more abilities, levels, timing system, and a progression system.
- Reimplement an analog stick movement for bash ability inspired by Ori series.
- Bug fixes will be done in the future to make the game more robust.

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
- Implemented Cinemachine with smooth camera tracking
- Improved movement mechanics
- Added particles
- UI Cleanup
- Created 3 demo levels
- *NEW => Added Transitions between scenes*
- *NEW => Pause function and Returning to menu at any point*
- *NEW => Added a cutscene*
- *NEW => Added tutorials*
- *NEW => Introduced a time-trial level*

**What we will do within this term**
- ~~Reimplement the joystick for movement, if possible.~~ Cut due complexity in rewriting the code.
- ~~Implement bouncing off from a projectile.~~ Cut due to complexity in rewriting the whole controllers to use the analog stick.

# Dev Logs

Log #6 - 22/4/2021
- Added scene transitions
- Added transitions when the player died
- Added particle effects for when the player die
- Added Pause button and return to main menu while in game.
- Added a cutscene
- Added level 3
- Added flood to encourage players to move quickly
- Finished a demo build

Log #5 - 16/4/2021
- Created level 2 with an increase in difficulty to improve the player skills to use dash ability
- Added a new zoom-out button to see the surroundings
- Created a basic scene linking
- Added more sound effects
- Tested a build on Android, so far 1 device tested with no performance issues

Log #4 - 15/4/2021
- Tidied up the main menu page and all the panels to be mobile-friendly
- Added custom font
- Implemented dashing with animations
- Created Level 1 which requires only a normal jump to complete
- Created a progression system where player needs to obtain abilities before using them **=> Not fully implemented yet**

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
