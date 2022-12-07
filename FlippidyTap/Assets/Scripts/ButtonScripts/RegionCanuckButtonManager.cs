using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionCanuckButtonManager : LocalizationUIButton {
	protected override void Awake() {
		base.Awake();
		_selectedLanguage = _LOCAL_LANG_CANUCK;
	}
}
