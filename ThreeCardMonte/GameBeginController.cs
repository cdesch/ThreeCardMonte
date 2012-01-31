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
		
		public GameBeginController ()
		{
			Log.Debug (classname + " init");
		}
		
		//Setup
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
			
			mNeedCheck = true;
			
			// Loop through all the cubes and set them up.
			lastIndex = 1;
			foreach (Cube cube in cubes) {
				
				GameCube wrapper = new GameCube (this, cube, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
			}
		}
		
		public void OnTick (float dt)
		{
			Log.Debug (classname + " OnTick");
			if (mNeedCheck) {
				mNeedCheck = false;
				bool t = CheckNeighbors ();
				//CheckSound (t);
				
			}

			foreach (GameCube wrapper in mWrappers) {
				//wrapper.Tick ();
				
				if (wrapper.mCubeSelected) {
					
					//For State Controller Imeplmentation
					//check if the a cube was previous selected 
					
					//Deselect the previous cube and select the curent one
				}
			}
			
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
			
		}
		
		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
	}
}

