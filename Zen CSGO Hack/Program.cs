using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Zen_CSGO_Hack
{
    static class Program
    {
        private static Utils.PlayerBones _aimbotBone = Utils.PlayerBones.Head;
        private static ESP_Overlay _espForm;

        public static void Main()
        {
            //We get all processes with the name csgo
            var csgoProcesses = Process.GetProcessesByName("csgo");

            //If there is none
            if (!csgoProcesses.Any())
            {
                //we display that the csgo game is not found
                Console.WriteLine("CS:GO Not Found!");
                //We wait for user's approval
                Console.ReadKey();
                //we terminate the program
                return;
            }

            //We get our csgo handle
            Utils.CsgoHandle = csgoProcesses[0].Handle;

            //We loop through all csgo modules
            foreach (ProcessModule module in csgoProcesses[0].Modules)
            {
                //If we find the client_panorama.dll module
                if (Path.GetFileName(module.FileName) == "client_panorama.dll")
                {
                    //We save client_panorama.dll base address
                    Utils.ClientBaseAddress = module.BaseAddress;
                }
            }

            //If the client base address is zero, that means that we didn't find it
            if (Utils.ClientBaseAddress == IntPtr.Zero)
            {
                //We display that to the user
                Console.WriteLine("client_panorama.dll Not Found!");
                //We wait for user's approval
                Console.ReadKey();
                //we terminate the program
                return;
            }

            //Get the screen height
            Utils.ScreenHeight = Convert.ToInt32(SystemParameters.PrimaryScreenHeight);
            //Get the screen width
            Utils.ScreenWidth = Convert.ToInt32(SystemParameters.PrimaryScreenWidth);

            while (true)
            {
                if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F1) != 0)
                {
                    if (!Options.EspIsEnabled)
                        Options.EspIsEnabled = true;
                    else
                        Options.EspIsEnabled = false;

                    Esp();
                }
                else if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F2) != 0)
                {
                    if (!Options.GlowHackIsEnabled)
                    {
                        Options.GlowHackIsEnabled = true;

                        Thread wallHack = new Thread(WallHack);
                        wallHack.Start();
                    }
                    else
                    {
                        Options.GlowHackIsEnabled = false;
                    }
                }
                else if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F3) != 0)
                {
                    if (!Options.AimbotIsEnabled)
                    {
                        Options.AimbotIsEnabled = true;

                        Thread aimbot = new Thread(Aimbot);
                        aimbot.Start();
                    }
                    else
                    {
                        Options.AimbotIsEnabled = false;
                    }
                }
                else if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F4) != 0)
                {
                    if (!Options.TriggerbotIsEnabled)
                    {
                        Options.TriggerbotIsEnabled = true;

                        Thread triggerbot = new Thread(Triggerbot);
                        triggerbot.Start();
                    }
                    else
                    {
                        Options.TriggerbotIsEnabled = false;
                    }
                }
                else if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F6) != 0)
                {
                    if (!Options.NoFlashIsEnabled)
                    {
                        Options.NoFlashIsEnabled = true;

                        Thread noFlash = new Thread(NoFlash);
                        noFlash.Start();
                    }
                    else
                    {
                        Options.NoFlashIsEnabled = false;
                    }
                }
                else if (PinvokeWrapper.GetTheKeyState(KeyCodes.VK_F7) != 0)
                {
                    switch (_aimbotBone)
                    {
                        case Utils.PlayerBones.Head:
                            _aimbotBone = Utils.PlayerBones.Body;
                            break;
                        case Utils.PlayerBones.Body:
                            _aimbotBone = Utils.PlayerBones.Pelvis;
                            break;
                        case Utils.PlayerBones.Pelvis:
                            _aimbotBone = Utils.PlayerBones.RightFoot;
                            break;
                        case Utils.PlayerBones.RightFoot:
                            _aimbotBone = Utils.PlayerBones.LeftFoot;
                            break;
                        case Utils.PlayerBones.LeftFoot:
                            _aimbotBone = Utils.PlayerBones.Head;
                            break;
                    }
                }

                //Clear console
                Console.Clear();
                //Print the selected options
                Console.WriteLine(String.Concat($"F1 - ESP: \t\t{Options.EspIsEnabled}"));
                Console.WriteLine(String.Concat($"F2 - Glow Hack: \t{Options.GlowHackIsEnabled}"));
                Console.WriteLine(String.Concat($"F3 - Aimbot: \t\t{Options.AimbotIsEnabled}"));
                Console.WriteLine(String.Concat($"F7 - Aimbot Bone: \t{_aimbotBone.ToString()}"));
                Console.WriteLine(String.Concat($"F4 - Triggerbot: \t{Options.TriggerbotIsEnabled}"));
                Console.WriteLine(String.Concat($"F6 - No Flash: \t\t{Options.NoFlashIsEnabled}"));

                //We sleep for 200 milliseconds
                Thread.Sleep(200);
            }
        }

        private static void WallHack()
        {
            //We create a struct with the desired color for the enemy players
            var enemyStruct = new PlayerGlowStruct(255f, 0f, 0f, 1f, true, false); //Red color
            //We create a struct with the desired color for the allies
            var friendStruct = new PlayerGlowStruct(0f, 255f, 0f, 0.8f, true, false); //Green color

            while (Options.GlowHackIsEnabled)
            {
                var players = Utils.GetPlayers();

                //If there are no players, we continue looping until there is one
                if (players.Count == 0)
                    continue;

                //We get our local player
                var localPlayer = Utils.GetLocalPlayer();

                foreach (var currentPlayer in players)
                {
                    //We exclude players that are dead or they are dormant
                    if (currentPlayer.GetHealth() == 0 || currentPlayer.IsDormant())
                        continue;

                    if (currentPlayer.GetTeam() != localPlayer.GetTeam())
                    {
                        //Enable player on radar
                        PinvokeWrapper.WriteBool((IntPtr)currentPlayer.BaseAddress + Offsets.m_bSpotted, Utils.CsgoHandle, true);
                        //Draw enemy glow
                        DrawPlayerGlow(enemyStruct, currentPlayer.GetGlowIndex());
                    }
                    else
                    {
                        //Draw friend
                        DrawPlayerGlow(friendStruct, currentPlayer.GetGlowIndex());
                    }
                }

                Thread.Sleep(2);
            }
        }

        private static void Esp()
        {
            if(Options.EspIsEnabled)
            {
                //We create a new overlay
                _espForm = new ESP_Overlay();
                //We show our overlay
                _espForm.Show();
            }
            else
            {
                //We close our overlay
                _espForm.Close();
            }
        }

        private static void DrawPlayerGlow(PlayerGlowStruct playerStruct, int glowIndex)
        {
            //We ge the glow object manager
            var glowObjectManager = PinvokeWrapper.ReadAddInt(Utils.ClientBaseAddress + Offsets.dwGlowObjectManager, Utils.CsgoHandle);

            //We write the struct to memory
            PinvokeWrapper.WriteFloat((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x4, Utils.CsgoHandle, playerStruct.Red);
            PinvokeWrapper.WriteFloat((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x8, Utils.CsgoHandle, playerStruct.Green);
            PinvokeWrapper.WriteFloat((IntPtr)glowObjectManager + glowIndex * 0x38 + 0xC, Utils.CsgoHandle, playerStruct.Blue);
            PinvokeWrapper.WriteFloat((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x10, Utils.CsgoHandle, playerStruct.Alpha);
            PinvokeWrapper.WriteFloat((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x10, Utils.CsgoHandle, playerStruct.Alpha);
            PinvokeWrapper.WriteBool((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x24, Utils.CsgoHandle, playerStruct.RenderOccluded);
            PinvokeWrapper.WriteBool((IntPtr)glowObjectManager + glowIndex * 0x38 + 0x25, Utils.CsgoHandle, playerStruct.RenderUnoccluded);
        }

        private static void Aimbot()
        {
            while (Options.AimbotIsEnabled)
            {
                if (PinvokeWrapper.GetTheKeyState(KeyCodes.ALT_KEY) != 0)
                {
                    //We get all players
                    var players = Utils.GetPlayers();

                    //We get the closest player to our crosshair
                    var closestPlayer = GetClosestEnemy(players);

                    //If a player has been found and is not dead
                    if (closestPlayer != null && closestPlayer.GetHealth() > 0)
                    {
                        //We aim at him
                        AimAt((int)closestPlayer.GetBonePos((int)_aimbotBone).X, (int)closestPlayer.GetBonePos((int)_aimbotBone).Y);
                    }
                }

                Thread.Sleep(7);
            }
        }

        private static void Triggerbot()
        {
            while (Options.TriggerbotIsEnabled)
            {
                if (PinvokeWrapper.GetTheKeyState(KeyCodes.ALT_KEY) == 0) continue;

                //We get the local player
                var localPlayer = Utils.GetLocalPlayer();

                //We get the crosshair ID
                var crosshairId = PinvokeWrapper.ReadAddInt((IntPtr)localPlayer.BaseAddress + Offsets.m_iCrosshairId, Utils.CsgoHandle);

                if (crosshairId > 0 && crosshairId <= 64)
                {
                    //We get the enemy at our crosshair
                    var enemy = new Player(PinvokeWrapper.ReadAddInt(Utils.ClientBaseAddress + Offsets.dwEntityList + ((crosshairId - 1) * 0x10), Utils.CsgoHandle));

                    //If the enemy is alive and is not one of our teammates
                    if (enemy.GetHealth() > 0 && localPlayer.GetTeam() != enemy.GetTeam())
                    {
                        //We shoot
                        PinvokeWrapper.DoubleClick();
                    }
                }

                //Sleep for 3 milliseconds
                Thread.Sleep(3);
            }
        }

        private static void NoFlash()
        {
            while (Options.NoFlashIsEnabled)
            {
                //Get local player
                var localPlayer = Utils.GetLocalPlayer();
                //Set flash interval to zero
                PinvokeWrapper.WriteFloat((IntPtr)localPlayer.BaseAddress + Offsets.m_flFlashDuration, Utils.CsgoHandle, 0);

                //Sleep for 2 milliseconds
                Thread.Sleep(2);
            }
        }

        private static Player GetClosestEnemy(List<Player> playersToFind)
        {
            //Set the lowest distance to the maximum double value
            var lowestDistance = Double.MaxValue;
            //We declare a variable to save the closest player
            Player closestPlayer = null;

            //We get the local player
            var localPlayer = Utils.GetLocalPlayer();

            foreach (var currentPlayer in playersToFind)
            {
                //We get a vector with the player's bone screen coordinates
                var currentPlayerPos = currentPlayer.GetBonePos((int)_aimbotBone);

                //If our player is not consuidered a valid matchm we continue looping
                if ((currentPlayerPos.X <= 0 || currentPlayerPos.Y <= 0)
                    || (currentPlayerPos.X > Utils.ScreenWidth || currentPlayerPos.Y > Utils.ScreenHeight)
                    || (currentPlayer.GetTeam() == localPlayer.GetTeam())
                    || (currentPlayer.GetHealth() <= 0) 
                    || (currentPlayer.IsDormant()))
                    continue;

                //We get the distance between the player and our crosshair (middle of the screen)
                var currentPlayerDistance = Utils.GetVectorsDistance(
                    new Vector2(currentPlayerPos.X,
                        currentPlayerPos.Y), new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2));

                //If the player is closer to the crosshair
                if (currentPlayerDistance < lowestDistance)
                {
                    //Then we have anew champion, and we save the new lowest distance
                    lowestDistance = currentPlayerDistance;
                    //and we save the closest player
                    closestPlayer = currentPlayer;
                }
            }

            //We return the closest player
            return closestPlayer;
        }

        //Credits: fredaikis
        private static void AimAt(int x, int y)
        {
            float aimSpeed = 5;
            float targetX = 0;
            float targetY = 0;

            //X Axis
            if (x != 0)
            {
                if (x > Utils.ScreenWidth / 2)
                {
                    targetX = -(Utils.ScreenWidth / 2 - x);
                    targetX /= aimSpeed;
                    if (targetX + Utils.ScreenWidth / 2 > Utils.ScreenWidth / 2 * 2) targetX = 0;
                }

                if (x < Utils.ScreenWidth / 2)
                {
                    targetX = x - Utils.ScreenWidth / 2;
                    targetX /= aimSpeed;
                    if (targetX + Utils.ScreenWidth / 2 < 0) targetX = 0;
                }
            }

            //Y Axis

            if (y != 0)
            {
                if (y > Utils.ScreenHeight / 2)
                {
                    targetY = -(Utils.ScreenHeight / 2 - y);
                    targetY /= aimSpeed;
                    if (targetY + Utils.ScreenHeight / 2 > Utils.ScreenHeight / 2 * 2) targetY = 0;
                }

                if (y < Utils.ScreenHeight / 2)
                {
                    targetY = y - Utils.ScreenHeight / 2;
                    targetY /= aimSpeed;
                    if (targetY + Utils.ScreenHeight / 2 < 0) targetY = 0;
                }
            }

            PinvokeWrapper.MoveMouseTo((int)targetX, (int)targetY);
        }

        //This struct contains the values for the RGBA color that we want to use for our glow,
        //and also it contains the two options that define how the player is going to be "drawn"
        public struct PlayerGlowStruct
        {
            public float Red, Green, Blue, Alpha;
            public bool RenderOccluded, RenderUnoccluded;

            public PlayerGlowStruct(float red, float green, float blue, float alpha, bool renderOccluded, bool renderUnoccluded)
            {
                Red = red;
                Green = green;
                Blue = blue;
                Alpha = alpha;
                RenderOccluded = renderOccluded;
                RenderUnoccluded = renderUnoccluded;
            }
        }
    }
}
