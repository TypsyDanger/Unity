using UnityEngine;
using System.Collections;

[System.Serializable]
public class LocalizedStoreStatusItem {

	/* LocalizedStoreStatusItem
	 * by tim@reallybigsmile.com
	 * last updated: 05/01/2018
	 * 
	 * Represents a particular status state in the translation json that may be caught and returned by the iOS or Google Play stores.
	 */

	public string statusId;
	public string statusConstEquiv;
	public string statusString;
}
