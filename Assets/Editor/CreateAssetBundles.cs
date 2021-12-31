using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
	[MenuItem("AssetBundles/Build")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory =Path.Combine(Application.streamingAssetsPath+"/Tank");
		if (!Directory.Exists(assetBundleDirectory))
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
										BuildAssetBundleOptions.None,
										BuildTarget.StandaloneWindows);
	}
}
