/// <summary>
/// Game begin controller.
/// Created by Chris Desch on 1/17/12.
/// Copyright (c) 2012 Desch Enterprises. All rights reserved.
/// </summary>
using System;
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace ThreeCardMonte
{
	public class GameBeginController : IStateController
	{
		String classname = "GameBeginController";
		public List<GameCube> mWrappers = new List<GameCube> (0);
		private bool mNeedCheck;
		private int lastIndex;
		public CubeSet cubes;
		private Color cubeBackground = new Color (255, 255, 0); //Yellow
		private Color mSelectColor = new Color (255, 0, 0); //Red
		
		ThreeCardMonte mApp;
		
		public GameBeginController (ThreeCardMonte app, CubeSet cubeSet)
		{
			Log.Debug (classname + " Init");
			mApp = app;
			cubes = cubeSet;
		}
		
		//Setup
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
			
			mNeedCheck = true;
			/*
			// Loop through all the cubes and set them up.
			lastIndex = 1;
			foreach (Cube cube in cubes) {
				
				GameCube wrapper = new GameCube (this, cube, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
			}*/
			
		}
		
		public void OnTick (float dt)
		{
			//Log.Debug (classname + " OnTick");
			
			//OnPaint (true);
		}
		
		private bool CheckNeighbors ()
		{
			bool found = false;
			int totalCubes = cubes.Count;
	
			// ### CubeHelper.FindRow ###
			// FindRow returns the first row found in the given cube set. It can be
			// used to check whether your cubes are all lined up.
			//
			// A row is a series of cubes neighbored **left to right**.  Cubes can only
			// form a row if they are all oriented the same way.
			Cube[] row = CubeHelper.FindRow (cubes);

			// If we have a full row, check to see if it is sorted by index.
			if (row.Length == totalCubes) {
				found = true;
				int lastId = -1;
				/*
				foreach (Cube cube in row) {
					CubeWrapper wrapper = (CubeWrapper)cube.userData;
					if (wrapper.mIndex < lastId)
						found = false;
					lastId = wrapper.mIndex;
				}*/
				/*
				//Is Cube Selected
				
				//Next State
				if (sm.CurrentState == menuController) {
					sm.CurrentState.OnDispose ();
					sm.CurrentState.OnSetup (MenuToGameBegin);
				}
				
				if (sm.CurrentState == gameBeginController) {
					
					//is there a selected cube
				}
				*/
			} else {
				//Cubes are not in row
				
			}
			
		
			// ### CubeHelper.FindColumn ###
			// FindColumn returns the first column found in the given cube set. It
			// can be used to check whether your cubes are all lined up.
			//
			// A column is a series of cubes neighbored **bottom to top**.  Cubes can
			// only form a column if they are all oriented the same way.
			
			Cube[] column = CubeHelper.FindColumn (cubes);
			// If we have a full column, check to see if it is sorted by index.
			if (column.Length == totalCubes) {
				found = true;
				int lastId = -1;
				foreach (Cube cube in column) {
					GameCube wrapper = (GameCube)cube.userData;
					if (wrapper.mIndex < lastId)
						found = false;
					lastId = wrapper.mIndex;
				}
			}

			// Here we go through each wrapper and update its state depending on the
			// results of our search.
			foreach (GameCube wrapper in mWrappers) {
				wrapper.CheckNeighbors (found);
			}

			return found;

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
							
							//DrawString (cube, 20, 10, "Begin Game");
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
		
		public void PaintCube (Cube cube)
		{
			//Paint this cube
			if (cube != null) {
				//cube.FillScreen (cubeBackground);
				cube.FillScreen (Color.Black);
				//cube.Image ("Spade_96x96-32", 0, 0, 0, 0, 96, 96, 1, 0);
				cube.Image ("buddy", 40, 24, 0, 48, 32, 48, 1, 0);
				cube.Paint ();	
			} else {
				//Handle this exception
			}
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

