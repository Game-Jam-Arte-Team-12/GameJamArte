using DG.Tweening;
using System;
using UnityEngine;

namespace Plugins.SoundManagerTool
{
	/// <summary>
	/// Custom behaviour class for Audio Source
	/// </summary>
	[ExecuteInEditMode]
	public class SoundSource : MonoBehaviour
	{
		#region Fields

		[Header("Base Properties")]
		[Tooltip("ID of the SoundSource")]
		[SerializeField] private string m_ID = "SoundSource_";

		[Tooltip("Type of sounds it plays")]
		[SerializeField] private SoundType m_type = SoundType.None;

		[Tooltip("Status of the SoundSource")]
		[SerializeField] private ActivityStatus m_status = ActivityStatus.None;

		private AudioSource m_audioSource = null;
		private SoundData m_currentSoundData = null;

		public string ID { get { return m_ID; } }
		public AudioSource AudioSource { get { return m_audioSource; } }
		public SoundType Type { get { return m_type; } set { m_type = value; } }
		public ActivityStatus Status { get { return m_status; } }

		public SoundData CurrentSoundData { get { return m_currentSoundData; } }

		// List of status when a SoundSource is playing a sound
		public static int[] IsPlayingStatus = new int[]
		{
			(int)ActivityStatus.FadeIn,
			(int)ActivityStatus.FadeOut,
			(int)ActivityStatus.Playing
		};

		// Check if the SoundSource is playing
		public bool IsPlaying
		{
			get
			{
				// /!\ If Utils not integrated, use lign below
				//return Utils.HasFlags((int)m_status, IsPlayingStatus);
				return HasFlags((int)m_status, IsPlayingStatus);
			}
		}

		// Will this SoundSource destroy itself when the clip is done playing
		public bool DestroyOnStop { get; set; }

		#endregion

		#region Methods

		#region MonoBehaviour

		protected virtual void OnEnable()
		{
			m_audioSource = GetComponent<AudioSource>();
			SoundManagerTool.RemoveSoundSource(this);
		}

		protected virtual void OnDrawGizmos()
		{
			if(!Application.isPlaying)
				m_ID = name;
		}

		protected virtual void Update()
		{
			if(!m_audioSource.isPlaying && IsPlaying)
				Stop();
		}

		#endregion

		#region Behaviour

		protected virtual void Destroy()
		{
			SoundManagerTool.RemoveSoundSource(this);
			Destroy(gameObject);
		}

		#endregion

		#region Control Behaviour

		public void Play()
		{
			if(m_audioSource.clip != null)
			{
				m_audioSource.Play();
				m_status = ActivityStatus.Playing;
			}
		}

		public void Pause()
		{
			if(m_audioSource.isPlaying)
			{
				m_audioSource.Pause();
				m_status = ActivityStatus.Pause;
			}
		}

		public void Stop()
		{
			m_audioSource.Stop();
			m_audioSource.clip = null;
			m_status = ActivityStatus.Stop;

			if(DestroyOnStop)
				Destroy();
		}

		/// <summary>
		/// Fades In the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="fromZero">Volume starts from 0 ?</param>
		/// <param name="callback">Callback when done fading</param>
		public void FadeIn(float duration, bool fromZero, Action callback = null)
		{
			m_status = ActivityStatus.FadeIn;

			if(fromZero)
				m_audioSource.volume = 0;

			m_audioSource.Play();

			float toVolume = m_currentSoundData.Volume;

			m_audioSource
				.DOFade(toVolume, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{

					if(callback != null)
						callback();

					m_status = ActivityStatus.Playing;

				});
		}


		/// <summary>
		/// Fades out the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="callback">Callback when done fading</param>
		public void FadeOut(float duration, Action callback = null)
		{
			m_status = ActivityStatus.FadeOut;

			//float fromVolume = m_audioSource.volume;
			float toVolume = m_currentSoundData.Volume;

			m_audioSource
				.DOFade(0, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{

					if(callback != null)
						callback();

					m_status = ActivityStatus.Stop;
					m_audioSource.Stop();

				});
		}

		#endregion

		#region Control Settings

		/// <summary>
		/// Set a SoundData to play
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void SetSoundData(SoundData data)
		{
			if(m_audioSource != null && data.Clip != null)
			{
				m_currentSoundData = data;
				m_audioSource.clip = data.Clip;
				m_audioSource.volume = data.Volume;
			}
		}

		public void SetLooping(bool isLooping)
		{
			m_audioSource.loop = isLooping;
		}

		public void SetVolume(float volume)
		{
			m_audioSource.volume = volume;
		}

		public void Unmute()
		{
			m_audioSource.volume = m_currentSoundData.Volume;
		}

		public void Mute()
		{
			SetVolume(0);
		}

		#endregion

		#region From Utils

		public static bool HasFlag(int a, int b)
		{
			return (a & b) == b;
		}

		public static bool HasFlags(int a, params int[] flags)
		{
			foreach(int flag in flags)
			{
				if(HasFlag(a, flag))
					return true;
			}
			return false;
		}

		#endregion

		#endregion


	}


}