using System;
using UnityEngine;

namespace Plugins.AudioManagerTool
{
	/// <summary>
	/// Custom class for an AudioClip
	/// </summary>
	[Serializable]
	public class SoundData
	{
		[Header("Base Properties")]
		[SerializeField] private string m_ID;
		[SerializeField] private AudioClip m_clip;
		[SerializeField, Range(0, 1)] private float m_volume;
		private SoundType m_type;

		public string ID { get { return m_ID; } }
		public AudioClip Clip { get { return m_clip; } }
		public float Volume { get { return m_volume; } }
		public SoundType Type { get { return m_type; } set { m_type = value; } }

		public void SetType(SoundType type)
		{
			m_type = type;
		}

	}

}