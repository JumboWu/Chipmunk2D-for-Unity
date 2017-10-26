using System;
namespace X.Engine
{

	public class cpPinJoint : cpConstraint
	{


		internal cpVect anchorA, anchorB;
		internal float dist;

		internal cpVect r1, r2;
		internal cpVect n;
		internal float nMass;

		internal float jnAcc;
		internal float bias;



		public override void PreStep(float dt)
		{
			cpBody a = this.a;
			cpBody b = this.b;

			this.r1 = cpTransform.Vect(a.transform, cpVect.cpvsub(this.anchorA, a.cog));
			this.r2 = cpTransform.Vect(b.transform, cpVect.cpvsub(this.anchorB, b.cog));

			cpVect delta = cpVect.cpvsub(cpVect.cpvadd(b.p, this.r2), cpVect.cpvadd(a.p, this.r1));
			float dist = cpVect.cpvlength(delta);
			this.n = cpVect.cpvmult(delta, 1.0f / (dist > 0 ? dist : cp.Infinity));

			// calculate mass normal
			this.nMass = 1.0f / cp.k_scalar(a, b, this.r1, this.r2, this.n);

			// calculate bias velocity
			float maxBias = this.maxBias;
			this.bias = cp.cpfclamp(-cp.bias_coef(this.errorBias, dt) * (dist - this.dist) / dt, -maxBias, maxBias);
		}

		public override void ApplyCachedImpulse(float dt_coef)
		{
			cpBody a = this.a;
			cpBody b = this.b;

			cpVect j = cpVect.cpvmult(this.n, this.jnAcc * dt_coef);
			cp.apply_impulses(a, b, this.r1, this.r2, j);
		}

		public override void ApplyImpulse(float dt)
		{

			cpBody a = this.a;
			cpBody b = this.b;
			cpVect n = this.n;

			// compute relative velocity
			float vrn = cp.normal_relative_velocity(a, b, this.r1, this.r2, n);

			float jnMax = this.maxForce * dt;

			// compute normal impulse
			float jn = (this.bias - vrn) * this.nMass;
			float jnOld = this.jnAcc;
			this.jnAcc = cp.cpfclamp(jnOld + jn, -jnMax, jnMax);
			jn = this.jnAcc - jnOld;

			// apply impulse
			cp.apply_impulses(a, b, this.r1, this.r2, cpVect.cpvmult(n, jn));
		}

		public override float GetImpulse()
		{
			return cp.cpfabs(this.jnAcc);
		}

		//public override void Draw(cpDebugDraw m_debugDraw)
		//{

		//	cpVect a = cpTransform.cpTransformPoint(this.a.transform, this.anchorA);
		//	cpVect b = cpTransform.cpTransformPoint(this.b.transform, this.anchorB);

		//	m_debugDraw.DrawSegment(a, b, cpColor.Grey);
		//	m_debugDraw.DrawPoint(a, 5, cpColor.DarkGrey);
		//	m_debugDraw.DrawPoint(b, 5, cpColor.DarkGrey);
		
		//}



		public override cpVect GetAnchorA()
		{
			return anchorA;
		}

		public override void SetAnchorA(cpVect anchr)
		{
			ActivateBodies();
			anchorA = anchr;
		}

		public override cpVect GetAnchorB()
		{
			return anchorB;
		}

		public override void SetAnchorB(cpVect anchr)
		{
			ActivateBodies();
			anchorB = anchr;
		}


		public override void SetDist(float distance)
		{
			ActivateBodies();
			dist = distance;
		}
		public override float GetDist()
		{

			return dist;
		}


		public cpPinJoint(cpBody a, cpBody b, cpVect anchorA, cpVect anchorB)
			: base(a, b)
		{

			this.anchorA = anchorA;
			this.anchorB = anchorB;

			// STATIC_BODY_CHECK
			cpVect p1 = (a != null ? cpTransform.Point(a.transform, anchorA) : anchorA);
			cpVect p2 = (b != null ? cpTransform.Point(b.transform, anchorB) : anchorB);
			this.dist = cpVect.cpvlength(cpVect.cpvsub(p2, p1));

			cp.AssertSoft(this.dist > 0, "You created a 0 length pin joint. A pivot joint will be much more stable.");
			//if (this.dist <= 0)
			//	throw new NotImplementedException("You created a 0 length pin joint. A pivot joint will be much more stable.");

			this.jnAcc = 0.0f;
		}





	}

}

