// CubeImageHelper.cs
// 
// Copyright (c) https://sites.google.com/site/mikescoderama/Home/sifteo-drawing-api
// 
// Contributors:  https://sites.google.com/site/mikescoderama/Home/sifteo-drawing-api
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
using System.Drawing;
using Sifteo;
using System.Collections.Generic;

namespace MathHelper
{
	public class CubeImageHelper
	{
		private System.Drawing.Bitmap screenImage;
		private Cube cube;
		private Sifteo.Color[,] lastColors;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="CubeHelper.CubeImageHelper"/> class.
		/// </summary>
		/// <param name='c'>
		/// C.
		/// </param>
		public CubeImageHelper (Cube c)
		{
			this.cube = c;
			this.lastColors = null;
		}
		/// <summary>
		/// Gets or sets the cube's screen image.
		/// </summary>
		/// <value>
		/// The screen image.
		/// </value>
		public System.Drawing.Bitmap ScreenImage {
			get{ return this.screenImage;}
			set{ this.screenImage = value;}
		}
		/// <summary>
		/// Repaint the cube.
		/// </summary>
		public void repaint ()
		{
			this.repaint (new Rectangle (0, 0, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT));
		}
		/// <summary>
		/// Repaint the specified rect.
		/// </summary>
		/// <param name='rect'>
		/// Rect.
		/// </param>
		public void repaint (System.Drawing.Rectangle rect)
		{
			this.lastColors = this.bitmapToColorArray (this.screenImage);
			this.blitColors (this.cube, lastColors, null, rect);
		}
	
		/// <summary>
		/// Only repaint the pixels that were changed between now
		/// and the last call to repaint. if the cube's pixels have changed
		/// between now and the last call to repaint
		/// don't use this method.
		/// </summary>
		public void repaintChanged ()
		{
			Sifteo.Color[,] colors = this.bitmapToColorArray (this.screenImage);
			this.blitColors (this.cube, colors, this.lastColors,
				new Rectangle (0, 0, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT));
			this.lastColors = colors;
		}
		/// <summary>
		/// Convets Bitmaps to color Sifteo.Color array.
		/// </summary>
		/// <returns>
		/// The to color array.
		/// </returns>
		/// <param name='bitmap'>
		/// Bitmap.
		/// </param>
		public Sifteo.Color[,]  bitmapToColorArray (System.Drawing.Bitmap bitmap)
		{
			Sifteo.Color[,] colors = new Sifteo.Color[bitmap.Width, bitmap.Height];
			for (int i =0; i < bitmap.Width; i++) {
				for (int j=0; j < bitmap.Height; j++) {
					System.Drawing.Color c = bitmap.GetPixel (i, j);
					colors [i, j] = new Sifteo.Color (c.R, c.G, c.B);	
				}
			}
			return colors;
		}
		/// <summary>
		/// Builds the histogram of Sifteo.Colors.
		/// </summary>
		/// <returns>
		/// The histogram.
		/// </returns>
		/// <param name='colors'>
		/// Colors.
		/// </param>
		/// <param name='rect'>
		/// Area to build a histogram for
		/// </param>
		public Dictionary<byte,int> buildHistogram (Sifteo.Color[,]  colors,
			Rectangle rect)
		{
			Dictionary<byte,int > histo = new Dictionary<byte, int> ();
			for (int i =rect.Left; i < rect.Right; i++) {
				for (int j=rect.Top; j <rect.Bottom; j++) {
					Sifteo.Color c = colors [i, j];
					
					if (!histo.ContainsKey (c.Data)) {
						histo [c.Data] = 1;
					} else {
						histo [c.Data] = histo [c.Data] + 1;
					}
				}
			}
			return histo;
		}
		/// <summary>
		/// Gets the color of the most frequent Sifteo color in thes image.
		/// </summary>
		/// <returns>
		/// The most frequent color.
		/// </returns>
		/// <param name='colors'>
		/// Colors.
		/// </param>
		/// <param name='rect'>
		/// Rect.
		/// </param>
		public byte getMostFrequentColor (Sifteo.Color [,] colors, Rectangle rect)
		{
			Dictionary<byte,int> histo = buildHistogram (colors, rect);
			int max = 0;
			byte mostFrequent = 0;
			foreach (KeyValuePair<byte,int> pair in histo) {
				if (pair.Value > max) {
					mostFrequent = pair.Key;
					max = pair.Value;
				}
			}
			return mostFrequent;
		}
		/// <summary>
		/// Gets the color that is equivilant to the data value.
		/// This is kind of a hack becuase I can't construct a Sifteo.Color from a byte.
		/// </summary>
		/// <returns>
		/// The color.
		/// </returns>
		/// <param name='colors'>
		/// Colors.
		/// </param>
		/// <param name='b'>
		/// B.
		/// </param>
		private Sifteo.Color  getColor (Sifteo.Color [,] colors, byte b)
		{
			for (int i=0; i < colors.GetUpperBound(0); i++) {
				for (int  j=0; j< colors.GetUpperBound(1); j++) {
					if (colors [i, j].Data == b)
						return colors [i, j];
				}
			}
			return new Sifteo.Color (0);
		}

		public static int totalFilLRects = 0;
		/// <summary>
		/// Blits the colors.
		/// </summary>
		/// <param name='cube'>
		/// A Cube.
		/// </param>
		/// <param name='colors'>
		/// The colors to paint to the cube.
		/// </param>
		/// <param name='lastColorsSent'>
		/// Can be null.  If provided the cube only updates the changed colors.
		/// </param>
		/// <param name='rect'>
		/// Update area.
		/// </param>
		private void blitColors (Cube cube,
			Sifteo.Color [,] colors, 
			Sifteo.Color [,] lastColorsSent,
			Rectangle rect)
		{
			
			Sifteo.Color lastSentColor = Sifteo.Color.Mask;
			bool changedOnly = lastColorsSent != null;
	
			int lastStart = 0;
			int length = 0;
			int fillRects = 0;
			if (changedOnly) {
				//only set the changed colors
				//but try to reduce sets by writing in lines once
				//you find a single pixel that needs changed.
				for (int i =rect.Left; i <rect.Right; i++) {
					length = -1;
					for (int j=rect.Top; j< rect.Bottom; j++) {
						Sifteo.Color c = colors [i, j];
						
						//is this is different than the last color sent to this position?
						bool newColorToImage = c.Data != lastColorsSent [i, j].Data;
						
						//are we already making a line of this color?
						bool sameColorInLine = length > -1 && c.Data == lastSentColor.Data;
						
						if (newColorToImage && !sameColorInLine) {
							//are we already sending some data?
							if (length > -1) {
								fillRects++;
								cube.FillRect (lastSentColor, i, lastStart, 1, length);
							}
							//start here for the next color
							lastStart = j;
							length = 1;
							lastSentColor = c;
	
						} else if (sameColorInLine) {
							length++;
						} else {
							if (length > -1) {
								fillRects++;
								cube.FillRect (lastSentColor, i, lastStart, 1, length);
							}
							//not a new color, not a line. nothing going on
							length = -1;
						}
					}
					if (length > -1) {
						fillRects++;
						cube.FillRect (lastSentColor, i, lastStart, 1, length);
					}
				}
				totalFilLRects += fillRects;
			} else {
				Sifteo.Color background = Sifteo.Color.Black;
		
				//if we don't have any last sent colors
				//fill the background with the most frequent color
				byte mostFrequent = getMostFrequentColor (colors, rect);
				Sifteo.Color color = getColor (colors, mostFrequent);
				background = color;
				lastSentColor = color;
				cube.FillRect (color, rect.Left, rect.Top, rect.Right, rect.Bottom);
		
				for (int i =rect.Left; i <rect.Right; i++) {
					length = -1;
					for (int j=rect.Top; j< rect.Bottom; j++) {
						Sifteo.Color c = colors [i, j];
						
						//are we already making a line of this color?
						bool sameColorInLine = length > -1 && c.Data == lastSentColor.Data;
						bool newColor = c.Data != lastSentColor.Data;
						
						//start a line if this is not the background color.
						//and we did not already start a line.
						if ((newColor || lastSentColor.Data != background.Data) && !sameColorInLine) {
							if (length > -1) {
								cube.FillRect (lastSentColor, i, lastStart, 1, length);
								fillRects++;
							}
							lastStart = j;
							length = 1;
							lastSentColor = c;
						} else if (sameColorInLine) {
							length++;
						} else {
							lastSentColor = c;
							length = -1;
						}	
					}
					if (length > -1) {
						fillRects++;
						cube.FillRect (lastSentColor, i, lastStart, 1, length);
					}
				}
				totalFilLRects += fillRects;
			
			}
		}
	}
}

