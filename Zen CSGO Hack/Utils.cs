using System;
using System.Collections.Generic;

namespace Zen_CSGO_Hack
{
    class Utils
    {
        //Global variables used to the whole program
        public static IntPtr CsgoHandle { get; set; }
        public static IntPtr ClientBaseAddress { get; set; }
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static List<Player> GetPlayers()
        {
            //Create new list of players
            var playerList = new List<Player>();

            //Get our local player
            var localPlayer = GetLocalPlayer();

            for (int i = 0; i < 64; i++)
            {
                //Get the next player
                var playerBase = PinvokeWrapper.ReadAddInt(ClientBaseAddress + Offsets.dwEntityList + (i * 0x10), CsgoHandle);

                //If the player doesn't exist, we continue looping
                if (playerBase == 0)
                    continue;

                //We make sure we do not include our self in the list
                if (playerBase != localPlayer.BaseAddress)
                    playerList.Add(new Player(playerBase));
            }

            //Return the player list
            return playerList;
        }

        public static Player GetLocalPlayer()
        {
            //Get the base address of
            var localPlayer = PinvokeWrapper.ReadAddInt(ClientBaseAddress + Offsets.dwLocalPlayer, CsgoHandle);

            //return our new class
            return new Player(localPlayer);
        }

        //Credits: 0x2aff 
        public static bool WorldToScreen(Vector3 from, Vector2 to)
        {
            var viewMatrix = new float[16];

            for (int i = 0; i < 16; i++)
                viewMatrix[i] = PinvokeWrapper.ReadAddFloat(ClientBaseAddress + Offsets.dwViewMatrix + (i * 0x4), CsgoHandle);

            float w = 0.0f;

            to.X = viewMatrix[0] * from.X + viewMatrix[1] * from.Y + viewMatrix[2] * from.Z + viewMatrix[3];
            to.Y = viewMatrix[4] * from.X + viewMatrix[5] * from.Y + viewMatrix[6] * from.Z + viewMatrix[7];
            w = viewMatrix[12] * from.X + viewMatrix[13] * from.Y + viewMatrix[14] * from.Z + viewMatrix[15];

            if (w < 0.01f)
                return false; //Not in FOV, we return false

            float inverse = 1.0f / w;
            to.X *= inverse;
            to.Y *= inverse;

            float x = ScreenWidth / 2;
            float y = ScreenHeight / 2;

            x += 0.5f * to.X * ScreenWidth + 0.5f;
            y -= 0.5f * to.Y * ScreenHeight + 0.5f;

            to.X = x;
            to.Y = y;

            return true; //Success
        }

        //Get the distance between two vectors using the Euclidean formula
        public static double GetVectorsDistance(Vector2 firstVector, Vector2 secondVector)
        {
            var x = firstVector.X - secondVector.X;
            var y = firstVector.Y - secondVector.Y;

            return Math.Sqrt(x * x + y * y);
        }

        //The player bones along with their current indexes.
        public enum PlayerBones
        {
            Head = 8,
            Body = 5,
            Pelvis = 0,
            RightFoot = 79,
            LeftFoot = 72
        };
    }
}
