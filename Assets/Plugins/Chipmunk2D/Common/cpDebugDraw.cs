using System;
using System.Collections.Generic;

namespace X.Engine
{
	[Flags]
	public enum cpDrawFlags
	{

		None = 1 << 0,


		/// <summary>
		/// Draw shapes.
		/// </summary>
		Shapes = 1 << 1,

		/// <summary>
		/// Draw joint connections.
		/// </summary>
		Joints = 1 << 2,

		/// <summary>
		/// Draw contact points.
		/// </summary>
		ContactPoints = 1 << 3,

		/// <summary>
		/// Draw polygon BB.
		/// </summary>
		BB = 1 << 4,

		/// <summary>
		/// Draw All connections.
		/// </summary>
		All = 1 << 10,

	}

	public abstract class cpDebugDraw
	{


		private cpDrawFlags m_drawFlags = 0x0;
		private int _PTMRatio = 1;

		//protected cpSpace space;

		public cpDebugDraw()
		{
			//	this.space = space;
		}

		public cpDebugDraw(int ptm)
		{
			_PTMRatio = ptm;
		}

		public int PTMRatio
		{
			get { return (_PTMRatio); }
		}
		/// Set the drawing flags.
		public void SetFlags(cpDrawFlags flags)
		{
			m_drawFlags = flags;
		}

		/// Get the drawing flags.
		public cpDrawFlags Flags
		{
			get { return (m_drawFlags); }
			set { m_drawFlags = value; }
		}

		/// Append flags to the current flags.
		public void AppendFlags(cpDrawFlags flags)
		{
			m_drawFlags |= flags;
		}

		/// Clear flags from the current flags.
		public void RemoveFlags(cpDrawFlags flags)
		{
			m_drawFlags &= ~flags;
		}



		/// Draw a closed polygon provided in CCW order.
		public abstract void DrawPolygon(cpVect[] vertices, int vertexCount, cpColor color);

		/// Draw a solid closed polygon provided in CCW order.
		public abstract void DrawSolidPolygon(cpVect[] vertices, int vertexCount, cpColor color);

		/// Draw a circle.
		public abstract void DrawCircle(cpVect center, float radius, cpColor color);

		/// Draw a solid circle.
		public abstract void DrawSolidCircle(cpVect center, float radius, cpVect axis, cpColor color);

		/// Draw a line segment.
		public abstract void DrawSegment(cpVect p1, cpVect p2, cpColor color);

		/// Draw a line segment with a strokeWidth.
		public abstract void DrawSegment(cpVect p1, cpVect p2, float lineWidth, cpColor color);

		public abstract void DrawString(int x, int y, string format, params object[] objects);

		public abstract void DrawSpring(cpVect a, cpVect b, cpColor cpColor);

		public abstract void DrawPoint(cpVect p, float size, cpColor color);

		public abstract void DrawBB(cpBB bb, cpColor color);

	}


}
