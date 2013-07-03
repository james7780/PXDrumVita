/// <summary>
/// Pattern data for one pattern in PXDrum
/// </summary> 

using System;

namespace PXDrum
{
	public class DrumEvent
	{
		public byte vol;
		public byte pan;

		// constructor
		public DrumEvent()
		{
			Init();
		}
	
		public void Init()
		{
			vol = 0;
			pan = 128;
		}
		
		public void CopyFrom(ref DrumEvent de)
		{
			vol = de.vol;
			pan = de.pan;	
		}
	}

	public class Pattern
	{
		// Number of rows/columns in a pattern (public)
		public const int STEPS_PER_PATTERN = 16;
		public const int NUM_TRACKS = 8;
		public const int PATTERN_NAME_LENGTH = 32;		
		//UInt16[] data = new UInt16[NUM_TRACKS * STEPS_PER_PATTERN];
		public string name;
	    public DrumEvent[] drumEvents = new DrumEvent[NUM_TRACKS * STEPS_PER_PATTERN];

		
		public Pattern()
		{
			// Initialise data
			name = "";
			for (int i = 0; i < NUM_TRACKS * STEPS_PER_PATTERN; i++)
			{
				// drumEvents inited in theier constructor already
				//drumEvents[i].Init();
			}
			
		}
		
		public void Clear()
		{
			for (int i = 0; i < NUM_TRACKS; i++)
			{
				for (int j = 0; j < STEPS_PER_PATTERN; j++)
				{
				drumEvents[i * STEPS_PER_PATTERN + j].Init();
				}
			}
		}
		
		public void CopyFrom(ref Pattern pattern)
		{
			for (int i = 0; i < NUM_TRACKS; i++)
			{
				for (int j = 0; j < STEPS_PER_PATTERN; j++)
				{
					drumEvents[i * STEPS_PER_PATTERN + j].CopyFrom(ref pattern.drumEvents[i * STEPS_PER_PATTERN + j]);
				}
			}
		}

		public bool Read(System.IO.FileStream hStream)
		{
//			fread(&name, PATTERN_NAME_LENGTH * sizeof(char), 1, pfile);

			name = "";
			byte[] buffer = new byte[PATTERN_NAME_LENGTH];
			hStream.Read(buffer, 0, PATTERN_NAME_LENGTH);
			for (int i = 0; i < PATTERN_NAME_LENGTH; i++)
			{
				if (0 == buffer[i])
					break;
				
				name += (char)buffer[i];
			}

			for (int i = 0; i < NUM_TRACKS; i++)
			{
				for (int j = 0; j < STEPS_PER_PATTERN; j++)
				{
//				unsigned char vol;
//				unsigned char pan;
//				fread(&vol, sizeof(char), 1, pfile);
//				fread(&pan, sizeof(char), 1, pfile);
				hStream.Read(buffer, 0, 2);
				drumEvents[i * STEPS_PER_PATTERN + j].vol = buffer[0];
				drumEvents[i * STEPS_PER_PATTERN + j].pan = buffer[1];
				}
			}

			return true;
		}

		public void Write(System.IO.FileStream hStream)
		{
/*
			fwrite(&name, PATTERN_NAME_LENGTH * sizeof(char), 1, pfile);
			for (int i = 0; i < NUM_TRACKS; i++)
			{
				for (int j = 0; j < STEPS_PER_PATTERN; j++)
				{
				unsigned char vol = events[i][j].vol;
				unsigned char pan = events[i][j].pan;
				fwrite(&vol, sizeof(char), 1, pfile);
				fwrite(&pan, sizeof(char), 1, pfile);
				}
			}
*/		
		}
		
		
		
	}
}

