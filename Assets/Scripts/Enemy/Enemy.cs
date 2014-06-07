using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private void OnBecameVisible () {
		Debug.Log ( "OnBecameVisible" );
		this.gameObject.SetActive( true );
	}

	private void OnBecameInvisible() {
		Debug.Log ( "OnBecameInvisible" );
		this.gameObject.SetActive( false );
	}

}
