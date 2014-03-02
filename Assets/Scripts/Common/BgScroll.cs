using UnityEngine;
using System.Collections;

/// <summary>
/// 背景スクロール.
/// </summary>
public class BgScroll : MonoBehaviour {

	#region const members.
	const float SCROLL_SPEED	= 0.1f;
	#endregion const members.
	
	 /// <summary>
	 /// Update this instance.
	 /// </summary>
	void Update () { 
		renderer.material.mainTextureOffset = new Vector2 ( renderer.material.mainTextureOffset.x - Time.deltaTime * SCROLL_SPEED, renderer.material.mainTextureOffset.y - Time.deltaTime * SCROLL_SPEED );
	}
}
