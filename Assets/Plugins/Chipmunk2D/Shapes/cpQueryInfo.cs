using System;
using System.Collections.Generic;

namespace X.Engine
{


	/// Extended point query info struct. Returned from calling pointQuery on a shape.
	public class cpSegmentQueryInfo
	{

		/// The shape that was hit, NULL if no collision occured.
		public cpShape shape;
		/// The normalized distance along the query segment in the range [0, 1].
		public float alpha;
		/// The normal of the surface hit.
		public cpVect normal;

		/// The point of impact.
		public cpVect point;

		public cpSegmentQueryInfo(cpShape shape, cpVect point, cpVect normal, float alpha)
		{
			/// The shape that was hit, NULL if no collision occured.
			this.shape = shape;
			/// The normalized distance along the query segment in the range [0, 1].
			this.alpha = alpha;
			/// The normal of the surface hit.
			this.normal = normal;

			this.point = point;

		}

		public void Set(cpSegmentQueryInfo info1)
		{
			/// The shape that was hit, NULL if no collision occured.
			this.shape = info1.shape;
			/// The normalized distance along the query segment in the range [0, 1].
			this.alpha = info1.alpha;
			/// The normal of the surface hit.
			this.normal = info1.normal;

			this.point = info1.point;

		}

		//public static cpSegmentQueryInfo CreateBlanck()
		//{
		//	return new cpSegmentQueryInfo(null, 1.0f, cpVect.Zero);
		//}

		//public cpVect HitPoint(cpVect start, cpVect end)
		//{
		//	return cpVect.Lerp(start, end, this.alpha);
		//}

		//public float HitDist(cpVect start, cpVect end)
		//{
		//	return cpVect.Distance(start, end) * this.alpha;
		//}

	}


	/// Extended point query info struct. Returned from calling pointQuery on a shape.
	public struct cpPointQueryExtendedInfo
	{
		public cpShape shape;
		public cpVect n;
		public float d;

		public cpPointQueryExtendedInfo(cpShape tShape)
		{
			/// The nearest shape, NULL if no shape was within range.
			this.shape = tShape;
			/// The closest point on the shape's surface. (in world space coordinates)
			this.d = cp.Infinity;
			/// The distance to the point. The distance is negative if the point is inside the shape.
			this.n = cpVect.Zero;
		}

	}

}
