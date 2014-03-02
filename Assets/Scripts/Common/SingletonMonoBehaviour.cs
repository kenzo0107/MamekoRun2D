using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType(typeof(T));
				
				if (instance == null) {
					Debug.LogError (typeof(T) + "is nothing");
				}
			}
			
			return instance;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected void Awake() {
		CheckInstance();
	}

	/// <summary>
	/// Checks the instance.
	/// </summary>
	/// <returns><c>true</c>, if instance was checked, <c>false</c> otherwise.</returns>
	protected bool CheckInstance() {
		if( this == Instance ){ return true;}
		Destroy(this);
		return false;
	}

}