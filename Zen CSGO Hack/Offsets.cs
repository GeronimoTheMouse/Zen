using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zen_CSGO_Hack
{
    internal class Offsets
    {
        //Shared offsets
        public const Int32 dwLocalPlayer = 0xC5C85C;
        public const Int32 m_iTeamNum = 0xF0;
        public const Int32 m_iHealth = 0xFC;
        public const Int32 dwEntityList = 0x4C3915C;

        //WallHack Offsets
        public const Int32 m_iGlowIndex = 0xA320;
        public const Int32 dwGlowObjectManager = 0x5178E58;
        public const Int32 m_bSpotted = 0x939;
        public const Int32 m_bDormant = 0xE9;
        public const Int32 m_ArmorValue = 0xB24C;

        //Aimbot Offsets
        public const Int32 dwViewMatrix = 0x4C2AB74;
        public const Int32 m_dwBoneMatrix = 0x2698;

        //Triggerbot offsets
        public const Int32 m_iCrosshairId = 0xB2B8;

        //No Flash Offsets
        public const Int32 m_flFlashDuration = 0xA308;
    }
}
