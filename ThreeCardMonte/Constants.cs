// 
// Constants.cs
//  
// Author:
//       Christopher Desch <cdesch@gmail.com>
// 
// Copyright (c) 2012 Christopher Desch. All Rights Reserved. 
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sifteo.Util;
using Sifteo;

namespace ThreeCardMonte
{
	public abstract class Constants
	{
		//Ints
		public static readonly int BLOCK_SIZE = 128;
		public static readonly int DEFAULT_ROYAL_BLUE_POWER = 850;
		
		//strings
		public static readonly string FLASH_STARTING_MESSAGE = "Preparing flash file for upload...";
		
		//Colors
		public static readonly Color transparentColor = new Color (72, 255, 170); 
	}
}

