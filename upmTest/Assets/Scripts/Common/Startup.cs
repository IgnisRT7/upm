using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	/// <summary>共有のオブジェクトがあるシーン名</summary>
	const string globalObjectName = "GlobalObjects";

	bool startup = false;

	/// <summary>初期化済みかどうか</summary>
	public bool IsStartup { get { return startup; } }

	IEnumerator Start()
	{
		//初期化チェック
		if (IsStartup)
		{
			Debug.LogWarning("Startup is already initialized.");
			Destroy(this.gameObject);
			yield break;
		}
		startup = true;

		//グローバルで管理されるオブジェクトの作成
		{
			yield return SceneManager.LoadSceneAsync(globalObjectName, LoadSceneMode.Additive);

			var scene = SceneManager.GetSceneByName(globalObjectName);
			var objects = scene.GetRootGameObjects();

			for (int i = 0, end = objects.Length; i < end; i++)
			{
				GlobalObjectHolder.Instance.Add(objects[i]);
			}
			SceneManager.UnloadSceneAsync(scene.name);
		}
	}

	/// <summary>
	/// シーン読み込み前に呼び出す処理
	/// <para>
	/// 他のコンポーネント系の処理は初期化されていないので注意
	/// </para>
	/// </summary>
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void StartUp()
	{
		//シーンの読み込み情報データを読み込む
		var startupScenes = Resources.Load("ScenesInfomation") as StartupSceneInfo;
		if(startupScenes != null)
		{
			if(startupScenes.IsTransitionToTheDefaultScene(SceneManager.GetActiveScene().path))
			{
				//デフォルトシーンへ遷移させる
				SceneManager.LoadSceneAsync(startupScenes.StartupSceneName, LoadSceneMode.Single);
			}
		}
	}
}
