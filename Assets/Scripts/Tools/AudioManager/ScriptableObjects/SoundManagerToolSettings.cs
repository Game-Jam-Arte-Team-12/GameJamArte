using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Plugins.SoundManagerTool
{
	[CreateAssetMenu(fileName = "FileName", menuName = "ScriptableObject/SoundManagerToolSettings", order = 1)]
	public class SoundManagerToolSettings : ScriptableObject
	{
		[Tooltip("Base duration when fading in a sound")]
		[SerializeField] private float m_baseFadeInDuration = 1f;

		[Tooltip("Base duration when fading out a sound")]
		[SerializeField] private float m_baseFadeOutDuration = 1f;

		[SerializeField] private List<SoundDataLibrary> m_soundDataLibraries = null;


		public float BaseFadeInDuration { get { return m_baseFadeInDuration; } }
		public float BaseFadeOutDuration { get { return m_baseFadeOutDuration; } }

		public List<SoundDataLibrary> SoundDataLibraries { get { return m_soundDataLibraries; } }
	}

}