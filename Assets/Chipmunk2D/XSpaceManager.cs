using X.Tools;

namespace X.Engine
{
    public class XSpaceManager
    {
        public static int PixelsPerUnit = 100; //Default:100
        public static float UnitsPerPixel = 1.0f / PixelsPerUnit;
        private cpSpace mSpace;

        private static XSpaceManager mInstance;

        public static XSpaceManager Instance{
            get
            {
                if(mInstance == null)
                {
                    mInstance = new XSpaceManager();
                }

                return mInstance;
            }
        }

        protected XSpaceManager()
        {
            mSpace = new cpSpace();
            mSpace.SetIterations(10);
        }

        public cpSpace Space { get { return mSpace; } }

    }

}
