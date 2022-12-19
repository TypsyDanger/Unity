# PlayFabIntegration
This is a functioning Unity project to demo a PlayFab integration and some Mobile Game-related usages.

Currently, the project will run a set of tests upon running, including:

* Playfab Login
* Get Player Title Data (this currently assumes that a lastLogin key/value exists in the player title data.  Will be updated in the future to make no assumptions);

To run this, you must do the following:
1. Sign up for a PlayFab account
2. Log into the account using the PlayFab Editor Extensions (installed)
3. Select your title in the Editor Extensions
4. Update /Assets/Scripts/Constants/PlayFabConstants.cs and change PLAYFAB_TITLE_ID to the title id in your account
