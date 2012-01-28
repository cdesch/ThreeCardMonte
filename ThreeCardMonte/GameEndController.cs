/// <summary>
/// Game end controller.
/// Created by Chris Desch on 1/17/12.
/// Copyright (c) 2012 Desch Enterprises. All rights reserved.
/// </summary>
using System;
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace ThreeCardMonte
{
	public class GameEndController : IStateController
	{
		String classname = "GameEndController";
		
		public GameEndController ()
		{
		}
				
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
		}
		
		public void OnTick (float dt)
		{
			Log.Debug (classname + " OnTick");
			
		}
		
		public void OnPaint (bool canvasDirty)
		{
			
		}
		
		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
	}
}

