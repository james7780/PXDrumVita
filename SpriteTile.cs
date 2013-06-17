//
// Sprite tile for use when drawing sprite lists
// Represents geometry of one sprite
// James Higgs 2013
// 

using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
//using Sce.PlayStation.Core.Imaging;
using System.IO;
using System.Reflection;

namespace PXDrum
{
	public class SpriteTile
	{
		// Vertex coordinate
		float[] vertices=new float[12];
		
		
		// Texture coordinate
		float[] texcoords = {
			0.0f, 0.0f,	// left top
			0.0f, 1.0f,	// left bottom
			1.0f, 0.0f,	// right top
			1.0f, 1.0f,	// right bottom
		};
		
		// Vertex color
		float[] colors = {
			1.0f,	1.0f,	1.0f,	1.0f,	// left top
			1.0f,	1.0f,	1.0f,	1.0f,	// left bottom
			1.0f,	1.0f,	1.0f,	1.0f,	// right top
			1.0f,	1.0f,	1.0f,	1.0f,	// right bottom
		};
		
		// Index
		const int indexSize = 4;
		ushort[] indices;
		
		// Property cannot describe Position.X, public variable is used.
		//public Vector3 Position ;
		protected Vector4 colour=Vector4.One;

		float size;
		public float Size 
		{
			get { return size;}
		}

		public SpriteTile(Vector3 position, float size, Vector2 texCoord)
		{
			this.size = size;
			
			vertices[0] = position.X;	// x0
			vertices[1] = position.Y;	// y0
			vertices[2]=0.0f;	// z0
			
			vertices[3] = position.X;	// x1
			vertices[4] = position.Y + size;	// y1
			vertices[5]=0.0f;	// z1
			
			vertices[6] = position.X + size;	// x2
			vertices[7] = position.Y;	// y2
			vertices[8]=0.0f;	// z2
			
			vertices[9] = position.X + size;	// x3
			vertices[10] = position.Y + size;	// y3
			vertices[11]=0.0f;	// z3
			

			indices = new ushort[indexSize];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;
			indices[3] = 3;
			
			SetTextureCoord(texCoord);
			
			SetColour(1.0f, 1.0f, 1.0f, 1.0f);
	
		}


		// Vertex color settings
		public void SetColour(Vector4 color)
		{
			SetColour(color.R, color.G, color.B, color.A);
		}
		
		// Vertex color settings
		public void SetColour(float r, float g, float b, float a)
		{
			this.colour.R = r;
			this.colour.G = g;
			this.colour.B = b;
			this.colour.A = a;
			
			for (int i = 0; i < colors.Length; i+=4)
			{
				colors[i] = r;
				colors[i+1] = g;
				colors[i+2] = b;
				colors[i+3] = a;
			}
		}
		
		/// Set normalised textture coords
		public void SetTextureCoord(float x0, float y0)
		{
			texcoords[0] = x0;	// left top u
			texcoords[1] = y0; // left top v
			
			texcoords[2] = x0;	// left bottom u
			texcoords[3] = y0 + size;	// left bottom v
			
			texcoords[4] = x0 + size;	// right top u
			texcoords[5] = y0;	// right top v
			
			texcoords[6] = x0 + size;	// right bottom u
			texcoords[7] = y0 + size;	// right bottom v
		}


		/// Set normalised textture coords
		public void SetTextureCoord(Vector2 topLeft)
		{
			SetTextureCoord(topLeft.X, topLeft.Y);
		}
		
	}
}


