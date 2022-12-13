# Unity

## Contents
This is a repo consisting of little side projects I've put together. 

### /DesignPatterns
This is a Unity project that I put together with scenes and a series of script namespaces covern a specific set of Design Pattner that's applicable to Game Development.  All projects will run in Unity 2021.3.15f1.  See the README.md file in each folder for an explanation of the pattern it represents.

To use this project, go to the individual scenes in /Assets/Scenes/ to view a design pattern in function.  Instructions for each are as follows:

#### Command Pattern
Use the A, S, W, D keys to move the camera relative to an object.  Each key is assigned it's functionality using the Command Pattner.

#### Flyweight Pattern
Really, the devil is in the details here.  Go to the Managers object in the Heirarchy and view the Flyweight component script.  It'll show a crude difference between instantiating some objects in an inefficient manner, and using the flyweight pattern to save some memory.

#### Observer Pattner
Use the A, S, W, D and Space keys as well as the mouse to look around and fire rudimentary bullets at the green and red spheres.  Shooting the green sphere will set an Observable manager to an ON state, raising the 5 blocks between the two spheres.  Shooting the red sphere will set the Observable manager to OFF, lowering the spheres.

NOTE: The bullets always face the X direction of the collection of objects, so if the mouse-look becomes confusing, fire some bullets and look at their orientation for a hint.

### /FlippidyTap
This is one such project that constituted a self-released ios and android game that I put out in 2018.  I've shared the animation and script files, tough the rest of the project has been kept private because, well hey, who gives away their IP's for free these days?

Of specific interest, I'd like to flag the FlippidyTap/Assets/Scripts/LocalizationFiles scripts, starting with LocalizationManager.  As a side goal for FlippidyTap I wanted to write a Localization manager that could, well, take a loc' jSON file and re-translate the game on the fly.  It worked fairly well by associating element names and types to an array of localized strings.  It was also the last thing I worked on in the project, so the code was more mature than some of the earlier elements.
