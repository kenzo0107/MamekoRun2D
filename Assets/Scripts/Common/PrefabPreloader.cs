using UnityEngine;
using System.Collections;

[AddComponentMenu("Common/PrefabPreloader")]
public class PrefabPreloader : MonoBehaviour {
	public PrefabPoolManager.PreloadPrefabInfo[] ppis;
	
	void Start(){
		PrefabPoolManager.Instance.PreloadPrefab(ppis);
	}
}