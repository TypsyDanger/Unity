# DesignPatterns
This is a functioning Unity project with a set of scenes, each reprenting a specific design pattern that's applicable to Unity development.  Their associated and support scripts are also contained in the Assets/Scripts folder and it's child folders.

To use this project, go to the individual scenes in /Assets/Scenes/ to view a design pattern in function.  Instructions for each are as follows:

## Command Pattern
Use the A, S, W, D keys to move the camera relative to an object.  Each key is assigned it's functionality using the Command Pattner.

## Flyweight Pattern
Really, the devil is in the details here.  Go to the Managers object in the Heirarchy and view the Flyweight component script.  It'll show a crude difference between instantiating some objects in an inefficient manner, and using the flyweight pattern to save some memory.

## Observer Pattner
Use the A, S, W, D and Space keys as well as the mouse to look around and fire rudimentary bullets at the green and red spheres.  Shooting the green sphere will set an Observable manager to an ON state, raising the 5 blocks between the two spheres.  Shooting the red sphere will set the Observable manager to OFF, lowering the spheres.

NOTE: The bullets always face the X direction of the collection of objects, so if the mouse-look becomes confusing, fire some bullets and look at their orientation for a hint.
