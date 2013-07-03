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
		static Song m_song = new Song();
		static Pattern currentPattern = new Pattern();
		private static PatternGrid patternGrid;
		static Text textWriter;
		static Timer clock;
		/// Tick count when we next have to advance the beat
		double nextProcessTick;
		
		// Thread control variables
		static bool isThreadAlive = true;
		static object syncObject = new object();
		static Thread playbackThread = null;
		
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
			
			m_song.Load("/Documents/songs/flowers1.xds");
			
			// Start playback thread
			//simStart = false;
			playbackThread = new Thread(new ThreadStart(playbackThreadMain));
			playbackThread.Start();
			
		
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
			//textWriter.Write(new Vector2(0.0f, 0.0f), "Hi James what's up???");
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		
		/// <summary>
		/// The playback thread
		/// </summary>
		static void playbackThreadMain()
		{
			Console.WriteLine("Playback thread start");
			
			// Are there any events which stop the program ???
			while(isThreadAlive)
			{
				// Process playback
				if(true) //simStart == true)
				{
					// Before changing myScene, 
					// it is necessary to avoid confliction with other thread					
					lock(syncObject)
					{
						//myScene.Simulate(clickIndex, clickPos, diffPos);
						// To avoid doing several times simulation within one frame,
						// set this flag as false
						//simStart = false;
					}
					
					Console.WriteLine("pb thread running");
					Thread.Sleep(1000);
				}
				else
				{
					// Nothing to do now, therefore it sleeps for a while
					Thread.Sleep(1);
				}
			}
			
			Console.WriteLine("Playback thread end");
		}
		
	}
}
