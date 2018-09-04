using System;

namespace Zen_CSGO_Hack
{
    internal class Offsets
    {
        //Shared offsets
        public const Int32 dwLocalPlayer = 0xC6086C;
        public const Int32 m_iTeamNum = 0xF0;
        public const Int32 m_iHealth = 0xFC;
        public const Int32 dwEntityList = 0x4C3D184;

        //WallHack Offsets
        public const Int32 m_iGlowIndex = 0xA320;
        public const Int32 dwGlowObjectManager = 0x517C4A8;
        public const Int32 m_bSpotted = 0x939;
        public const Int32 m_bDormant = 0xE9;
        public const Int32 m_ArmorValue = 0xB24C;
        public const Int32 m_vecOrigin = 0x134;

        //Aimbot Offsets
        public const Int32 dwViewMatrix = 0x4C2EBB4;
        public const Int32 m_dwBoneMatrix = 0x2698;

        //Triggerbot offsets
        public const Int32 m_iCrosshairId = 0xB2B8;

        //No Flash Offsets
        public const Int32 m_flFlashDuration = 0xA308;
    }
}
