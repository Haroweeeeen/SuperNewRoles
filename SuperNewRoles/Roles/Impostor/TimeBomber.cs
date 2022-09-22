using System;
using System.Collections.Generic;
using System.Text;
using SuperNewRoles.Buttons;
using SuperNewRoles.Patch;

using UnityEngine;




using COT = SuperNewRoles.Patch.CustomOptionType;
using CO = SuperNewRoles.Patch.CustomOption;

namespace SuperNewRoles.Roles.Impostor
{
    public class TimeBomber
    {
        public static CustomRoleOption Option;
        public static CO PlayerCount;
        public static CO KillCool;          // キルクール
        public static CO StartTime;         // 起動するまでの時間
        public static CO BombTime;          // 爆発するまでの時間
        public static CO BombScope;         // 爆発のキル半径
        public static CO CanNotMoveTime;    // 起爆時動けない時間
        public static CO ExtensionKillCool; // 起爆時に伸びるキルクールの量
        public static CO IsArrow;           // 起爆時に矢印を表示するか
        public static CO ArrowScope;        // 起爆時に矢印を表示する半径

        public static void SetupCustomOptions()
        {
            var id = 992;
            Option = new(id, false, COT.Impostor, "TimeBomberName", color, 1);
            PlayerCount = CO.Create(id + 1, false, COT.Impostor, "SettingPlayerCountName", 1f, 1f, 5f, 1f, Option);
            KillCool = CO.Create(id + 2, false, COT.Impostor, "KillCoolDown", 20, 0, 60, 2.5f, Option);
            StartTime = CO.Create(id + 3, false, COT.Impostor, "TimeToStart", 5, 0, 15, 0.5f, Option);
            BombTime = CO.Create(id + 4, false, COT.Impostor, "TimeToBomb", 15, 0, 45, 0.5f, Option);
            BombScope = CO.Create(id + 5, false, COT.Impostor, "BombScope", 1, 0.5f, 3, 0.5f, Option);
            CanNotMoveTime = CO.Create(id + 6, false, COT.Impostor, "CanNotMoveTime", 5, 0, 10, 0.5f, Option);
            ExtensionKillCool = CO.Create(id + 7, false, COT.Impostor, "ExtensionKillCool", 10, 0, 30, 2.5f, Option);
            IsArrow = CO.Create(id + 8, false, COT.Impostor, "BombArrow", true, Option);
            ArrowScope = CO.Create(id + 9, false, COT.Impostor, "ArrowScope", 4, 0.5f, 10, 0.5f, IsArrow);
        }

        public static List<PlayerControl> Player;
        public static Color32 color = RoleClass.ImpostorRed;
        public static List<PlayerControl> AllTarget; // 爆破した全てのターゲット
        public static List<PlayerControl> NowTarget; // 今のターゲット
        public static float NowBombTime;

        public static void ClearAndReload()
        {
            Player = new();
            AllTarget = new();
            NowTarget = new();
            NowBombTime = BombTime.GetFloat();
        }

        public static void AttachBomb(PlayerControl target)
        {
            if (AllTarget.Contains(target)) return; // targetがすでに爆破されているなら破棄

            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.IsAlive() && p.PlayerId != target.PlayerId)
                    if (SelfBomber.GetIsBomb(target, p, BombScope.GetFloat()))
                    {
                        target.RpcMurderPlayer(p);
                    }
            }
            target.RpcMurderPlayer(target);
            AllTarget.Add(target);
        }

        private static CustomButton BombButton;  // 爆弾取り付けボタン
        private static CustomButton StartButton; // 起爆ボタン
        public static void SetupCustomButton(HudManager hm)
        {
            BombButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        var target = HudManagerStartPatch.SetTarget();
                        new LateTask(() =>
                        {
                            AttachBomb(target);
                        }, BombTime.GetFloat(), "Time bomber attach");
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && PlayerControl.LocalPlayer.IsRole(RoleId.TimeBomber) && HudManagerStartPatch.SetTarget(); },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => { ResetBombCoolDown(); },
                RoleClass.SelfBomber.GetButtonSprite(),
                new Vector3(0, 1, 0),
                hm,
                hm.AbilityButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("TimeBomberBombName"),
                showButtonText = true
            };

            StartButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        var target = HudManagerStartPatch.SetTarget();
                        ResetStartCoolDown();
                        AttachBomb(target);
                        NowBombTime += ExtensionKillCool.GetFloat();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && PlayerControl.LocalPlayer.IsRole(RoleId.TimeBomber); },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => { ResetStartCoolDown(); },
                RoleClass.SelfBomber.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                hm,
                hm.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("TimeBomberStartName"),
                showButtonText = true
            };
        }

        public static void ResetStartCoolDown()
        {
            StartButton.Timer = StartTime.GetFloat();
            StartButton.MaxTimer = StartTime.GetFloat();
        }
        public static void ResetBombCoolDown()
        {
            BombButton.Timer = NowBombTime;
            BombButton.MaxTimer = NowBombTime;
        }
    }
}