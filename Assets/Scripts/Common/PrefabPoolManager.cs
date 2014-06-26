using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Prefab Poolクラス。
/// </summary>
[AddComponentMenu("Common/PrefabPoolManager")]
public class PrefabPoolManager : SingletonMonoBehaviour<PrefabPoolManager>{
	
	private Dictionary<string,PrefabFamiliy> prefabFamilies = new Dictionary<string, PrefabFamiliy>();
	///<summary>インスペクタ設定用. 必要なデータを保存する.</summary>
	public PreloadPrefabInfo[] preloadPrefabInfo;
	
	///<summary>
	/// 起動時にPreloadPrefabに設定されたObjectを指定個数確保.
	/// シーン遷移時にも追加で確保できるようにbase.Awake()前にロードする.
	/// このタイミングで行うことによって、廃棄される新しいシーンのMPrefabが.
	/// 前のシーンから引き継がれているMPrefabにアクセスできる.
	/// </summary>
	new void Awake(){
		PrefabPoolManager.Instance.PreloadPrefab( preloadPrefabInfo );
		base.Awake();
	}
	
	//----------------------------　LoadPrefab系　--------------------------//
	/// <summary>
	/// Loads the prefab.
	/// </summary>
	/// <returns><c>true</c>, if prefab was loaded, <c>false</c> otherwise.</returns>
	/// <param name="prefab">Prefab.</param>
	public bool LoadPrefab(GameObject prefab){
		if(prefabFamilies.ContainsKey(prefab.name))return false;
		prefabFamilies[prefab.name] = new PrefabFamiliy(prefab);
		return true;	
	}
	
	/// <summary>
	/// Loads the prefab.
	/// </summary>
	/// <returns><c>true</c>, if prefab was loaded, <c>false</c> otherwise.</returns>
	/// <param name="prefabPath">Prefab path.</param>
	public bool LoadPrefab(string prefabPath){
		//同名ファイルはロードさせない.
		string prefabName = GetPrefabName( prefabPath );
		if( prefabFamilies.ContainsKey( prefabName ) )	return false;
		prefabFamilies[prefabName] = new PrefabFamiliy( prefabPath );
		return true;
	}

	/// <summary>
	/// Unloads the prefab.
	/// </summary>
	/// <param name="prefabPath">Prefab path.</param>
	public void UnloadPrefab(string prefabPath){
		string prefabName = GetPrefabName( prefabPath );
		prefabFamilies[prefabName].Unload();
		prefabFamilies[prefabName]=null;
		Resources.UnloadUnusedAssets();
	}
	
	/// <summary>
	/// プレハブの一括確保を行い、バックグラウンドに待機させる.
	/// 現在新規prefabFamilyのみの適応可能。すでにロードされているけど、確保する量を増やすのは不可能.
	/// </summary>
	
	public void PreloadPrefab(PreloadPrefabInfo[] ppis){
		GameObject srcPrefab,instance;
		Transform _transform;
		int amount,i;
		string name;
		
		if(ppis.Length==0){
			return;
		}
		
		_transform= transform;

		foreach(PreloadPrefabInfo ppi in ppis){
			srcPrefab=ppi.prefab;
			amount = ppi.amount;
			name = srcPrefab.name;

			//すでに読み込んでいたら次へ.
			if(!LoadPrefab(srcPrefab))continue;
			
			for(i=0;i<amount;i++){
				instance= (GameObject)Instantiate(srcPrefab);
				instance.name = name;
				instance.transform.parent=_transform;
//				instance.SetActive(false);
				prefabFamilies[name].Add(instance);
				ReleasePrefab( instance );
			}
		}
		
	}
	
	//----------------------------  GetPrefab系　--------------------------//
	
	/// <summary>
	/// Gets the source prefab.
	/// Resources.Loadしたあとの一時保存に使いたいときなどに.
	/// NGUIだと独自プレファブ管理がどうもやりにくいための苦肉の策.
	/// </summary>
	/// <returns>
	/// The source prefab.(GameObject)
	/// </returns>
	/// <param name='prefabName'>
	/// Prefab name.
	/// </param>
	public GameObject GetSourcePrefab(string prefabPath){
		string prefabName = GetPrefabName(prefabPath);
		if(!prefabFamilies.ContainsKey(prefabName)){
			LoadPrefab(prefabName);
		}
		
		return prefabFamilies[prefabName].sourcePrefab;
	}
	
	
	public GameObject GetPrefab(GameObject prefab){
		Transform _transform = prefab.transform;
		string prefabName = prefab.name;
		return GetPrefab(prefabName,_transform.position,_transform.rotation);
		
	}

	/// <summary>
	/// ファイルパスからParefab取得.
	/// </summary>
	/// <returns>The prefab.</returns>
	/// <param name="prefabPath">Prefab path.</param>
	public GameObject GetPrefab(string prefabPath){
		Transform _transform = transform;
		string prefabName = GetPrefabName(prefabPath);
		return GetPrefab(prefabName,_transform.position,_transform.rotation);
	}

	/// <summary>
	/// ファイルパスからParefab取得, 位置設定.
	/// </summary>
	/// <returns>The prefab.</returns>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="_transform">_transform.</param>
	public GameObject GetPrefab(string prefabPath,Transform _transform){
		string prefabName = GetPrefabName(prefabPath);
		return GetPrefab(prefabName, _transform.position, _transform.rotation);
	}

	/// <summary>
	/// Gets the prefab.
	/// </summary>
	/// <returns>The prefab.</returns>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	public GameObject GetPrefab(string prefabPath, Vector3 position, Quaternion rotation){
		PrefabFamiliy familiy;
		GameObject prefab;
		string prefabName = GetPrefabName(prefabPath);

		if(!prefabFamilies.ContainsKey(prefabName)){
			LoadPrefab(prefabName);
		}
		
		//対象を一時変数に代入.
		familiy = prefabFamilies[prefabName];

		Debug.Log ( "familiy.releasedAmount=" + familiy.releasedAmount );

		//1.リリースしてあるInstanceがなければ新しくinstantiate.
		if(familiy.releasedAmount==0){
			prefab=(GameObject)Instantiate(familiy.sourcePrefab, position, rotation);
			prefab.name=prefabName;
			familiy.Add(prefab);
			return prefab;
		}
		
		
		//2.リリースしてあるのがあればそれを使う.
		if(familiy.releasedAmount>0){
			prefab = familiy.GetReleasedPrefab();
			prefab.transform.position=position;
			prefab.transform.rotation=rotation;
			prefab.SetActive(true);
			return prefab;
		}

		return null;
	}

	/// <summary>
	/// Instantiates the prefa.
	/// </summary>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	public void instantiatePrefab(string prefabPath, Vector3 position, Quaternion rotation){
		PrefabFamiliy familiy;
		GameObject prefab;
		string prefabName = GetPrefabName(prefabPath);

		if(!prefabFamilies.ContainsKey(prefabName)){
			LoadPrefab(prefabName);
		}
		
		//対象を一時変数に代入.
		familiy = prefabFamilies[prefabName];

		if(familiy.releasedAmount==0){
			prefab=(GameObject)Instantiate(familiy.sourcePrefab, position, rotation);
			prefab.name=prefabName;
			familiy.Add(prefab);
		}
		else { 
//		//2.リリースしてあるのがあればそれを使う.
//		if(familiy.releasedAmount>0){
			prefab = familiy.GetReleasedPrefab();
			Transform transformPrefab	= prefab.transform;
			transformPrefab.position	= position;
			transformPrefab.rotation	= rotation;
			prefab.SetActive(true);

			if ( prefab.GetComponent<ParticleSystem>() ) {
				StartCoroutine( WaitingForRelease( prefab, prefab.GetComponent<ParticleSystem>().duration ) );
			}
		}
	}

	/// <summary>
	/// Instantiates the prefab.
	/// </summary>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	/// <param name="parentTransfom">Parent transfom.</param>
	public void instantiatePrefab(string prefabPath, Vector3 position, Quaternion rotation, Transform parentTransfom ){
		PrefabFamiliy familiy;
		GameObject prefab;
		string prefabName = GetPrefabName(prefabPath);
		
		if(!prefabFamilies.ContainsKey(prefabName)){
			LoadPrefab(prefabName);
		}
		
		//対象を一時変数に代入.
		familiy = prefabFamilies[prefabName];
		
		if(familiy.releasedAmount==0){
			Debug.Log ( "prefabName=" + prefabName );
			prefab=(GameObject)Instantiate(familiy.sourcePrefab, position, rotation);
			prefab.name=prefabName;
			familiy.Add(prefab);
		}
		else { 
			prefab = familiy.GetReleasedPrefab();
			Transform transformPrefab	= prefab.transform;
			transformPrefab.parent		= parentTransfom;
			transformPrefab.position	= position;
			transformPrefab.rotation	= rotation;
			prefab.SetActive(true);

			if ( prefab.GetComponent<ParticleSystem>() ) {
				StartCoroutine( WaitingForRelease( prefab, prefab.GetComponent<ParticleSystem>().duration ) );
			}
		}
	}

	public void instantiatePrefab(string prefabPath, Vector3 position, Quaternion rotation, Transform parentTransfom, Vector3 scale ){
		PrefabFamiliy familiy;
		GameObject prefab;
		string prefabName = GetPrefabName(prefabPath);
		
		if(!prefabFamilies.ContainsKey(prefabName)){
			LoadPrefab(prefabName);
		}
		
		//対象を一時変数に代入.
		familiy = prefabFamilies[prefabName];
		
		if(familiy.releasedAmount==0){
			Debug.Log ( "prefabName=" + prefabName );
			prefab=(GameObject)Instantiate(familiy.sourcePrefab, position, rotation);
			prefab.name=prefabName;
			familiy.Add(prefab);
		}
		else { 
			prefab = familiy.GetReleasedPrefab();
			Transform transformPrefab		= prefab.transform;
			transformPrefab.parent			= parentTransfom;
			transformPrefab.localPosition	= position;
			transformPrefab.localRotation	= rotation;
			transformPrefab.localScale		= scale;
			prefab.SetActive(true);
			
			if ( prefab.GetComponent<ParticleSystem>() ) {
				StartCoroutine( WaitingForRelease( prefab, prefab.GetComponent<ParticleSystem>().duration ) );
			}
		}
	}
	/// <summary>
	/// Waitings for release.
	/// </summary>
	/// <returns>The for release.</returns>
	/// <param name="waitTime">Wait time.</param>
	IEnumerator WaitingForRelease( GameObject prefab, float waitTime ) {
		yield return new WaitForSeconds( waitTime );
		ReleasePrefab( prefab );
	}
	
	//----------------------------　Release　--------------------------//
	
	public bool ReleasePrefab(GameObject prefab){

		if ( prefab ) {
			return ReleasePrefab(prefab.name,prefab);
		}
		else {
			return true;
		}
	}
	
	
	public bool ReleasePrefab(string prefabPath,GameObject prefab){
		PrefabFamiliy family;
		string prefabName = GetPrefabName(prefabPath);
		
		if(!prefabFamilies.ContainsKey(prefabName)){
			Debug.LogWarning("Source Prefab is Not loaded");
			return false;
		}
		
		//解放するとき自分の子になおす.
		prefab.transform.parent=transform;
		family = prefabFamilies[prefabName];

		return family.Release(prefab);
	}
	
	
	//----------------------------　Util　--------------------------//
	
	/// <summary>
	/// Gets the name of the prefab.
	/// パスからプレハブ名を取得する.　末尾ファイル名を取り出すだけ.
	/// </summary>
	private string GetPrefabName(string prefabPath){
		string[] splited = prefabPath.Split("/"[0]);
		return splited[splited.Length-1];
	}
	
	//----------------------------　Utillity Class　--------------------------//
	
	//プレファブファミリィクラス。 private.
	//コピー元prefabと、それに連なる子供たちを保存する.
	class PrefabFamiliy{
		//----------------------------メンバ群.--------------------------//
		public GameObject sourcePrefab;
		public List<GameObject> instancies = new List<GameObject>();
		public List<bool> isUsing = new List<bool>();
		public int releasedAmount;
		public List<int> releasedIndex =new List<int>();
		
		//----------------------------コンストラクタ群.--------------------------//
		public PrefabFamiliy(){}
		
		public PrefabFamiliy( string prefabPath ) {
			sourcePrefab = (GameObject)Resources.Load( prefabPath );
		}
		
		public PrefabFamiliy( GameObject sourcePrefab ) {
			this.sourcePrefab = sourcePrefab;
		}
		
		
		
		//----------------------------　メンバ群　--------------------------//
		public void Unload(){
			sourcePrefab=null;
			instancies.Clear();
			isUsing=null;
			return;
		}

		/// <summary>
		/// Add the specified instance.
		/// </summary>
		/// <param name="instance">Instance.</param>
		public void Add(GameObject instance){
			
			instancies.Add(instance);
			isUsing.Add(true);
		}

		/// <summary>
		/// Release the specified instance.
		/// </summary>
		/// <param name="instance">Instance.</param>
		public bool Release(GameObject instance){
			int index;

			if(instancies.Contains(instance) ==false){
				return false;
			}
			index = instancies.IndexOf(instance);
			
			releasedAmount++;
			releasedIndex.Add(index);
			isUsing[index]=false;
			
			instancies[index].SetActive(false);
			return true;
		}

		/// <summary>
		/// Gets the released prefab.
		/// </summary>
		/// <returns>The released prefab.</returns>
		public GameObject GetReleasedPrefab(){
			int index = releasedIndex[0];
			GameObject target;
			
			target = instancies[index];
			target.SetActive(true);
			
			releasedAmount--;
			releasedIndex.RemoveAt(0);
			isUsing[index]=true;
			
			return target;
		}
	}
	
	//事前読み込みするファイルと量を設定.
	[Serializable]
	public class PreloadPrefabInfo{
		public GameObject prefab;
		public int amount;
	}
}