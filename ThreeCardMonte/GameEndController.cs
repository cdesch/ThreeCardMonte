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
		public CubeSet cubes;
		private Color cubeBackground = new Color (0, 255, 255); //Purple
		
		ThreeCardMonte mApp;

		public GameEndController (ThreeCardMonte app, CubeSet cubeSet)
		{
			Log.Debug (classname + " Init");
			mApp = app;
			cubes = cubeSet;
		}
				
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
		}
		
		public void OnTick (float dt)
		{
			//Log.Debug (classname + " OnTick");
			
		}
		
		public void OnPaint (bool canvasDirty)
		{
			//Check the cube set
			if (cubes != null) {
				//Make sure the canvase needs to be redrawn
				if (canvasDirty) {
				
					//Cycle through all the cubes
					foreach (Cube cube in cubes) {
						
						//Paint the cube
						if (cube != null) {
							
							if (cube == mApp.selectedCube) {
								cube.FillScreen (mSelectColor);
								//cube.FillRect (mSelectColor, 0, 0, 128, 128);	
								cube.FillRect (cubeBackground, 6, 6, 116, 116);
							} else {
								cube.FillScreen (cubeBackground);	
							}
							cube.Paint ();
						} else {
							//Handle this exception
						}
					}
				
				} else {
					//Skip paint
					//return
				}
			} else {
				//Handle this exception
			}
		}

		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
		
		public static void DrawString (Cube c, int x, int y, String s)
		{
			int cur_x = x, cur_y = y;

			for (int i = 0; i < s.Length; ++i) {
				char ascii = s [i];

				// newlines
				if (s [i] == '\n') {
					cur_y += 10;
					cur_x = x;
				} else if (s [i] == ' ') {
					// blit the appropriate character
					cur_x += 6;	
				} else {
					// blit the appropriate character
					// note that for this example, the image is called "xterm610";
					// if you want multiple fonts, you may want to pass in the image name as a parameter to this function
					c.Image ("xterm610", cur_x, cur_y, (ascii % 16) * 6, (ascii / 16) * 10, 6, 10, 1, 0);
					cur_x += 6;
				}
			}
		}
	}
}

