using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "scenesInfomation", menuName = "ScriptableObjects/CreateSceneInfomation")]
public class StartupSceneInfo : ScriptableObject
{
    [SerializeField]
    [Tooltip("デフォルトシーン名")]
    string startupSceneName;

    [SerializeField]
    [Tooltip("デフォルトシーンへ遷移させるシーンディレクトリ")]
    string defaultTransitionSceneDirectory = "Scenes";

    [SerializeField]
    [Tooltip("例外となるシーンディレクトリ名")]
    string[] excludedFromSceneTransitionNames;

    /// <summary>デフォルトで遷移するシーン名</summary>
    public string StartupSceneName { get { return startupSceneName; } }

    /// <summary>
    /// 指定したpathのディレクトリリストを取得
    /// </summary>
    void GetDirectoryList(string path, out string[] directories)
    {
        var directory = Path.GetDirectoryName(path);
        char[] seps = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        //ルートディレクトリを取り除いたものを返す
        directories = directory.Split(seps).Where(t => t != "Assets").ToArray();
    }

    /// <summary>
    /// デフォルトのシーンへ遷移させるかどうかを確認する
    /// </summary>
    public bool IsTransitionToTheDefaultScene(string activeSceneFullPath)
    {
        var scenes = EditorBuildSettings.scenes;

        string name = Path.GetDirectoryName(activeSceneFullPath);
        string[] directories = null;
        GetDirectoryList(activeSceneFullPath, out directories);

        //エディタ上での作成されたが、保存されていないシーン
        if(activeSceneFullPath =="")
        {
            return false;
		}

        //デフォルト遷移シーン名のnullチェック
        if(string.IsNullOrEmpty(defaultTransitionSceneDirectory))
        {
            return false;
		}

        if(excludedFromSceneTransitionNames != null && excludedFromSceneTransitionNames.Length > 0)
        {
            //例外的なシーンかどうか
            if (!excludedFromSceneTransitionNames.Any(a => directories.Any(b => a == b)))
            {
                return false;
            }
        }

        return true;
    }
}
