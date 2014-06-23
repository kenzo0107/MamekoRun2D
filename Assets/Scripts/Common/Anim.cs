using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

	#region public members.
	public string Clip;
	#endregion public members.

	#region private members.
	private Animation anim;
	#endregion private members.

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start( ) {
		anim	= this.GetComponent<Animation>();
		anim.Play ( Clip );
	}
}
