//
// Sprite blitter for PXDrum
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
	public class Sprite
	{
		static ShaderProgram shaderProgram;
		protected GraphicsContext graphics;

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
		
		// Vertex buffer 
		VertexBuffer vertexBuffer;
		
		protected Texture2D texture;
		
		
		// Property cannot describe Position.X, public variable is used.
		public Vector3 Position ;
		
		public Vector2 Center;
		
		public Vector2 Scale=Vector2.One;
		
		protected Vector4 color=Vector4.One;
		

		float width,height;
		public float Width 
		{
			get { return width * Scale.X;}
		}
		public float Height 
		{
			get { return height * Scale.Y;}
		}

		public Sprite(GraphicsContext graphics, Texture2D texture)
		{
			if(shaderProgram == null)
			{
				//shaderProgram=CreateSimpleSpriteShader();
				shaderProgram = new ShaderProgram("/Application/shaders/Sprite.cgx");
				shaderProgram.SetUniformBinding(0, "u_WorldMatrix");
			}
			
			
			if (texture == null)
			{
				throw new Exception("ERROR: texture is null.");
			}
			
			this.graphics = graphics;
			this.texture = texture;
			this.width = texture.Width;
			this.height = texture.Height;
			
			vertices[0]=0.0f;	// x0
			vertices[1]=0.0f;	// y0
			vertices[2]=0.0f;	// z0
			
			vertices[3]=0.0f;	// x1
			vertices[4]=1.0f;	// y1
			vertices[5]=0.0f;	// z1
			
			vertices[6]=1.0f;	// x2
			vertices[7]=0.0f;	// y2
			vertices[8]=0.0f;	// z2
			
			vertices[9]=1.0f;	// x3
			vertices[10]=1.0f;	// y3
			vertices[11]=0.0f;	// z3
			

			indices = new ushort[indexSize];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;
			indices[3] = 3;
			
			
			//@e                                                Vertex coordinate,               Texture coordinate,     Vertex color
			vertexBuffer = new VertexBuffer(4, indexSize, VertexFormat.Float3, VertexFormat.Float2, VertexFormat.Float4);
			
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
			
			for (int i = 0; i < colors.Length; i+=4)
			{
				colors[i] = r;
				colors[i+1] = g;
				colors[i+2] = b;
				colors[i+3] = a;
			}
		}
		
		
		public void SetTextureCoord(float x0, float y0, float x1, float y1)
		{
			texcoords[0] = x0 / texture.Width;	// left top u
			texcoords[1] = y0 / texture.Height; // left top v
			
			texcoords[2] = x0 / texture.Width;	// left bottom u
			texcoords[3] = y1 / texture.Height;	// left bottom v
			
			texcoords[4] = x1 / texture.Width;	// right top u
			texcoords[5] = y0 / texture.Height;	// right top v
			
			texcoords[6] = x1 / texture.Width;	// right bottom u
			texcoords[7] = y1 / texture.Height;	// right bottom v
		}
		
		public void SetTextureCoord(Vector2 topLeft, Vector2 bottomRight)
		{
			SetTextureCoord(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}
		
		
		public void Render()
		{
			graphics.SetShaderProgram(shaderProgram);
			
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);
			
			vertexBuffer.SetIndices(indices);
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0, texture);
			
			float screenWidth=graphics.Screen.Rectangle.Width;
			float screenHeight=graphics.Screen.Rectangle.Height;
			
			Matrix4 unitScreenMatrix = new Matrix4(
				 Width*2.0f/screenWidth,	0.0f,		0.0f, 0.0f,
				 0.0f,	 Height*(-2.0f)/screenHeight,	0.0f, 0.0f,
				 0.0f,	 0.0f, 1.0f, 0.0f,
				 -1.0f+(Position.X-Width * Center.X)*2.0f/screenWidth,  1.0f+(Position.Y-Height*Center.Y)*(-2.0f)/screenHeight, 0.0f, 1.0f
			);
			
			shaderProgram.SetUniformValue(0, ref unitScreenMatrix);

			graphics.DrawArrays(DrawMode.TriangleStrip, 0, indexSize);
		}
				
	}
}
