/// <summary>
/// Three card monte.
/// Created by Chris Desch on 1/17/12.
/// Copyright (c) 2012 Desch Enterprises. All rights reserved.
/// </summary>
using System;
using System.Collections.Generic;
using Sifteo;
using Sifteo.Util;

/// <summary>
/// Three card monte.
/// </summary>
namespace ThreeCardMonte
{
	public class ThreeCardMonte : BaseApp
	{
		private bool debuggingMode = true;
		public List<CubeWrapper> mWrappers = new List<CubeWrapper> (0);
		private bool mNeedCheck;
		private Sound mMusic;
		private int lastIndex;
		public Cube selectedCube = null;
		public StateMachine sm;
		
		
		//State MachineControllers
		private TitleController titleController;
		private MenuController menuController;
		private GameController gameController;
		private GameShuffleController gameShuffleController;
		private GameBeginController gameBeginController;
		private GameEndController gameEndController;	
		
		//States
		public static readonly string LAUNCH = "STARTUP";
		public static readonly string TitleState = "Title";
		public static readonly string MenuState = "Menu";
		public static readonly string GameBegin = "GameBegin";
		public static readonly string GameShuffle = "GameShuffle";
		public static readonly string GameEnd = "GameEnd";
		
		
		//Transition // Prefix with 't'
		public static readonly string tTitleToMenu = "TitleToMenu";
		public static readonly string tMenuToGameBegin = "MenuToGameBegin";
		public static readonly string tGameBeginToMenu = "GameBeginToMenu";
		public static readonly string tGameBeginToGameShuffle = "GameBeginToGameShuffle";
		public static readonly string tGameShuffleToGameEnd = "GameShuffleToGameEnd";
		public static readonly string tGameEndToMenu = "GameEndToMenu";
		//public static readonly string GameBeginToMenu = "GameBeginToMenu";
		
		//FrameRate
		override public int FrameRate {
			get { return 20; }
    
		}

		// called during intitialization, before the game has started to run
		override public void Setup ()
		{
			Log.Debug ("Setup()");
			mNeedCheck = true;
			
			System.Version myVersion = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version;		
			Log.Debug (myVersion.ToString ());
			
			//Init the State Machine
			sm = new StateMachine ();
			
			//Init the Controllers
			titleController = new TitleController ();
			menuController = new MenuController (this, CubeSet);
			gameController = new GameController ();
			gameBeginController = new GameBeginController (this, CubeSet);
			gameShuffleController = new GameShuffleController (this, CubeSet);
			gameEndController = new GameEndController (this, CubeSet);
			
			sm.State ("Title", titleController);
			sm.State ("Menu", menuController);
			sm.State ("Game", gameController);
			sm.State ("GameBegin", gameBeginController);
			sm.State ("GameShuffle", gameShuffleController);
			sm.State ("GameEnd", gameEndController);
	
			sm.Transition ("Title", "TitleToMenu", "Menu");
			sm.Transition ("Menu", tMenuToGameBegin, "GameBegin");
			sm.Transition ("GameBegin", tGameBeginToMenu, "Menu");  
			sm.Transition ("GameBegin", tGameBeginToGameShuffle, "GameShuffle");
			sm.Transition ("GameShuffle", tGameShuffleToGameEnd, "GameEnd");
			sm.Transition ("GameEnd", tGameEndToMenu, "Menu");
			
			//sm.SetState ("Title", "TitleToMenu");
			sm.SetState ("Menu", "MenuToGameBegin");
			
			// Loop through all the cubes and set them up.
			lastIndex = 1;
			
			foreach (Cube cube in CubeSet) {
				
				CubeWrapper wrapper = new CubeWrapper (this, cube, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
			}
			
			this.PauseEvent += OnPause;
			this.UnpauseEvent += OnUnpause;
			CubeSet.NewCubeEvent += OnNewCube;
			CubeSet.LostCubeEvent += OnLostCube;
			CubeSet.NeighborAddEvent += OnNeighborAdd;
			CubeSet.NeighborRemoveEvent += OnNeighborRemove;
 
		}
		
		private void testStateFunction ()
		{
			Log.Debug ("TestStateFunction");
		}		
		// development mode only
		// start Template as an executable and run it, waiting for Siftrunner to connect
		//static void Main(string[] args) { new ThreeCardMonte().Run(); }
		
			
		// ### Pause ###
		// When the users clicks Pause in Siftrunner, the game will stop ticking or
		// processing events, effectively bringing it to a stop.
		//
		// When Siftrunner pauses, the game is responsible for drawing a message to
		// the cubes' display to notify the user.
		private void OnPause ()
		{
			Log.Debug ("Pause!");
			foreach (Cube cube in CubeSet) {
				cube.FillScreen (Color.Black);
				cube.FillRect (Color.White, 55, 55, 6, 18);
				cube.FillRect (Color.White, 67, 55, 6, 18);
				cube.Paint ();
			}
    
		}
	
		// ### Unpause ###
		// When Siftrunner unpauses, we need to repaint everything that was
		// previously covered by the pause message.
		//
		// In this case, we can just set the mNeedCheck flag, and we'll repaint on
		// the next Tick.
		private void OnUnpause ()
		{
			Log.Debug ("Unpause.");
			mNeedCheck = true;
    
		}
	
		// ### New Cube ###
		// When a new cube connects while the game is running, we need to create a
		// wrapper for it so that it is included in gameplay.
		//
		// If a new cube is added while the game is paused, this event will be
		// handled after the player unpauses, but before the unpause event is
		// handled.
		private void OnNewCube (Cube c)
		{
			Log.Debug ("New Cube {0}", c.UniqueId);
			CubeWrapper wrapper = (CubeWrapper)c.userData;
			if (wrapper == null) {
				wrapper = new CubeWrapper (this, c, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
				Log.Debug ("{0}", mWrappers);
			}
			mNeedCheck = true;
    
		}
	
		// ### Lost Cube ###
		// When a cube falls offline while the game is running, we need to delete
		// its wrapper.
		//
		// If Siftrunner forced the to pause due to a cube going offline, this
		// event will be handled before the pause event is handled.
		private void OnLostCube (Cube c)
		{
			Log.Debug ("Lost Cube {0}", c.UniqueId);
			CubeWrapper wrapper = (CubeWrapper)c.userData;
			if (wrapper != null) {
				c.userData = null;
				mWrappers.Remove (wrapper);
			}
			mNeedCheck = true;
    
		}

		// Don't do any neighbor checking logic in these event handlers. Set a
		// flag on each wrapper, but wait until the next tick.
    
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			mNeedCheck = true;
    
		}
    
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			mNeedCheck = true;
    
		}

		public override void Tick ()
		{

			// Here we see if anyone raised the flag for a neighbor check; if so, we
			// do the check and play the appropriate sound depending on the result.
			if (mNeedCheck) {
				mNeedCheck = false;
				bool t = CheckNeighbors ();
				sm.CurrentState.OnPaint (true);
				CheckSound (t);
			}
			
			//Tick the current state
			//sm.CurrentState.OnTick (1);
			
			/*
			foreach (CubeWrapper wrapper in mWrappers) {
		
				wrapper.Tick ();
				
				//if the cube is selected but is not the currently selected cube, then turn it off. 
				if (wrapper.mCubeSelected && wrapper.mCube != selectedCube) {
					
					//set to false and redraw the cube to remove the marker.
					wrapper.mCubeSelected = false;
					wrapper.mNeedDraw = true;
					stateTransition (true);
					sm.Tick (1);
				}

			}*/
			
		}
	
		// This method scans the cubes and checks to see if they are neighbored in
		// a left-to-right row or a bottom-to-top column. If they are neighbored
		// correctly, it also checks to see if they are sorted correctly.
		private bool CheckNeighbors ()
		{
			bool found = false;
			int totalCubes = CubeSet.Count;
	
			// ### CubeHelper.FindRow ###
			// FindRow returns the first row found in the given cube set. It can be
			// used to check whether your cubes are all lined up.
			//
			// A row is a series of cubes neighbored **left to right**.  Cubes can only
			// form a row if they are all oriented the same way.
			Cube[] row = CubeHelper.FindRow (CubeSet);

			// If we have a full row, check to see if it is sorted by index.
			if (row.Length == totalCubes) {
				found = true;
				int lastId = -1;

				stateTransition (true);
				//Apply the transition
				sm.Tick (1);
				
			} else {
				//Cubes are not in row
				
				//Queue the transition
				stateTransition (false);
				//Apply the transition
				sm.Tick (1);
			}
			
			
			// ### CubeHelper.FindColumn ###
			// FindColumn returns the first column found in the given cube set. It
			// can be used to check whether your cubes are all lined up.
			//
			// A column is a series of cubes neighbored **bottom to top**.  Cubes can
			// only form a column if they are all oriented the same way.
			Cube[] column = CubeHelper.FindColumn (CubeSet);
			// If we have a full column, check to see if it is sorted by index.
			if (column.Length == totalCubes) {
				found = true;
				int lastId = -1;
				foreach (Cube cube in column) {
					CubeWrapper wrapper = (CubeWrapper)cube.userData;
					if (wrapper.mIndex < lastId)
						found = false;
					lastId = wrapper.mIndex;
				}
			}

			// Here we go through each wrapper and update its state depending on the
			// results of our search.
			foreach (CubeWrapper wrapper in mWrappers) {
				wrapper.CheckNeighbors (found);
			}

			return found;

		}

		// ### Sound ###
		// This method starts or stops the appropriate sounds depending on the
		// results of the search in CheckNeighbors.
		private void CheckSound (bool isCorrectlyNeighbored)
		{

			// If the cubes are lined up correctly, stop the music and play a sound
			// effect.
			if (isCorrectlyNeighbored) {

				if (mMusic != null) {
					// Stop the music if it is currently playing.
					if (mMusic.IsPlaying) {
						mMusic.Stop ();
					}
					mMusic = null;

					// To play a one-shot sound effect, just create a sound object and
					// call its Play method.
					//
					// The audio system will clean up the sound object after it is done,
					// so we don't have to hold on to a handle.
					Sound s = Sounds.CreateSound ("gliss");
					s.Play (1);
				}

			} else {
				if (mMusic == null) {

					// To play a looping sound effect or music track, create the sound
					// object and call its Play method with the extra argument to tell it
					// to loop.
					//
					// We hold on to the sound object after we play it so that we can
					// stop it later.
					
					mMusic = Sounds.CreateSound ("music");
          
					mMusic.Play (1, 1);
				
				}
			}
		}
		
		public void stateTransition (bool inRow)
		{
			
			//Start Conditions
			//End Conditions
			
			Log.Debug ("Start Transition");
			
			if (sm.CurrentState == menuController) {
				
				if (inRow) {
					//Begin the game
					sm.QueueTransition (tMenuToGameBegin);
				}
				
				Log.Debug (tMenuToGameBegin);
				
				
			} else if (sm.CurrentState == gameBeginController) {
				//GameBeginController
				/*
				if (selectedCube != null) {
						
				}
				*/
				//Are the cubes in a row?
				if (inRow) {
					
					//is a cube selected?
					if (selectedCube != null) {
						//Transition to shuffle state
						//sm.QueueTransition (tGameBeginToGameShuffle);
						//Wait
					} else {
						// Do Nothing
					}
					
				} else {
					
					if (selectedCube != null) {
						//Transition to shuffle state
						sm.QueueTransition (tGameBeginToGameShuffle);
					} else {
						//Go back to main menu
						sm.QueueTransition (tGameBeginToMenu);			
					}	
					
				}
			
			} else if (sm.CurrentState == gameShuffleController) {
				if (inRow) { 
					//Transition to game end
					sm.QueueTransition (tGameShuffleToGameEnd);
				} else {
					//Do Nothing Still Shuffling
				}
			} else if (sm.CurrentState == gameEndController) {
				
				//Tap to continue
				// Do nothing
				
			} else {
			
				//Handle unknown state
				Log.Debug ("Exception");
					
			}
			
		}
		
		public void StartState ()
		{
			Log.Debug ("StartingState");
		}
		
		public void OnCubeSelected (Cube cube)
		{
			Log.Debug ("Cube Selected!");
			mNeedCheck = true;
			
			if (sm.CurrentState == gameBeginController) {
				selectedCube = cube;
			} else if (sm.CurrentState == gameEndController) {
				sm.QueueTransition (tGameEndToMenu);
			}
		}
		
	}
	
	// ## CubeWrapper ##

	/// <summary>
	/// Cube wrapper.
	/// </summary>
  
	public class CubeWrapper
	{

		public ThreeCardMonte mApp;
		public Cube mCube;
		public int mIndex;
		private int mSpriteIndex;
		private Color mRectColor;
		private Color mSelectColor;
		private int mRotation;
		public IStateController	mCubeStateController;
		public StateMachine mCubeStateMachine;

		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mCubeSelected = false; //Was selected by the user on this tick
		public bool mIsSelected = false; //Is noted as the selected cube
		public bool mNeedDraw = true;

		public CubeWrapper (ThreeCardMonte app, Cube cube, int seq)
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
					mApp.OnCubeSelected (null);
				} else {
					mCubeSelected = true;
					//mApp.selectedCube = mCube;
					mApp.OnCubeSelected (mCube);
					
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
						
				//Paint ();
			}

		}

		public void Paint ()
		{
			
			Color bgColor = Color.Black;
			
			if (mCube != null) {
				
				//Draw the selection icon if the cube has been selected
				if (mCubeSelected) {
					//if (mApp.selectedCube == this) {
					
					//Draw Reactangle for the selected cube
					mCube.FillRect (mSelectColor, 6, 6, 116, 116);	
					mCube.FillRect (bgColor, 20, 20, 88, 88);	
				} else {
					//  Draw background color
					mCube.FillRect (bgColor, 6, 6, 116, 116);	
				}
				
				mCube.FillRect (mRectColor, 40, 24, 48, 48);
				
				
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
				//mCube.Image ("buddy", 40, 24, 0, mSpriteIndex * 48, 32, 48, 1, mRotation);

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
	


