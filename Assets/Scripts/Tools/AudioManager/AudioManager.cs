using UnityEngine;
using System.Collections;
using Plugins.SoundManagerTool;
using DG.Tweening;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{

	#region Fields

	#region Singleton 


	private static AudioManager m_instance = null;

	public static AudioManager Instance
	{
		get
		{
			return m_instance;
		}
		set
		{
			m_instance = value;
		}
	}

	#endregion

	[SerializeField] private string m_soundIDToTest = "LoopMusic";

	private const string TEST_IVA = "TestIva";

	[SerializeField] private int inIndexFade = 0;
	[SerializeField] private int indexFadeOut = 1;


	public const string MUSIC_MUS_ROOT = "Mus_";

	#endregion


	#region Methods

	private void OnEnable()
	{
		if(Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

	SoundManagerTool.Init(transform);
	}

	private void OnDestroy()
	{
		Destroy(this);
	}


	[ContextMenu("PlayTest")]
	public void PlayTest()
	{
		SoundManagerTool.PlaySound(m_soundIDToTest);
	}

	private void OnGUI()
	{
		return;
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

	[ContextMenu("ICIIII")]
	public void TestMusicFade()
	{
		BlendSound(indexFadeOut, inIndexFade, 5f);
	}

	[ContextMenu("TestTransition")]
	public void BlendSound(int indexFadeOut, int indexFadeIn, float blendDuration)
	{
		string fadeOutName = "MUS_" + indexFadeOut;
		string fadeInName = "MUS_" + indexFadeIn;

		Debug.Log(fadeOutName + " : " + fadeInName);

		SoundData fadeInMusic = SoundManagerTool.GetSoundData(fadeInName);

		Debug.Log(fadeInMusic.ID);

		SoundManagerTool.StopSound(fadeOutName, blendDuration);
		SoundManagerTool.PlaySound(fadeInName, blendDuration);
	}



	#endregion

}
