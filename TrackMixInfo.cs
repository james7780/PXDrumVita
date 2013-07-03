/// <summary>
/// Track Mix Info for one track in PXDrum
/// </summary>
/// 
using System;

namespace PXDrum
{
	public class TrackMixInfo
	{
		enum TRACKSTATE { TS_ON = 0, TS_MUTE = 1, TS_SOLO = 2 };

		byte vol;
		byte pan;
		byte state;		// current track state
		byte prevState;	// state before last solo enable

		public TrackMixInfo ()
		{
			Init();
		}
		
		private void Init()
		{
			vol = 255;				// full volumne 
			pan = 128;				// centrepan
			state = (byte)TRACKSTATE.TS_ON;			// track is not muted or solo'd
			prevState = (byte)TRACKSTATE.TS_ON;		// track is not muted or solo'd
		}
		
		public bool Read(System.IO.FileStream hStream)
		{
//			fread(&vol, sizeof(char), 1, pfile);
//			fread(&pan, sizeof(char), 1, pfile);
//			fread(&state, sizeof(char), 1, pfile);
//			fread(&prevState, sizeof(char), 1, pfile);
			if (null == hStream)
				return false;
			
			byte[] buffer = new byte[4];
			hStream.Read(buffer, 0, 4);
			vol = buffer[0];
			pan = buffer[1];
			state = buffer[2];
			prevState = buffer[3];
			
			return true;
		}
		
		void Write(System.IO.FileStream hStream)
		{
//			fwrite(&vol, sizeof(char), 1, pfile);
//			fwrite(&pan, sizeof(char), 1, pfile);
//			fwrite(&state, sizeof(char), 1, pfile);
//			fwrite(&prevState, sizeof(char), 1, pfile);
			if (null == hStream)
				return;
			
			byte[] buffer = new byte[4] {vol, pan, state, prevState};
			hStream.Write(buffer, 0, 4);
		}

		
	}	// end class
}	// end namespace

