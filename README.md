# Unity

## Contents
This is a repo consisting of code examples I've written in the past. 

### /FlippidyTap
This is a collection of scripts from self-released ios and android game that I put out in 2018.  I've shared the corescript files, tjough the rest of the project has been kept private because, well hey, who gives away their IP's for free these days.

### /LocalizationManager
This is a set of scripts that handles localization in a given game project.  It parses a json file, looking for objects consisting of Type, Name and Localized Value properties, then it scans a given scene for objects of the given type, with the given names and then finds the currently selected languages value, then updates appropriately.  

It can use Image and Text types, and will replace an image with a localized version by the asset ID listed in the localized value, or update the text value using the localized text in the appropriate value property.

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
