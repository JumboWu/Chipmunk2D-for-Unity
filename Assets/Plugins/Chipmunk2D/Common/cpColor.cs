using System;
using System.Collections.Generic;
using System.Text;

namespace X.Engine
{
	public struct cpColor
	{

		public cpColor(float xr, float xg, float xb) { r = xr; g = xg; b = xb; a = 255; }
		public cpColor(float xr, float xg, float xb, float ba) { r = xr; g = xg; b = xb; a = ba; }

		public cpColor(cpColor color)
		{
			r = color.r; g = color.g; b = color.b; a = color.a;
		}

		public void Set(cpColor color)
		{
			Set(color.r, color.g, color.b, color.a);
		}


		public void Set(float ri, float gi, float bi, float ba) { r = ri; g = gi; b = bi; a = ba; }

		public float r, g, b, a;

		public static cpColor Red { get { return new cpColor(255, 0, 0); } }
		public static cpColor Green { get { return new cpColor(0, 255, 0); } }
		public static cpColor Blue { get { return new cpColor(0, 0, 255); } }
		public static cpColor Black { get { return new cpColor(0, 0, 0); } }

		public static cpColor White { get { return new cpColor(255, 255, 255); } }

		public static cpColor Grey { get { return new cpColor(84, 84, 84); } }
		public static cpColor DarkGrey { get { return new cpColor(50, 50, 50); } }

		public static cpColor CyanBlue { get { return new cpColor(170, 212, 255); } }

		public static cpColor WhiteGreen { get { return new cpColor(212, 255, 212); } }

		public static cpColor WhiteRed { get { return new cpColor(255, 127, 127); } }

	}
}
