using UnityEngine;
//using System.Collections;


public class TouchManager : SingletonMonoBehaviour<TouchManager> {

	/// <summary>
	/// Determines whether this instance is touch began.
	/// </summary>
	/// <returns><c>true</c> if this instance is touch began; otherwise, <c>false</c>.</returns>
	public static bool IsTouchBegan( ) {

#if !UNITY_EDITOR
		if ( Input.touchCount > 0 ) {
			
			foreach ( Touch touch in Input.touches ) {
				// タッチ or ムーブの場合.
				if ( Input.GetTouch(0).phase == TouchPhase.Began ) {
					return true;
				}
			}
		}
#else
		if ( Input.GetMouseButtonDown(0) ) {
			return true;
		}

#endif
		return false;
	}
}
