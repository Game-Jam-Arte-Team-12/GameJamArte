using UnityEngine;
using System.Collections;
using Plugins.SoundManagerTool;

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{

	#region Fields
	
	[SerializeField] private string m_soundIDToTest = "LoopMusic";

	private const string TEST_IVA = "TestIva";

	#endregion


	#region Methods

	private void OnEnable()
	{
		SoundManagerTool.Init(transform);
	}


	[ContextMenu("PlayTest")]
	public void PlayTest()
	{
		SoundManagerTool.PlaySound("m_soundIDToTest");
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical();

		if(GUILayout.Button("Play Test SFX"))
			SoundManagerTool.PlaySound(TEST_IVA);

		if(GUILayout.Button("Stop Test SFX"))
			SoundManagerTool.StopSound(TEST_IVA);

		if(GUILayout.Button("FadeIn Test SFX"))
			SoundManagerTool.PlaySound(TEST_IVA, 2f);

		if(GUILayout.Button("FadeOut Test SFX"))
			SoundManagerTool.StopSound(TEST_IVA, 1f);

		GUILayout.EndVertical();
	}

	#endregion

}
