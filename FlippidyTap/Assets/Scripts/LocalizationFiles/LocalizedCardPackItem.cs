using UnityEngine;
using System.Collections;

[System.Serializable]
public class LocalizedCardPackItem {
	/* LocalizedCardPackItem
	 * by tim@reallybigsmile.com
	 * last updated: 05/01/2018
	 * 
	 * Represents a "cardPackDataObject" object in the translation json.  Required for accurate parsing.
	 */

	public string cardPackId;
	public string cardPackLabel;
	public string cardPackCopy;
}
