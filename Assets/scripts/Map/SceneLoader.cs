using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// 이 친구는 싱글톤으로 관리가 가능한 씬 로더 입니다.
/// 간단히 두개 이상의 씬을 병합하는 기능을 담당합니다.
public class SceneLoader : MonoBehaviour {
	public static SceneLoader instance;
	void Start () 
	{
		if(instance == null) instance = this;
		DontDestroyOnLoad(instance);

	}

	public static void LoadScenes(string[] sceneName)
	{
		//TODO
	}
	
}
