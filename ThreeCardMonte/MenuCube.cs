// 
// MenuCube.cs
//  
// Author:
//       Christopher Desch <cdesch@gmail.com>
// 
// Copyright (c) 2012 Christopher Desch. All Rights Reserved. 
// 
using System;
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace ThreeCardMonte
{
	public class MenuCube
	{
		public MenuController mApp;
		public Cube mCube;
		public int mIndex;
		private int mSpriteIndex;
		private Color mRectColor;
		private Color mSelectColor;
		private int mRotation;
		public IStateController	mCubeStateController;
		public StateMachine mCubeStateMachine;

		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mCubeSelected = false;
		public bool mNeedDraw = true;

		public MenuCube (MenuController app, Cube cube, int seq)
		{
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mSpriteIndex = 0;
			mRectColor = new Color (36, 182, 255);
			mSelectColor = new Color (255, 0, 0);
			mRotation = 0;
			mCube.TiltEvent += OnTilt;
			mIndex = seq;
	
			mCube.ButtonEvent += HandleCubeButtonEvent;
		
			//mCubeStateController
		}
		//
		//Handle Button Event
		void HandleCubeButtonEvent (Cube c, bool pressed)
		{
			//Check if the button was pressed
			if (pressed) {
				
				//If this cube was already selected, turn it off
				if (mCubeSelected) {
					mCubeSelected = false;
				} else {
					mCubeSelected = true;
					
					//mCubeStateMachine.Transition()
					//Check if a neighboor cube is selected
					//If they are, tell them to not be. 
				}
				//Refresh the screen by setting this flag
				mNeedDraw = true;

			}
		}

		private void OnTilt (Cube cube, int tiltX, int tiltY, int tiltZ)
		{
      
			int oldRotation = mRotation;
			// If the cube is tilted to a standing position, set the sprite's
			// rotation so that its head is pointing towards that side.
			if (tiltZ == 1) {
				if (tiltY == 2) {
					mRotation = 0;
				} else if (tiltY == 0) {
					mRotation = 2;
				} else if (tiltX == 0) {
					mRotation = 1;
				} else if (tiltX == 2) {
					mRotation = 3;
				}
			} else {
				mRotation = 0;
			}
	
			// If the rotation has changed, raise the flag to force a repaint.
      
			if (mRotation != oldRotation) {
				mNeedDraw = true;
			}
		}

		// This method changes the background color depending on the game state.
		public void CheckNeighbors (bool rowFound)
		{
			if (mCube != null) {
				mSpriteIndex = 0;
				mRectColor = new Color (36, 182, 255);

				// ### CubeHelper.FindConnected ###
				// CubeHelper.FindConnected returns an array of all cubes that are
				// neighbors of the given cube, or neighbors of those neighbors, etc.
				// The result includes the given cube, so there should always be at
				// least one element in the array.
				//
				// Here we check to see if the cube is connected to any other cubes,
				// and if it is, we draw the orange background.
				Cube[] connected = CubeHelper.FindConnected (mCube);
				if (connected.Length > 1) {
					mSpriteIndex = 1;
					mRectColor = new Color (255, 145, 0);
				}

				if (rowFound) {
					mSpriteIndex = 2;
					mRectColor = new Color (182, 218, 85);
				}

				mNeedDraw = true;
      
			}
		}

		// This method is called every frame by the Tick in SorterApp. (see above.)
		public void Tick ()
		{
			// If anyone has raised the mNeedDraw flag, redraw the image on the cube.
			if (mNeedDraw) {
				Log.Debug ("mNeedDraw {0}", this.mCube.UniqueId);
				mNeedDraw = false;
		
				Paint ();
      
			}
		}

		public void Paint ()
		{
			
			Color bgColor = Color.White;
			
			if (mCube != null) {
				
				mCube.FillScreen (bgColor);
				mCube.FillRect (mRectColor, 40, 24, 48, 48);
				
				//Draw the selection icon if the cube has been selected
				if (mCubeSelected) {
					mCube.FillRect (mSelectColor, 25, 25, 30, 30);	
				}
				
				// ### Image rotation ###
				//
				// You can rotate an image by quarters. The rotation value is an
				// integer representing counterclockwise rotation.
				//
				// * 0 = no rotation
				// * 1 = 90 degrees counterclockwise
				// * 2 = 180 degrees
				// * 3 = 90 degrees clockwise
				//
				// When you rotate an image, the upper-left x/y position remains the
				// same. If the image does not have a square width-to-height ratio, an
				// image rotated 90 degrees will take up a different space than if it
				// were rotated 0 or 180.
				mCube.Image ("buddy", 40, 24, 0, mSpriteIndex * 48, 32, 48, 1, mRotation);

				mCube.FillRect (mRectColor, 40, 80, 48, 16);
				int startX = 64 - ((mIndex - 1) * 8 + 4) / 2;
        
				for (int i=0; i < mIndex; i++) {
					int x = startX + i * 8;
					mCube.FillRect (Color.Black, x, 82, 4, 12);
				}

				mCube.Paint ();
			
			}		
		}
	}	
}

