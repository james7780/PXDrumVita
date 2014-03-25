//
// Pattern grid renderer for PXDrum
// James Higgs 2013
// 

using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
//using Sce.PlayStation.Core.Imaging;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace PXDrum
{
	public class PatternGrid
	{
		static ShaderProgram shaderProgram;
		protected GraphicsContext graphics;
		
		// Vertex coordinates
		const int NUMVERTICES = 4 * Pattern.STEPS_PER_PATTERN * Pattern.NUM_TRACKS;
		float[] vertices = new float[NUMVERTICES * 3];
		
		// Texture coordinates
		float[] texcoords = new float[8 * Pattern.STEPS_PER_PATTERN * Pattern.NUM_TRACKS];
/* WAS
		float[] texcoords = {
			0.0f, 0.0f,	// left top
			0.0f, 1.0f,	// left bottom
			1.0f, 0.0f,	// right top
			1.0f, 1.0f,	// right bottom
		};
*/		
		// Vertex color
		float[] colours = new float[16 * Pattern.STEPS_PER_PATTERN * Pattern.NUM_TRACKS];
/*
		float[] colours = {
			1.0f,	1.0f,	1.0f,	1.0f,	// left top
			1.0f,	1.0f,	1.0f,	1.0f,	// left bottom
			1.0f,	1.0f,	1.0f,	1.0f,	// right top
			1.0f,	1.0f,	1.0f,	1.0f,	// right bottom
		};
*/		
		
		// Vertex buffer 
		VertexBuffer vertexBuffer;
		
		protected Texture2D texture;
		
		
		// Property cannot describe Position.X, public variable is used.
		public Vector2 origin ;
		
		public Vector2 Center;
		
		public Vector2 Scale=Vector2.One;
		
		protected Vector4 color=Vector4.One;

		float width, height;
		public float Width 
		{
			get { return width * Scale.X;}
		}
		public float Height 
		{
			get { return height * Scale.Y;}
		}

		public PatternGrid(GraphicsContext graphics, Texture2D texture, Vector2 position, Vector2 size)
		{
			if(shaderProgram == null)
			{
				//shaderProgram=CreateSimpleSpriteShader();
				shaderProgram = new ShaderProgram("/Application/shaders/Sprite.cgx");
				shaderProgram.SetUniformBinding(0, "u_WorldMatrix");
			}
			
			
			//if (texture == null)
			//{
			//	throw new Exception("ERROR: texture is null.");
			//}

			
			this.graphics = graphics;
			//this.texture = texture;
			this.texture = new Texture2D("/Application/resources/textures512.png", false);
			this.width = size.X;
			this.height = size.Y;
			this.origin = position;

			//@e                                                Vertex coordinate,               Texture coordinate,     Vertex color
			//vertexBuffer = new VertexBuffer(4, indexSize, VertexFormat.Float3, VertexFormat.Float2, VertexFormat.Float4);
			vertexBuffer = new VertexBuffer(NUMVERTICES, VertexFormat.Float3, VertexFormat.Float2, VertexFormat.Float4);
			
		}


		// Vertex color settings
		public void SetColor(Vector4 color)
		{
			SetColor(color.R, color.G, color.B, color.A);
		}
		
		// Vertex color settings
		public void SetColor(float r, float g, float b, float a)
		{
			this.color.R = r;
			this.color.G = g;
			this.color.B = b;
			this.color.A = a;
			
			for (int i = 0; i < colours.Length; i+=4)
			{
				colours[i] = r;
				colours[i+1] = g;
				colours[i+2] = b;
				colours[i+3] = a;
			}
		}
		
		// update tile data from the specified pattern
		public void Update(Pattern pattern)
		{
			// TODO
			const float tileSize = 1.0f / 16.0f;
			int vertIndex = 0;
			int texIndex = 0;
			for (int row = 0; row < Pattern.NUM_TRACKS; row++)
			{
				for (int column = 0; column < Pattern.STEPS_PER_PATTERN; column ++)
				{
					float x = tileSize * column;
					float y = tileSize * row;
					
					vertices[vertIndex + 0] = x;	// x0
					vertices[vertIndex + 1] = y;	// y0
					vertices[vertIndex + 2] = 0.0f;	// z0
			
					vertices[vertIndex + 3] = x;	// x1
					vertices[vertIndex + 4] = y + tileSize;	// y1
					vertices[vertIndex + 5] = 0.0f;	// z1
			
					vertices[vertIndex + 6] = x + tileSize;	// x2
					vertices[vertIndex + 7] = y;	// y2
					vertices[vertIndex + 8] = 0.0f;	// z2
			
					vertices[vertIndex + 9] = x + tileSize;	// x3
					vertices[vertIndex + 10] = y + tileSize;	// y3
					vertices[vertIndex + 11] = 0.0f;	// z3

					vertIndex += 12;

					// Set up texture coords (sprite) according to drumevent
					DrumEvent drumEvent = pattern.drumEvents[row * Pattern.STEPS_PER_PATTERN + column];
					x = 256.0f / 512;
					y = 256.0f / 512;
					float w = 48.0f / 512;
					float h = 48.0f / 512;
					if (drumEvent.vol > 20)
						x = 352.0f / 512;

					texcoords[texIndex] = x;
					texcoords[texIndex + 1] = y;

					texcoords[texIndex + 2] = x;
					texcoords[texIndex + 3] = y + h;

					texcoords[texIndex + 4] = x + w;
					texcoords[texIndex + 5] = y;

					texcoords[texIndex + 6] = x + w;
					texcoords[texIndex + 7] = y + h;

					texIndex += 8;
	
				}
				
			}
			
			for (int i = 0; i < 16 * Pattern.STEPS_PER_PATTERN * Pattern.NUM_TRACKS; i++)
			{
				colours[i] = 1.0f;	
			}
			
		}
		
		public void Render()
		{
			graphics.SetShaderProgram(shaderProgram);
			
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colours);
			
			//vertexBuffer.SetIndices(indices);
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0, texture);
			
			float screenWidth=graphics.Screen.Rectangle.Width;
			float screenHeight=graphics.Screen.Rectangle.Height;
			
			Matrix4 unitScreenMatrix = new Matrix4(
				 Width*2.0f/screenWidth,	0.0f,		0.0f, 0.0f,
				 0.0f,	 Width*(-2.0f)/screenHeight,	0.0f, 0.0f,
				 0.0f,	 0.0f, 1.0f, 0.0f,
				 -1.0f+(origin.X-Width * Center.X)*2.0f/screenWidth,  1.0f+(origin.Y-Height*Center.Y)*(-2.0f)/screenHeight, 0.0f, 1.0f
			);
			
			shaderProgram.SetUniformValue(0, ref unitScreenMatrix);

			//graphics.DrawArrays(DrawMode.TriangleStrip, 0, indexSize);
			graphics.DrawArrays(DrawMode.TriangleStrip, 0, 4, Pattern.NUM_TRACKS * Pattern.STEPS_PER_PATTERN);

		}

	}
}

