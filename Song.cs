/// <summary>
/// Song data for one song in PXDrum
/// </summary>

/*
// File format:
	// char[32]				songname
	// uchar vol
	// uchar bpm
	// char pitch
	// uint currentpattern
	// uint songpos
	// uint songList_length
	// uchar songlist[songList_length]
	// uint numPatterns
	// DrumPattern patterns[numPatterns]
	// uint numTracks
	// TrackMixInfo trackMixInfo[numTracks]
*/
using System;
using System.IO;

namespace PXDrum
{
	public class Song
	{
		private string m_fileName;
		
		public Song ()
		{
/*
#define SONG_NAME_LEN		32
#define PATTERNS_PER_SONG	100			// max length of song in patterns
#define MAX_PATTERN			50			// max number of patterns in a song
#define NO_PATTERN_INDEX	0xFF		// marker for "no pattern" in songlist

	unsigned char vol;					// song current vol
	unsigned char BPM;					// song current BPM
	char pitch;							// song current pitch offset
	char name[SONG_NAME_LEN];			// song name
	DrumPattern patterns[MAX_PATTERN];
	int songPos;						// current song position
	int currentPatternIndex;			// index of current pattern
	// song list (pattern indices)
	unsigned char songList[PATTERNS_PER_SONG];
	
	// track mix info
	TrackMixInfo trackMixInfo[NUM_TRACKS];

	// init songlist
	for (int i = 0; i < PATTERNS_PER_SONG; i++)
		songList[i] = NO_PATTERN_INDEX;

	// init pattern names
	for (int i = 0; i < MAX_PATTERN; i++)
		{
		sprintf(patterns[i].name, "P%d", i+1);
		}
*/
			
			
		}
		
		/// <summary>
		/// Load this song from disk.
		/// </summary> 
		public bool Load(string fileName)
		{
			if ( true == System.IO.File.Exists(fileName) )
			{
				using (System.IO.FileStream hStream = System.IO.File.Open(fileName, FileMode.Open))
				{
					if (hStream != null)
					{
						m_fileName = fileName;
						long size = hStream.Length;
						byte[] inputData = new byte[size];
						hStream.Read(inputData, 0, (int)size);
						//inputDataSize = (int)size;
						//string label = System.Text.Encoding.Unicode.GetString(inputData);
						//inputTextButton.SetText(label);
						hStream.Close();
						//eventAction = EVENT_LOAD;
						return true;
					}
				}
			}
			return false;
			
/*
	fread(&name, SONG_NAME_LEN * sizeof(char), 1, pfile);
	fread(&vol, sizeof(char), 1, pfile);
	fread(&BPM, sizeof(char), 1, pfile);
	fread(&pitch, sizeof(char), 1, pfile);
	currentPatternIndex = freadInt(pfile);
	songPos = freadInt(pfile);
	progressCallback(10);

	// read song list (sequence)
	int songListLength = freadInt(pfile);
	if (songListLength > PATTERNS_PER_SONG)
		{
		DoMessage(screen, bigFont, "Song Load Warning", "Song sequence length too long,\npossibly from later version.\n \nSong may be truncated.", false);
		fread(&songList[0], PATTERNS_PER_SONG * sizeof(char), 1, pfile);
		// skip over extra
		fseek(pfile, songListLength - PATTERNS_PER_SONG, SEEK_CUR);
		}
	else
		{
		fread(&songList[0], songListLength * sizeof(char), 1, pfile);
		}
	progressCallback(30);

	// read patterns
	int numPatterns = freadInt(pfile);
	if (numPatterns > MAX_PATTERN)
		{
		DoMessage(screen, bigFont, "Song Load Warning", "Too many patterns,\npossibly from later version.\n \nSome patterns may not be loaded.", false);
		for (int i = 0; i < MAX_PATTERN; i++)
			patterns[i].Read(pfile);
		// skip over extra
		DrumPattern tempPat;
		for (int i = 0; i < (numPatterns - MAX_PATTERN); i++)
			tempPat.Read(pfile);
		}
	else
		{
		for (int i = 0; i < numPatterns; i++)
			patterns[i].Read(pfile);
		}
	progressCallback(60);

	// read patterns
	int numTracks = freadInt(pfile);
	if (numTracks > NUM_TRACKS)
		{
		DoMessage(screen, bigFont, "Song Load Warning", "Too many tracks!\nSome tracks may not be loaded.", false);
		for (int i = 0; i < NUM_TRACKS; i++)
			trackMixInfo[i].Read(pfile);
		// skip over extra
		TrackMixInfo tempTrackInfo;
		for (int i = 0; i < (numTracks - NUM_TRACKS); i++)
			tempTrackInfo.Read(pfile);
		}
	else
		{
		for (int i = 0; i < numTracks; i++)
			trackMixInfo[i].Read(pfile);
		}

	progressCallback(100);
	
	fclose(pfile);
*/ 
			
		}
		
		/// <summary>
		/// Save this song to disk.
		/// </summary> 
		public bool Save()
		{
			// TODO
			return false;
		}
		
			// Insert a pattern into the songlist at the cuurent song pos
		public bool InsertPattern(int patternIndex)
		{
			return false;
		}
		
		// Remove the songlist entry at the current song pos
		public bool RemovePattern()
		{
			return false;	
		}
		
		
	}	// end class Song
}	// end namespace

