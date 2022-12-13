using UnityEngine;

[System.Serializable]
public class LocalizedDataContainer{

	/* LocalizedDataContainer
	 * by tim.perry.wagner@gmail.com
	 * last updated: 05/01/2018
	 * 
	 * Creates the required objects and numbers thereof to properly represent all data in the translation json.
	 */

	public LocalizedDataItem[] mainScreenAssets = new LocalizedDataItem[11];
	public LocalizedDataItem[] randomQuoteItems = new LocalizedDataItem[28];
	public LocalizedDataItem[] gamePlayScreenAssets = new LocalizedDataItem[8];
	public LocalizedCardPackItem[] cardPackDataObjects = new LocalizedCardPackItem[3];
	public LocalizedUILabels gamePlayUILabels;
	public LocalizedStoreStatusItem[] statusCodeItems = new LocalizedStoreStatusItem[9];

	public static LocalizedDataContainer initAndReturn(string jSonDataString) {
		return JsonUtility.FromJson<LocalizedDataContainer>(jSonDataString);
	}
}
