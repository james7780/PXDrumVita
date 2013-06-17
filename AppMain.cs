/// PXDrum Vita App
/// James Higgs 2013
/// <summary>
/// App main.
/// </summary>

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Input;
using System.Threading;

namespace PXDrum
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		static Sprite bgSprite;
		static Texture2D texture;
		static GamePadData gamePadData;
		static Pattern currentPattern = new Pattern();
		private static PatternGrid patternGrid;
		static Text textWriter;
		static Timer clock;
		/// Tick count when we next have to advance the beat
		double nextProcessTick;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			// Set up background sprite
			ImageRect rectScreen = graphics.Screen.Rectangle;
			Console.WriteLine("Screen size: " + rectScreen.Width + " x " + rectScreen.Height);
	
			texture = new Texture2D("/Application/resources/bg.png", false);
			bgSprite = new Sprite(graphics, texture);
			bgSprite.Position.X = 0.0f; //rectScreen.Width/2.0f;
			bgSprite.Position.Y = 0.0f; //rectScreen.Height/2.0f;
			bgSprite.Position.Z = 0.0f;
			
			patternGrid = new PatternGrid(graphics, texture, new Vector2(240.0f, 144.0f), new Vector2(720.0f, 400.0f));
			patternGrid.Update(currentPattern);
			
			textWriter = new Text(graphics, 16.0f);
			
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			bgSprite.Render();
			
			patternGrid.Render();
			
			textWriter.Write(new Vector2(10.0f, 10.0f), "Hi James whats up???");
			
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
