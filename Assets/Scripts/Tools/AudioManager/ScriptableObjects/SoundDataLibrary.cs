using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Plugins.SoundManagerTool
{

	/// <summary>
	/// Collection of a specific type of SoundDatas
	/// </summary>
	[CreateAssetMenu(fileName = "SoundDataLibrary", menuName = "ScriptableObject/SoundDataLibrary", order = 1)]
	public class SoundDataLibrary : ScriptableObject
	{

		#region Fields

		[Tooltip("Short description of the library")]
		[SerializeField, TextArea(0, 10)] private string m_description = "";

		[Tooltip("Type of library")]
		[SerializeField] private SoundType m_type = SoundType.None;

		[Tooltip("Library data of SoundDatas")]
		[Space(40)]
		[SerializeField] private List<SoundData> m_soundDatas = null;

		public List<SoundData> SoundDatas { get { return m_soundDatas; } }
		public SoundType Type { get { return m_type; } }

		#endregion

		#region Methods

		/// <summary>
		/// Initialize the library
		/// </summary>
		public void Init()
		{
			// Set the type if all SoundType
			for(int i = 0; i < m_soundDatas.Count; i++)
				m_soundDatas[i].SetType(m_type);

		}

		#endregion

	}

}