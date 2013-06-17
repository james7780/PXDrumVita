//
// Text renderer for PXDrum
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
	public class Text
	{
		static ShaderProgram shaderProgram;
		protected GraphicsContext graphics;
		
		// Max  number of characters in a line
		const int MAXCHARS = 100;
		
		// Vertex coordinates
		const int NUMVERTICES = 4 * MAXCHARS;
		float[] vertices = new float[NUMVERTICES * 3];
		
		// Texture coordinates
		float[] texcoords = new float[8 * MAXCHARS];
	
		// Vertex color
		float[] colours = new float[16 * MAXCHARS];
		
		// Vertex buffer 
		VertexBuffer vertexBuffer;
		
		protected Texture2D fontTexture;
		
		
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

		public Text(GraphicsContext graphics, float charHeight)
		{
			if(shaderProgram == null)
			{
				//shaderProgram=CreateSimpleSpriteShader();
				shaderProgram = new ShaderProgram("/Application/shaders/Sprite.cgx");
				shaderProgram.SetUniformBinding(0, "u_WorldMatrix");
			}
			
			if (fontTexture == null)
			{
				fontTexture = new Texture2D("/Application/resources/font_8x16.png", false);
			}
			
			this.graphics = graphics;
			this.width = charHeight;
			this.height = charHeight;
			//this.origin = position;

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
		
		// Write text to the screen
		public void Write(Vector2 position, string text)
		{
			// TODO
			this.origin = position;
			
			const float tileSize = 1.0f / 32.0f;
			int vertIndex = 0;
			int texIndex = 0;
			float x = 0.0f;
			float y = 0.0f;
			for (int i = 0; i < text.Length; i ++)
			{
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
				x += tileSize;
				
				char c = text[i];
				float tx = tileSize * (c % 32);		// 32 chars per row in the texture
				float ty = tileSize * (c / 32);
				
				texcoords[texIndex] = tx;
				texcoords[texIndex + 1] = ty;

				texcoords[texIndex + 2] = tx;
				texcoords[texIndex + 3] = ty + tileSize;

				texcoords[texIndex + 4] = tx + tileSize;
				texcoords[texIndex + 5] = ty;

				texcoords[texIndex + 6] = tx + tileSize;
				texcoords[texIndex + 7] = ty + tileSize;

				texIndex += 8;
			}
			
			for (int i = 0; i < 16 * MAXCHARS; i++)
			{
				colours[i] = 1.0f;	
			}
			
			// And render it
			Render();
		}
		
		public void Render()
		{
			graphics.SetShaderProgram(shaderProgram);
			
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colours);
			
			//vertexBuffer.SetIndices(indices);
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0, fontTexture);
			
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
			graphics.DrawArrays(DrawMode.TriangleStrip, 0, 4, MAXCHARS);

		}
				
	}
}

