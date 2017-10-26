using System;
namespace X.Engine
{

	public class cpGearJoint : cpConstraint
	{

		internal float phase, ratio;
		internal float ratio_inv;

		internal float iSum;

		internal float bias;
		internal float jAcc;


		public override void PreStep(float dt)
		{
			cpBody a = this.a;
			cpBody b = this.b;

			// calculate moment of inertia coefficient.
			this.iSum = 1.0f / (a.i_inv * this.ratio_inv + this.ratio * b.i_inv);

			// calculate bias velocity
			float maxBias = this.maxBias;
			this.bias = cp.cpfclamp(-cp.bias_coef(this.errorBias, dt) * (b.a * this.ratio - a.a - this.phase) / dt, -maxBias, maxBias);
		}

		public override void ApplyCachedImpulse(float dt_coef)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			float j = this.jAcc * dt_coef;
			a.w -= j * a.i_inv * this.ratio_inv;
			b.w += j * b.i_inv;
		}

		public override void ApplyImpulse(float dt)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			// compute relative rotational velocity
			float wr = b.w * this.ratio - a.w;

			float jMax = this.maxForce * dt;

			// compute normal impulse	
			float j = (this.bias - wr) * this.iSum;
			float jOld = this.jAcc;
			this.jAcc = cp.cpfclamp(jOld + j, -jMax, jMax);
			j = this.jAcc - jOld;

			// apply impulse
			a.w -= j * a.i_inv * this.ratio_inv;
			b.w += j * b.i_inv;
		}

		public override float GetImpulse()
		{
			return cp.cpfabs(this.jAcc);
		}


		public cpGearJoint(cpBody a, cpBody b, float phase, float ratio)
			: base(a, b)
		{

			this.phase = phase;
			this.ratio = ratio;
			this.ratio_inv = 1 / ratio;

			this.jAcc = 0.0f;


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

		public override float GetRatio()
		{
			return this.ratio;
		}

		public override void SetRatio(float value)
		{
			this.ActivateBodies();
			this.ratio = value;
			this.ratio_inv = 1 / value;

		}

	}

}

