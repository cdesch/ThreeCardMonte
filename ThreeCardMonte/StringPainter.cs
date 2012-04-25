// NumberPainter.cs
// 
// Copyright (c) 2012 Montclair State University
// 
// Contributors:  Christopher Desch <cdesch@gmail.com>
//				  Dr. Jerry Fails 	<failsj@mail.montclair.edu>
// 
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
using System;
using Sifteo;
using Sifteo.Util;
using System.Drawing;
using System.Collections.Generic;

namespace MathScramble
{
	public class StringPainter
	{
		Cube mCube;
		Sifteo.Color mRectColor = Constants.LightBlue;
		Dictionary<String, CubeImageHelper> imageHelperLookup;
		private System.Drawing.Color fontColor;
		private Sifteo.Color bgColor = Sifteo.Color.White;
		private int fontSize;
		/*
		 * Constructor
		 */ 
		public StringPainter (Cube cube, String label)
		{
			Log.Debug ("StringPainter {0}", label);
			mCube = cube;
		
			fontSize = 50;
			fontColor = System.Drawing.Color.Black;
			writeWord (label, mCube);
			
				
		}
		/*
		 * Constructor with color and size
		 */ 
		public StringPainter (Cube cube, String label, System.Drawing.Color color, int size)
		{
			Log.Debug ("StringPainter {0}", label);
			mCube = cube;
			
			fontSize = size;
			fontColor = color;
			writeWord (label, mCube);
				
		}
		
		/*
		 * Deconstructor
		 */ 
		~StringPainter ()
		{
			//Clean up
		}

		public CubeImageHelper getImageHelper (Cube c)
		{
			if (this.imageHelperLookup == null) {
				this.imageHelperLookup = new Dictionary<string, CubeImageHelper> ();
			}
			if (this.imageHelperLookup.ContainsKey (c.UniqueId))
				return this.imageHelperLookup [c.UniqueId];
			CubeImageHelper cih = new CubeImageHelper (c);
			this.imageHelperLookup.Add (c.UniqueId, cih);
			return cih;
		}
        
		public void writeWord (String word, Cube cube)
		{
			//get a helper for each cube
			CubeImageHelper cih = this.getImageHelper (cube);
            
			System.Drawing.Bitmap img = new System.Drawing.Bitmap (Cube.SCREEN_HEIGHT, Cube.SCREEN_WIDTH);
			System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage (img);
			gr.FillRectangle (new SolidBrush (System.Drawing.Color.White), new Rectangle (0, 0, 128, 128));
            
			//do some drawing with the graphics object
			//http://msdn.microsoft.com/en-us/library/system.drawing.aspx
                
			System.Drawing.Font f = new System.Drawing.Font ("Veranda", fontSize);
			System.Drawing.SolidBrush b = new System.Drawing.SolidBrush (fontColor);//fontColor);
            
			//draw the word in the center of the image
			SizeF sz = gr.MeasureString (word, f);
			System.Drawing.PointF p = new System.Drawing.PointF (64 - sz.Width / 2, 64 - sz.Height / 2);
            
			
			gr.DrawString (word, f, b, p);
			
			if (word.Equals ("6") || word.Equals ("9"))
				gr.FillRectangle (b, new Rectangle (4, 120, 120, 4));
			/*
			f = new System.Drawing.Font ("arial", 18);
			b = new System.Drawing.SolidBrush (System.Drawing.Color.Blue);
			sz = gr.MeasureString (word, f);
			p = new System.Drawing.PointF (128 - sz.Width, 128 - sz.Height);
            
			//draw the word in the bottom right of the image
			gr.DrawString (word, f, b, p);
            */
			//update the image helper
			cih.ScreenImage = img;
			//repaint the software buffer.
			cih.repaintChanged ();
            
			//paint the cube
			//cube.Paint ();
            
		}
		
		
	}
}

