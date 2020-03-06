using UnityEngine;
using System.Collections;
using System;

namespace Plugins.SoundManagerTool
{

	/// <summary>
	/// Category of sound to play in an application
	/// </summary>
	public enum SoundType
	{
		None = 0,

		/// <summary>
		/// Concerns all ambiant musics
		/// </summary>
		Music = 1,

		/// <summary>
		/// SFX used inside the game/gameplay
		/// </summary>
		GameSFX = 2,

		/// <summary>
		/// SFX used inside the menus (buttons, feedback)
		/// </summary>
		MenuSFX = 4
	}

}