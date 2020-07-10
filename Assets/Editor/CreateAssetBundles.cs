using UnityEditor;
using System.IO;
using UnityEngine;

namespace Game
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = Application.streamingAssetsPath;
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            // TODO: Buildtarget.StandaloneWindows -> BuildTarget.Standalone
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}