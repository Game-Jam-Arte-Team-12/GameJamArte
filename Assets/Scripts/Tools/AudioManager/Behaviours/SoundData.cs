using System;
using UnityEngine;

namespace Plugins.SoundManagerTool
{
	/// <summary>
	/// Custom class for an AudioClip
	/// </summary>
	[Serializable]
	public class SoundData
	{
		[Header("Base Properties")]
		[SerializeField] private string m_ID = "";
		[SerializeField] private AudioClip m_clip = null;
		[SerializeField, Range(0, 1)] private float m_volume = 1f;
		private SoundType m_type = SoundType.None;

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