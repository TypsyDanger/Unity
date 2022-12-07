using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSASpanishButtonManager : LocalizationUIButton {
	// OnMouseDown and constants all defined in super.
	protected override void Awake() {
		base.Awake();
		_selectedLanguage = _LOCAL_LANG_SA_SPANISH;
	}
}
