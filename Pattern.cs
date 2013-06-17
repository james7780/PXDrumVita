/// <summary>
/// Pattern data for one pattern in PXDrum
/// </summary> 

using System;

namespace PXDrum
{
	public class Pattern
	{
		// Number of rows/columns in a pattern (public)
		public const int NUMCOLUMNS = 16;
		public const int NUMROWS = 8;
		
		UInt16[] data = new UInt16[NUMROWS * NUMCOLUMNS];
		
		public Pattern ()
		{
			// Initialise data
			for (int i = 0; i < NUMROWS * NUMCOLUMNS; i++)
			{
				data[i] = 0;
			}
			
		}
	}
}

