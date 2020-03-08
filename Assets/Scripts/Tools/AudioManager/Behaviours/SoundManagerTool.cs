using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.SoundManagerTool
{

	/*

		Note : Every word bewteen brackets ([like this]) is an implemented and usefull class


		[SoundManagerTool] is a static class used to Play/Stop/FadeIn/FadeOut musics and SFX
		It's coupled with a setting class nammed [SoundManagerToolSettings], which is located in the Resource Folder and stores a collection of [SoundDataLibrary].
		A [SoundDataLibrary] is like a library for [SoundData].
		[SoundData] is a custom class to store AudioClip of a specific sound, identified by an ID (string), along with other parameters.

		Basicaly [SoundManagerTool]'s role is to manage [SoundSources], mute or unmute them as you please.

		[SoundSource] is a custom class used to play a [SoundData].
		It can also Mute/Unmute [SoundSource] of the same type.

		Important note : 

		When you call [SoundManagerTool].PlaySound, the function return the [SoundSource] used to play the sound. You can then use it to alter its behaviour

	 */


	/// <summary>
	/// Control class
	/// </summary>
	public static class SoundManagerTool
	{

		#region Fields

		#region SoundSources

		public static Transform m_soundSourceContainer = null;

		private static Dictionary<SoundType, List<SoundSource>> m_typeToSoundSources = new Dictionary<SoundType, List<SoundSource>>();

		#endregion

		#region SoundLibraries

		private static SoundManagerToolSettings m_settings = null;

		private static List<SoundDataLibrary> SoundDataLibraries
		{
			get { return m_settings.SoundDataLibraries; }
		}

		#endregion

		#region Global Settings

		private static float BaseFadeInDuration
		{
			get
			{
				return m_settings.BaseFadeInDuration;
			}
		}

		private static float BaseFadeOutDuration
		{
			get
			{
				return m_settings.BaseFadeOutDuration;
			}
		}

		public static Dictionary<SoundType, bool> m_soundTypeToMuteStatus = new Dictionary<SoundType, bool>();

		#endregion

		#endregion

		#region Methods

		#region Behaviour

		/// <summary>
		/// Initialize the AudioManager
		/// </summary>
		public static void Init(Transform soundSourceContainer)
		{
			m_soundSourceContainer = soundSourceContainer;

			m_settings = Resources.Load("SoundManagerTool/SoundManagerToolSettings") as SoundManagerToolSettings;

			// Initialize SoundDataLibraries
			foreach(SoundDataLibrary library in SoundDataLibraries)
				library.Init();

			SoundSource[] soundSources = m_soundSourceContainer.GetComponentsInChildren<SoundSource>();
			int length = soundSources.Length;

			for(int i = 0; i < length; i++)
			{
				SoundSource soundSource = soundSources[i];

				if(!m_typeToSoundSources.ContainsKey(soundSource.Type))
					m_typeToSoundSources.Add(soundSource.Type, new List<SoundSource>() { soundSource });
				else
					m_typeToSoundSources[soundSource.Type].Add(soundSource);
			}

			// Creates a mute status for each SoundType
			m_soundTypeToMuteStatus.Clear();
			foreach(SoundType type in (SoundType[])Enum.GetValues(typeof(SoundType)))
				m_soundTypeToMuteStatus.Add(type, false);

		}

		#endregion

		#region Control

		/// <summary>
		/// Play a sound identified by it's ID
		/// </summary>
		/// <param name="soundDataID"> ID of the sound</param>
		/// <returns>SoundSource for specific behaviour</returns>
		public static SoundSource PlaySound(string soundDataID)
		{
			// Check if all libraries has this ID stored
			SoundData soundData = GetSoundData(soundDataID);
			if(soundData == null)
			{
				Debug.LogError(string.Format("Can't play SoundData with ID {0}, because it's null", soundDataID));
				return null;
			}

			// Find a non playing SoundSource to play the sound
			SoundSource source = GetNonPlayingSoundSource(soundData.Type);
			source.SetSoundData(soundData);
			source.Play();

			return source;
		}

		/// <summary>
		/// Play a sound identified by it's ID
		/// </summary>
		/// <param name="soundDataID"> ID of the sound</param>
		/// <param name="fadeDuration">FadeDuration. Not changing the value means it will use AAudioManager.m_fadeInBaseDuration</param>
		/// <returns>SoundSource for specific behaviour</returns>
		public static SoundSource PlaySound(string soundDataID, float fadeDuration = -1f)
		{
			// Check if all libraries has this ID stored
			SoundData soundData = GetSoundData(soundDataID);
			if(soundData == null)
			{
				Debug.LogError(string.Format("Can't play SoundData with ID {0}, because it's null", soundDataID));
				return null;
			}

			// Find a non playing SoundSource to play the sound
			SoundSource source = GetNonPlayingSoundSource(soundData.Type);

			source.SetSoundData(soundData);

			if(fadeDuration == -1)
				fadeDuration = BaseFadeInDuration;

			source.FadeIn(fadeDuration, true);

			return source;
		}

		public static void StopSound(string soundDataID)
		{
			List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

			foreach(SoundSource source in playingSources)
				source.Stop();
		}

		public static void StopSound(string soundDataID, float fadeDuration = -1)
		{
			List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

			if(fadeDuration == -1)
				fadeDuration = BaseFadeOutDuration;

			foreach(SoundSource source in playingSources)
				source.FadeOut(fadeDuration);

		}

		public static void BlendSound(int indexFadeOut, int indexFadeIn, float blendDuration)
		{
			string fadeOutName = "MUS_" + indexFadeOut;
			string fadeInName = "MUS_" + indexFadeIn;

			Debug.Log(fadeOutName + " : " + fadeInName);

			SoundData fadeInMusic = GetSoundData(fadeInName);

			Debug.Log(fadeInMusic.ID);

			StopSound(fadeOutName, blendDuration);
			PlaySound(fadeInName, blendDuration);
		}

		/// <summary>
		/// Mute or Unmute SoundSources of specific type
		/// </summary>
		/// <param name="type"></param>
		/// <param name="muteOrUnmute"></param>
		public static void MuteUnmuteSoundSources(SoundType type, bool muteOrUnmute)
		{
			List<SoundSource> sources = GetSoundSources(type);

			if(m_soundTypeToMuteStatus.ContainsKey(type))
				m_soundTypeToMuteStatus[type] = muteOrUnmute;

			foreach(SoundSource source in sources)
			{
				if(muteOrUnmute)
					source.Mute();
				else
					source.Unmute();
			}
		}


		#endregion

		#region SoundData

		/// <summary>
		/// Get the SoundData with given ID
		/// </summary>
		/// <param name="ID"> ID of the SoundData</param>
		/// <returns></returns>
		public static SoundData GetSoundData(string ID)
		{
			SoundData sound = null;

			foreach(SoundDataLibrary library in SoundDataLibraries)
			{
				sound = library.SoundDatas.Find(data => data.ID == ID);

				if(sound != null)
					return sound;
			}

			Debug.LogError(string.Format("There is no SoundData with ID {0} in libraries", ID));
			return null;
		}


		#endregion

		#region SoundSource


		/// <summary>
		/// Dynamicaly retrieve the last available SoundSource. If there is none availble, creates a new one and stores it
		/// </summary>
		/// <param name="soundSources"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static SoundSource GetNonPlayingSoundSource(SoundType type)
		{
			// First, we find the list of SoundSources of given type
			List<SoundSource> soundSources = GetSoundSources(type);
			SoundSource soundSource = null;

			// if there is none, we create a new one and store it
			if(soundSources.Count == 0)
				soundSource = CreateSoundSource(type);
			else
			{
				// We find the SoundSource currently available to play something
				soundSource = soundSources.Find(source => !source.IsPlaying);

				// If thre is no SoundSource available, we create a new one and store it
				if(soundSource == null)
					soundSource = CreateSoundSource(type);
			}

			return soundSource;
		}

		/// <summary>
		/// Get the list of SoundSource of given type
		/// </summary>
		/// <param name="type">SoundType of SoundSource</param>
		/// <returns></returns>
		public static List<SoundSource> GetSoundSources(SoundType type)
		{
			// If there is no SoundSource stored of this type, creates a new list, store it and a new SoundSource
			if(!m_typeToSoundSources.ContainsKey(type))
			{
				List<SoundSource> soundSources = new List<SoundSource>();
				m_typeToSoundSources.Add(type, soundSources);
				CreateSoundSource(type);

				return soundSources;
			}
			else
				return m_typeToSoundSources[type];
		}

		/// <summary>
		/// Create a new SoundSource and store it
		/// </summary>
		/// <param name="type">SoundType of SoundSource</param>
		/// <returns></returns>
		public static SoundSource CreateSoundSource(SoundType type)
		{
			// Instantiate & Initialize
			//SoundSource soundSource = Instantiate(m_prefabSoundSource, Vector3.zero, Quaternion.identity, m_soundSourceContainer);
			//soundSource.Type = type;
			//soundSource.name = string.Format("{0}_{1}", type.ToString(), GetSoundSources(type).Count);
			//soundSource.DestroyOnStop = true;
			//soundSource.Init(this);


			GameObject newObject = new GameObject();
			newObject.transform.parent = m_soundSourceContainer;
			newObject.AddComponent<AudioSource>();

			SoundSource newSource = newObject.AddComponent<SoundSource>();
			newSource.Type = type;
			newSource.name = string.Format("{0}_{1}", type.ToString(), GetSoundSources(type).Count);
			newSource.DestroyOnStop = true;

			AddSoundSource(newSource);

			return newSource;
		}

		/// <summary>
		/// Remove a stored SoundSource
		/// </summary>
		/// <param name="soundSource">Instance to remove</param>
		public static void RemoveSoundSource(SoundSource soundSource)
		{
			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
				if(pair.Value.Contains(soundSource))
					pair.Value.Remove(soundSource);
		}

		/// <summary>
		/// Store a SoundSource according to its type, to mute it maybe
		/// </summary>
		/// <param name="soundSource"></param>
		public static void AddSoundSource(SoundSource soundSource)
		{
			if(m_typeToSoundSources.ContainsKey(soundSource.Type))
				m_typeToSoundSources[soundSource.Type].Add(soundSource);
			else
				m_typeToSoundSources.Add(soundSource.Type, new List<SoundSource>() { soundSource });
		}

		/// <summary>
		/// Retrieve all SoundSources playing a specific sound, identified by its SoundID
		/// </summary>
		/// <param name="soundID">ID of the sound</param>
		/// <returns></returns>
		public static List<SoundSource> SoundSourcesPlayingSoundID(string soundID)
		{
			List<SoundSource> sources = new List<SoundSource>();

			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
			{
				sources.AddRange(pair.Value.FindAll(source => source.CurrentSoundData != null && source.CurrentSoundData.ID == soundID));
			}

			return sources;
		}


		#endregion

		#endregion
	}

}
