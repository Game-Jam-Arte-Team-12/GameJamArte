using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.AudioManagerTool
{
	/// <summary>
	/// Base behaviour class for every AudioManager
	/// </summary>
	public abstract class AAudioManager : MonoBehaviour
	{

		#region Fields

		#region SoundSources

		[Header("SoundSources - Behaviour class for AudioSource")]

		//[Tooltip("Prefab used to dynamicaly create a SoundSource")]
		//[SerializeField] protected SoundSource m_prefabSoundSource = new SoundSource();

		[Tooltip("Contains all SoundSources in the game")]
		[SerializeField] protected Transform m_soundSourceContainer = null;

		protected Dictionary<SoundType, List<SoundSource>> m_typeToSoundSources = new Dictionary<SoundType, List<SoundSource>>();

		#endregion

		#region SoundLibraries

		[Header("SoundDataLibraries - Collection of sounds to play")]
		[Space(20)]
		[Tooltip("List of all libraries of music & SFX")]
		[SerializeField] protected List<SoundDataLibrary> m_soundDataLibraries = null;

		#endregion

		#region Global Settings

		[Header("Global Settings")]
		[Space(20)]

		[Tooltip("Global duration for FadeIn")]
		[SerializeField] protected float m_fadeInBaseDuration = 2f;

		[Tooltip("Global duration for FadeOut")]
		[SerializeField] protected float m_fadeOutBaseDuration = 2f;

		protected Dictionary<SoundType, bool> m_soundTypeToMuteStatus = new Dictionary<SoundType, bool>();

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

		protected virtual void OnEnable()
		{
			Init();
		}

		#endregion

		#region Behaviour

		/// <summary>
		/// Initialize the AudioManager
		/// </summary>
		public virtual void Init()
		{

			// Initialize SoundDataLibraries
			foreach(SoundDataLibrary library in m_soundDataLibraries)
				library.Init();

			// Get all SoundSources and sort them by type
			if(m_soundSourceContainer == null)
				m_soundSourceContainer = transform;

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
		public virtual SoundSource PlaySound(string soundDataID)
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
		public virtual SoundSource PlaySound(string soundDataID, float fadeDuration = -1f)
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
				fadeDuration = m_fadeInBaseDuration;

			source.FadeIn(fadeDuration, true);

			return source;
		}


		public virtual void StopSound(string soundDataID)
		{
			List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

			foreach(SoundSource source in playingSources)
				source.Stop();
		}

		public virtual void StopSound(string soundDataID, float fadeDuration = -1)
		{
			List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

			foreach(SoundSource source in playingSources)
				source.FadeOut(fadeDuration);

		}

		/// <summary>
		/// Mute or Unmute SoundSources of specific type
		/// </summary>
		/// <param name="type"></param>
		/// <param name="muteOrUnmute"></param>
		public virtual void MuteUnmuteSoundSources(SoundType type, bool muteOrUnmute)
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
		protected SoundData GetSoundData(string ID)
		{
			SoundData sound = null;

			foreach(SoundDataLibrary library in m_soundDataLibraries)
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
		protected SoundSource GetNonPlayingSoundSource(SoundType type)
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
		protected List<SoundSource> GetSoundSources(SoundType type)
		{

			// If there is no SoundSource stored of this type, creates a new list, a new SoundSource and store it
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
		protected SoundSource CreateSoundSource(SoundType type)
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
			newSource.Init(this);


			// Store it
			if(m_typeToSoundSources.ContainsKey(type))
				m_typeToSoundSources[type].Add(newSource);
			else
				m_typeToSoundSources.Add(type, new List<SoundSource>() { newSource });

			return newSource;
		}

		/// <summary>
		/// Remove a stored SoundSource
		/// </summary>
		/// <param name="soundSource">Instance to remove</param>
		public void RemoveSoundSource(SoundSource soundSource)
		{
			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
				if(pair.Value.Contains(soundSource))
					pair.Value.Remove(soundSource);
		}

		/// <summary>
		/// Retrieve all SoundSources playing a specific sound, identified by its SoundID
		/// </summary>
		/// <param name="soundID">ID of the sound</param>
		/// <returns></returns>
		protected List<SoundSource> SoundSourcesPlayingSoundID(string soundID)
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