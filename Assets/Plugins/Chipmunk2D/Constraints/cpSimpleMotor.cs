using System;
namespace X.Engine
{

	public class cpSimpleMotor : cpConstraint
	{

	internal	float rate;
	internal	float iSum;
	internal	float jAcc;

		public override void PreStep(float dt)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			// calculate moment of inertia coefficient.
			this.iSum = 1.0f / (a.i_inv + b.i_inv);
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

			cpBody a = this.a;
			cpBody b = this.b;

			// compute relative rotational velocity
			float wr = b.w - a.w + this.rate;

			float jMax = this.maxForce * dt;

			// compute normal impulse	
			float j = -wr * this.iSum;
			float jOld = this.jAcc;
			this.jAcc = cp.cpfclamp(jOld + j, -jMax, jMax);
			j = this.jAcc - jOld;

			// apply impulse
			a.w -= j * a.i_inv;
			b.w += j * b.i_inv;

		}

		public override float GetImpulse()
		{
			return cp.cpfabs(jAcc);
		}




		public cpSimpleMotor(cpBody a, cpBody b, float rate)
			: base(a, b)
		{

			this.rate = rate;

			this.jAcc = 0.0f;

		}



		public override void SetRate(float rate)
		{
			ActivateBodies();
			this.rate = rate;
		}


		public override float GetRate()
		{
			return rate;
		}



	}


}