using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GlobalObjectHolder : MonoBehaviour
{
	const string objectName = "GlobalObjects";

	static GlobalObjectHolder instance;
	/// <summary>
	/// インスタンスを取得
	/// </summary>
	public static GlobalObjectHolder Instance
	{
		get
		{
			if(instance == null)
			{
				//無い場合は自身を作成する
				var go = new GameObject(objectName, typeof(GlobalObjectHolder));
				instance = go.GetComponent<GlobalObjectHolder>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	public void Add(GameObject gameObject)
	{
		gameObject.transform.SetParent(this.transform, false);
	}
}

