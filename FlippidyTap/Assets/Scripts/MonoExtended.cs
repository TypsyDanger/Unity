using UnityEngine;

public class MonoExtended : MonoBehaviour {
	protected GameManager _gameManagerRef;
	protected string _className;

	protected virtual void Awake() {
		_className = this.GetType().Name;
		if (_className != "GameManager") {
			_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
	}

	protected void printStatus(string statusArg) {
		print(_className + string.Format(": {0}", statusArg));
	}
}
