using UnityEngine;
using System.Collections;
using Plugins.AudioManagerTool;

[ExecuteInEditMode]
public class AudioManager : AAudioManager
{

	[SerializeField] private string m_soundIDToTest = "LoopMusic";

	[ContextMenu("PlayTest")]
	public void PlayTest()
	{
		PlaySound("m_soundIDToTest");
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical();

		if(GUILayout.Button("Play Test SFX"))
			PlaySound("TestIva");

		if(GUILayout.Button("Stop Test SFX"))
			StopSound("TestIva");

		if(GUILayout.Button("FadeIn Test SFX"))
			PlaySound("TestIva", 2f);

		if(GUILayout.Button("FadeOut Test SFX"))
			StopSound("TestIva", 1f);

		GUILayout.EndVertical();
	}

}
