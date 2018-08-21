using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zen_CSGO_Hack
{
    internal class Player
    {
        public Player(int playerBase)
        {
            BaseAddress = playerBase;
        }

        public Player() { }

        public int BaseAddress { get; }

        public int GetTeam()
        {
            return PinvokeWrapper.ReadAddInt((IntPtr)BaseAddress + Offsets.m_iTeamNum, Utils.CsgoHandle);
        }

        public int GetHealth()
        {
            return PinvokeWrapper.ReadAddInt((IntPtr)BaseAddress + Offsets.m_iHealth, Utils.CsgoHandle);
        }

        public int GetArmor()
        {
            return PinvokeWrapper.ReadAddInt((IntPtr) BaseAddress + Offsets.m_ArmorValue, Utils.CsgoHandle);
        }

        public int GetGlowIndex()
        {
            return PinvokeWrapper.ReadAddInt((IntPtr)BaseAddress + Offsets.m_iGlowIndex, Utils.CsgoHandle);
        }

        public bool IsDormant()
        {
            //Check if player is dormant
            var isDormant = PinvokeWrapper.ReadAddInt((IntPtr) BaseAddress + Offsets.m_bDormant, Utils.CsgoHandle);

            //Return the result
            return isDormant != 0;
        }

        public Vector2 GetBonePos(int boneId)
        {
            //Create a new vector to save the 2d bone coordinates later
            var player2DPosition = new Vector2();

            //Get the bone matrix
            var boneMatrix = PinvokeWrapper.ReadAddInt((IntPtr)BaseAddress + Offsets.m_dwBoneMatrix, Utils.CsgoHandle);

            //Get the world coordinates of the player's bone
            var playerBoneX = PinvokeWrapper.ReadAddFloat((IntPtr)boneMatrix + 0x30 * boneId + 0x0C, Utils.CsgoHandle);
            var playerBoneY = PinvokeWrapper.ReadAddFloat((IntPtr)boneMatrix + 0x30 * boneId + 0x1C, Utils.CsgoHandle);
            var playerBoneZ = PinvokeWrapper.ReadAddFloat((IntPtr)boneMatrix + 0x30 * boneId + 0x2C, Utils.CsgoHandle);

            //Convert the world coordinates to screen coordinates
            Utils.WorldToScreen(new Vector3(playerBoneX, playerBoneY, playerBoneZ), player2DPosition);

            //Return the result
            return player2DPosition;
        }
    }
}
