using System;

namespace X.Engine
{
	public class cpRatchetJoint : cpConstraint
	{

		internal float angle, phase, ratchet;

		internal float iSum;

		internal float bias;
		internal float jAcc;

		public override void PreStep(float dt)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			float angle = this.angle;
			float phase = this.phase;
			float ratchet = this.ratchet;

			float delta = b.a - a.a;
			float diff = angle - delta;
			float pdist = 0.0f;

			if (diff * ratchet > 0.0f)
			{
				pdist = diff;
			}
			else
			{
				this.angle = cp.cpffloor((delta - phase) / ratchet) * ratchet + phase;
			}

			// calculate moment of inertia coefficient.
			this.iSum = 1.0f / (a.i_inv + b.i_inv);

			// calculate bias velocity
			float maxBias = this.maxBias;
			this.bias = cp.cpfclamp(-cp.bias_coef(this.errorBias, dt) * pdist / dt, -maxBias, maxBias);

			// If the bias is 0, the joint is not at a limit. Reset the impulse.
			if (this.bias == 0) this.jAcc = 0.0f;
		}

		public override void ApplyCachedImpulse(float dt_coef)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			float j = this.jAcc * dt_coef;
			a.w -= j * a.i_inv;
			b.w += j * b.i_inv;
		}

		public override void ApplyImpulse(float dt)
		{
			if (this.bias != 0) return; // early exit

			cpBody a = this.a;
			cpBody b = this.b;

			// compute relative rotational velocity
			float wr = b.w - a.w;
			float ratchet = this.ratchet;

			float jMax = this.maxForce * dt;

			// compute normal impulse	
			float j = -(this.bias + wr) * this.iSum;
			float jOld = this.jAcc;
			this.jAcc = cp.cpfclamp((jOld + j) * ratchet, 0.0f, jMax * cp.cpfabs(ratchet)) / ratchet;
			j = this.jAcc - jOld;

			// apply impulse
			a.w -= j * a.i_inv;
			b.w += j * b.i_inv;
		}

		public override float GetImpulse()
		{
			return cp.cpfabs(jAcc);
		}

		public cpRatchetJoint(cpBody a, cpBody b, float phase, float ratchet)
			: base(a, b)
		{



			this.angle = 0.0f;
			this.phase = phase;
			this.ratchet = ratchet;

			// STATIC_BODY_CHECK
			this.angle = (b != null ? b.a : 0.0f) - (a != null ? a.a : 0.0f);

		}


		public override float GetAngle()
		{
			return this.angle;
		}

		public override void SetAngle(float angle)
		{
			ActivateBodies();
			this.angle = angle;
		}

		public override float GetPhase()
		{
			return this.phase;
		}


		public override void SetPhase(float phase)
		{
			ActivateBodies();
			this.phase = phase;
		}


		public override float GetRatchet()
		{
			return this.ratchet;
		}


		public override void SetRatchet(float ratchet)
		{
			ActivateBodies();
			this.ratchet = ratchet;
		}

	}


}