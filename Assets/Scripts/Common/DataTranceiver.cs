using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataTranceiver : SingletonMonoBehaviour<DataTranceiver> {

	/// <summary>ローディングオブジェクト.</summary>
	private GameObject	objLoading;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {
		objLoading	= transform.FindChild( "Loading" ).gameObject;
		DontDestroyOnLoad( this );
	}

	/// <summary>
	/// GET通信.
	/// </summary>
	/// <param name="url">URL.</param>
	public WWW Request( string url ) {
		WWW www = new WWW ( url );
		StartCoroutine ( WaitForRequest ( www ) );
		return www;
	}
	
	/// <summary>
	/// POST通信.
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="post">POSTデータ.</param>
	public WWW Request( string url, Dictionary<string,string> post ) {
		WWWForm form = new WWWForm();
		foreach( KeyValuePair<string,string> postArg in post ) {
			form.AddField( postArg.Key, postArg.Value );
		}
		WWW www = new WWW( url, form );
		StartCoroutine( WaitForRequest( www ) );
		return www;
	}

	/// <summary>
	/// リクエスト待機状態.
	/// </summary>
	/// <returns>The for request.</returns>
	/// <param name="www">Www.</param>
	private IEnumerator WaitForRequest( WWW www ) {

		objLoading.SetActive( true );

		yield return new WaitForSeconds( 3f );

		yield return www;

		// 正常系.
		if ( www.error == null ) {
			Debug.Log("WWW Ok!: " + www.text);
		}
		// 異常系.
		else {
			Debug.Log("WWW Error: "+ www.error);
		}
		objLoading.SetActive( false );
	}
}
