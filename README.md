# Unity

## Contents
This is a repo consisting of code examples I've written in the past. 

### /Addressables
This is a small demo for using Addressables in a game project to manage in-game assets.  It's currently 

### /DesignPatterns
This is a Unity project that I put together with scenes and a series of script namespaces covern a specific set of Design Pattner that's applicable to Game Development.  All projects will run in Unity 2021.3.15f1.  See the README.md file in each folder for an explanation of the pattern it represents.

Instructions for each design pattern scene are in the /DesignPatterns/README.md file

### /FlippidyTap
This is a collection of scripts from self-released ios and android game that I put out in 2018.  I've shared the corescript files, tjough the rest of the project has been kept private because, well hey, who gives away their IP's for free these days.

### /LocalizationManager
This is a set of scripts that handles localization in a given game project.  It parses a json file, looking for objects consisting of Type, Name and Localized Value properties, then it scans a given scene for objects of the given type, with the given names and then finds the currently selected languages value, then updates appropriately.  

It can use Image and Text types, and will replace an image with a localized version by the asset ID listed in the localized value, or update the text value using the localized text in the appropriate value property.

### /PlayFabIntegration
This is a functioning Unity project that exemplifies a set of scripts that enable various PlayFab usages in Mobile Games.  

To run this project, you'll need to sign up for a Playfab account, login into the SDK in the Unity IDE using the Playfab Editor Extensions (installed), then update the Assets/Scripts/Constants/PlayFabConstants.cs file to use your title ID.

