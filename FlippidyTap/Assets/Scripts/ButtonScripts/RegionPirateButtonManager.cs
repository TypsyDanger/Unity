using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionPirateButtonManager : LocalizationUIButton {
	// constants and onmousedown set in super.
	protected override void Awake() {
		base.Awake();
		_selectedLanguage = _LOCAL_LANG_PIRATE;
	}
}
