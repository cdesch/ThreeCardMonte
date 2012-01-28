using System;
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace ThreeCardMonte
{
	public class TitleController : IStateController
	{
		String classname = "TitleController";
		
		public TitleController ()
		{
			Log.Debug (classname + " Init");
		}
		
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
			Log.Debug (trainsitionId + " :TransitionId");
		}
		
		public void OnTick (float dt)
		{
			//Log.Debug (classname + " OnTick");
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

