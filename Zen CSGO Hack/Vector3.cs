namespace Zen_CSGO_Hack
{
    internal class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
    }
}
