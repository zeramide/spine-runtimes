using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spine {
	public class SkeletonRenderer {
		GraphicsDevice device;
		SpriteBatcher batcher;
		BasicEffect effect;
		RasterizerState rasterizerState;

		public SkeletonRenderer (GraphicsDevice device) {
			this.device = device;

			batcher = new SpriteBatcher();

			effect = new BasicEffect(device);
			effect.World = Matrix.Identity;
			effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
			effect.Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, 1, 0);
			effect.TextureEnabled = true;
			effect.VertexColorEnabled = true;

			rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;

			Bone.yDown = true;
		}

		public void Draw (Skeleton skeleton) {
			List<Slot> drawOrder = skeleton.DrawOrder;
			for (int i = 0, n = drawOrder.Count; i < n; i++) {
				Slot slot = drawOrder[i];
				Attachment attachment = slot.Attachment;
				if (attachment == null) continue;
				if (attachment is RegionAttachment) {
					RegionAttachment regionAttachment = (RegionAttachment)attachment;

					SpriteBatchItem item = batcher.CreateBatchItem();
					item.Texture = ((XnaAtlasPage)regionAttachment.Region.Page).Texture;

					byte r = (byte)(slot.R * 255);
					byte g = (byte)(slot.G * 255);
					byte b = (byte)(slot.B * 255);
					byte a = (byte)(slot.A * 255);
					item.vertexTL.Color.R = r;
					item.vertexTL.Color.G = g;
					item.vertexTL.Color.B = b;
					item.vertexTL.Color.A = a;
					item.vertexBL.Color.R = r;
					item.vertexBL.Color.G = g;
					item.vertexBL.Color.B = b;
					item.vertexBL.Color.A = a;
					item.vertexBR.Color.R = r;
					item.vertexBR.Color.G = g;
					item.vertexBR.Color.B = b;
					item.vertexBR.Color.A = a;
					item.vertexTR.Color.R = r;
					item.vertexTR.Color.G = g;
					item.vertexTR.Color.B = b;
					item.vertexTR.Color.A = a;

					regionAttachment.UpdateVertices(slot.Bone);
					float[] vertices = regionAttachment.Vertices;
					item.vertexTL.Position.X = vertices[RegionAttachment.X1];
					item.vertexTL.Position.Y = vertices[RegionAttachment.Y1];
					item.vertexTL.Position.Z = 0;
					item.vertexBL.Position.X = vertices[RegionAttachment.X2];
					item.vertexBL.Position.Y = vertices[RegionAttachment.Y2];
					item.vertexBL.Position.Z = 0;
					item.vertexBR.Position.X = vertices[RegionAttachment.X3];
					item.vertexBR.Position.Y = vertices[RegionAttachment.Y3];
					item.vertexBR.Position.Z = 0;
					item.vertexTR.Position.X = vertices[RegionAttachment.X4];
					item.vertexTR.Position.Y = vertices[RegionAttachment.Y4];
					item.vertexTR.Position.Z = 0;

					float[] uvs = regionAttachment.UVs;
					item.vertexTL.TextureCoordinate.X = uvs[RegionAttachment.X1];
					item.vertexTL.TextureCoordinate.Y = uvs[RegionAttachment.Y1];
					item.vertexBL.TextureCoordinate.X = uvs[RegionAttachment.X2];
					item.vertexBL.TextureCoordinate.Y = uvs[RegionAttachment.Y2];
					item.vertexBR.TextureCoordinate.X = uvs[RegionAttachment.X3];
					item.vertexBR.TextureCoordinate.Y = uvs[RegionAttachment.Y3];
					item.vertexTR.TextureCoordinate.X = uvs[RegionAttachment.X4];
					item.vertexTR.TextureCoordinate.Y = uvs[RegionAttachment.Y4];
				}
			}

			device.RasterizerState = rasterizerState;
			device.BlendState = BlendState.AlphaBlend;

			foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
				pass.Apply();
				batcher.Draw(device);
			}
		}
	}
}