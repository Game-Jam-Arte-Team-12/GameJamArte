﻿using UnityEngine;
using System.Collections;
using System;


namespace Plugins.SoundManagerTool
{

    public enum ActivityStatus
	{
        None            =    0,
        FadeIn          =    1,
        FadeOut         =    2,
        Playing         =    4,
        Pause           =    8,
        Stop            =   16
    }

}