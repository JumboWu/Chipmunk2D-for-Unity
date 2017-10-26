using System;
using System.Collections.Generic;

namespace X.Engine
{

	public class cpContact
	{

		//public cpVect p, n;
		//public float dist;

		public cpVect r1, r2;
		public float nMass, tMass, bounce;

		public float jnAcc, jtAcc, jBias;
		public float bias;

		public ulong hash;

		public override string ToString()
		{
			return string.Format("{0}: p({1}),n({2})", hash, r1, r2);
		}

		public cpContact(cpVect r1, cpVect r2, ulong hash)
		{
			Init(r1, r2, hash);
		}

		//public cpContact Clone()
		//{
		//	cpContact tmp = new cpContact(p, n, dist, hash);
		//	return tmp;
		//}

		public void Init(cpVect r1, cpVect r2, ulong hash)
		{
			this.r1 = r1;
			this.r2 = r2;
			//this.dist = dist;

			//	this.r1 = this.r2 = cpVect.Zero;
			this.nMass = this.tMass = this.bounce = this.bias = 0;

			this.jnAcc = this.jtAcc = this.jBias = 0;
			this.hash = hash;
			cp.numContacts++;
		}

		//public void Draw(cpDebugDraw m_debugDraw)
		//{
		//	m_debugDraw.DrawPoint(r1, 1, cpColor.Red);
		//	m_debugDraw.DrawPoint(r2, 1, cpColor.Red);
		//}

	};

}
