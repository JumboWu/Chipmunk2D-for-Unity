using System;
namespace X.Engine
{
	public class cpDampedSpring : cpConstraint
	{
        /// Function type used for damped spring force callbacks.
        public delegate float cpDampedSpringForceFunc(cpDampedSpring spring, float dist);

		internal cpVect anchorA, anchorB;
		internal float restLength;
		internal float stiffness;
		internal float damping;
        internal cpDampedSpringForceFunc springForceFunc;

		internal float v_coef;
		internal float target_vrn;

		internal cpVect n;
		internal cpVect r1, r2;
		internal float nMass;

		internal float jAcc;


		public float defaultSpringForce(cpDampedSpring spring, float dist)
		{
			return (spring.restLength - dist) * spring.stiffness;
		}

		public override void PreStep(float dt)
		{

			cpBody a = this.a;
			cpBody b = this.b;

			this.r1 = cpTransform.Vect(a.transform, cpVect.cpvsub(this.anchorA, a.cog));
			this.r2 = cpTransform.Vect(b.transform, cpVect.cpvsub(this.anchorB, b.cog));

			cpVect delta = cpVect.cpvsub(cpVect.cpvadd(b.p, this.r2), cpVect.cpvadd(a.p, this.r1));
			float dist = cpVect.cpvlength(delta);
			this.n = cpVect.cpvmult(delta, 1.0f / (dist > 0 ? dist : cp.Infinity));

			float k = cp.k_scalar(a, b, this.r1, this.r2, this.n);
			cp.AssertSoft(k != 0.0, "Unsolvable this.");
			this.nMass = 1.0f / k;

			this.target_vrn = 0.0f;
			this.v_coef = 1.0f - cp.cpfexp(-this.damping * dt * k);

			// apply spring force
			float f_spring = this.springForceFunc(this, dist);
			float j_spring = this.jAcc = f_spring * dt;
			cp.apply_impulses(a, b, this.r1, this.r2, cpVect.cpvmult(this.n, j_spring));
		}

		public override void ApplyCachedImpulse(float coef)
		{
		}

		public override void ApplyImpulse(float dt)
		{
			cpBody a = this.a;
			cpBody b = this.b;

			cpVect n = this.n;
			cpVect r1 = this.r1;
			cpVect r2 = this.r2;

			// compute relative velocity
			float vrn = cp.normal_relative_velocity(a, b, r1, r2, n);

			// compute velocity loss from drag
			float v_damp = (this.target_vrn - vrn) * this.v_coef;
			this.target_vrn = vrn + v_damp;

			float j_damp = v_damp * this.nMass;
			this.jAcc += j_damp;
			cp.apply_impulses(a, b, this.r1, this.r2, cpVect.cpvmult(this.n, j_damp));
		}

		public cpDampedSpring(cpBody a, cpBody b, cpVect anchr1, cpVect anchr2, float restLength, float stiffness, float damping)
			: base(a, b)
		{

			this.anchorA = anchr1;
			this.anchorB = anchr2;

			this.restLength = restLength;

			this.stiffness = stiffness;
			this.damping = damping;

			this.springForceFunc = defaultSpringForce;

			this.target_vrn = this.v_coef = 0;

			this.r1 = this.r2 = null;
			this.nMass = 0;
			this.n = null;

			this.jAcc = 0f;


		}


		#region PROPS OVERIDE


		public override void SetStiffness(float stiffness)
		{
			this.stiffness = stiffness;
		}

		public override float GetStiffness()
		{
			return base.GetStiffness();
		}

		public override void SetAnchorA(cpVect anchr1)
		{
			this.anchorA = anchr1;
		}

		public override cpVect GetAnchorA()
		{
			return this.anchorA;
		}

		public override void SetAnchorB(cpVect anchr2)
		{
			this.anchorB = anchr2;
		}

		public override cpVect GetAnchorB()
		{
			return anchorB;
		}

		public override void SetRestLength(float restLength)
		{
			this.restLength = restLength;
		}

		public override float GetRestLength()
		{
			return restLength;
		}

		public override float GetDamping()
		{
			return damping;
		}

		public override void SetDamping(float damping)
		{
			this.damping = damping;
		}

		#endregion


		public override float GetImpulse()
		{
			return this.jAcc;
		}

		//public override void Draw(cpDebugDraw m_debugDraw)
		//{
		//	var a = this.a.LocalToWorld(this.anchorA);
		//	var b = this.b.LocalToWorld(this.anchorB);

		//	m_debugDraw.DrawSpring(a, b, cpColor.Grey);
		//}




	}

}

