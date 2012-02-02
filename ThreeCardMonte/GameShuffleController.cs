/// <summary>
/// Game shuffle controller.
/// Created by Chris Desch on 1/17/12.
/// Copyright (c) 2012 Desch Enterprises. All rights reserved.
/// </summary>
/// 
using System;
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace ThreeCardMonte
{
	public class GameShuffleController : IStateController
	{
		String classname = "GameShuffleController";
		public List<ShuffleCube> mWrappers = new List<ShuffleCube> (0);
		private bool mNeedCheck;
		private int lastIndex;
		public CubeSet cubes;
		private Color cubeBackground = new Color (0, 0, 0); //Black
		
		
		ThreeCardMonte mApp;
		//Init
		public GameShuffleController (ThreeCardMonte app, CubeSet cubeSet)
		{
			Log.Debug (classname + " Init");
			mApp = app;
			cubes = cubeSet;
		}
			
		//Setup
		public void OnSetup (string trainsitionId)
		{
			
			Log.Debug (classname + " OnSetup");
			
			/*
			mNeedCheck = true;
			
			// Loop through all the cubes and set them up.
			lastIndex = 1;
			foreach (Cube cube in cubes) {
				
				ShuffleCube wrapper = new ShuffleCube (this, cube, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
			}*/
		}
		
		public void OnTick (float dt)
		{
			//Log.Debug (classname + " OnTick");
			
			
			//OnPaint (true);
			/*
			// Here we see if anyone raised the flag for a neighbor check; if so, we
			// do the check and play the appropriate sound depending on the result.
			if (mNeedCheck) {
				mNeedCheck = false;
				bool t = CheckNeighbors ();
				//CheckSound (t);
			}
			

			foreach (MenuCube wrapper in mWrappers) {
				//wrapper.Tick ();
				
				if (wrapper.mCubeSelected) {
					
					//For State Controller Imeplmentation
					//check if the a cube was previous selected 
					
					//Deselect the previous cube and select the curent one
				}
			}
			*/
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
					ShuffleCube wrapper = (ShuffleCube)cube.userData;
					if (wrapper.mIndex < lastId)
						found = false;
					lastId = wrapper.mIndex;
				}
			}

			// Here we go through each wrapper and update its state depending on the
			// results of our search.
			foreach (ShuffleCube wrapper in mWrappers) {
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
							cube.FillScreen (cubeBackground);
							//DrawString (cube, 20, 10, "SHUFFFLE!");
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

