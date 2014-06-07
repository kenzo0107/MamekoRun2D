using UnityEngine;
using System.Collections;

/// <summary>
/// 背景スクロール.
/// </summary>
public class RendererScroll : MonoBehaviour {

	#region public members.
	/// <summary>水平方向にスクロールするスピード.</summary>
	public float ScrollSpeedLandScape; 
	/// <summary>垂直方向にスクロールするスピード.</summary>
	public float ScrollSpeedVertical;
	#endregion public members.

	#region private members.
	/// <summary>水平方向にスクロールするブレーキ速度.</summary>
	private float scrollLandScapeBrakeSpeed	= 0f;
	/// <summary>垂直方向にスクロールするブレーキ速度.</summary>
	private float scrollVerticalBrakeSpeed	= 0f;

	/// <summary>初期値 水平方向にスクロールするスピード.</summary>
	private float defaultScrollSpeedLandScape;
	/// <summary>初期値 垂直方向にスクロールするスピード.</summary>
	private float defaultScrollSpeedVertical;
	#endregion private members.

	private void Awake( ) {
		defaultScrollSpeedLandScape	= ScrollSpeedLandScape;
		defaultScrollSpeedVertical	= ScrollSpeedVertical;
	}

	 /// <summary>
	 /// Update this instance.
	 /// </summary>
	private void FixedUpdate () { 

		if ( Mathf.Abs( ScrollSpeedLandScape )	<	Mathf.Abs( defaultScrollSpeedLandScape / 10f ) )	return;
		if ( Mathf.Abs( ScrollSpeedVertical )	<	Mathf.Abs( defaultScrollSpeedVertical / 10f ) )		return;

		ScrollSpeedLandScape	-= scrollLandScapeBrakeSpeed;
		ScrollSpeedLandScape	-= scrollVerticalBrakeSpeed;

		renderer.material.mainTextureOffset = new Vector2 ( 
			renderer.material.mainTextureOffset.x - Time.deltaTime * ScrollSpeedLandScape,
			renderer.material.mainTextureOffset.y - Time.deltaTime * ScrollSpeedVertical
		);
	}

	/// <summary>
	/// ブレーキ.
	/// </summary>
	private void BrakeScroll( ) {
		scrollLandScapeBrakeSpeed	= ( ScrollSpeedLandScape >= 0 )?	-ScrollSpeedLandScape / 100f	:	ScrollSpeedLandScape / 100f;
		scrollVerticalBrakeSpeed	= ( ScrollSpeedVertical >= 0 )?		-ScrollSpeedVertical / 100f	:	ScrollSpeedVertical / 100f;
	}
}
