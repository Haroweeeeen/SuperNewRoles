using SuperNewRoles.CustomRPC;
using SuperNewRoles.Roles;

namespace SuperNewRoles.Buttons
{
    public static class DestroyPlayerList
    {
        public static bool isDestroyKill(this PlayerControl player)
        {
            var IsDestroyKill = false;
            switch (player.getRole())
            {
                case RoleId.FastMaker:
                    return !RoleClass.FastMaker.IsCreatedMadMate;
                case RoleId.SecretlyKiller:
                case RoleId.DoubleKiller:
                case RoleId.Smasher:
                    IsDestroyKill = true;
                    break;
                    //キルボタン無効か
            }
            return IsDestroyKill;
        }
        public static bool isDestroyReport(this PlayerControl player)
        {
            var IsDestroyReport = false;
            switch (player.getRole())
            {
                case RoleId.Minimalist:
                    return !RoleClass.Minimalist.UseReport;
                case RoleId.Fox:
                    return !RoleClass.Fox.UseReport;
                case RoleId.Neet:
                    IsDestroyReport = true;
                    break;
                    //通報ボタン無効か
            }
            return IsDestroyReport;
        }
        public static bool isDestroySabo(this PlayerControl player)
        {
            var IsDestroySabo = false;
            switch (player.getRole())
            {
                case RoleId.Minimalist:
                    return !RoleClass.Minimalist.UseSabo;
                case RoleId.DoubleKiller:
                    return !RoleClass.DoubleKiller.CanUseSabo;
                    //サボタージュボタン無効か
            }
            return IsDestroySabo;
        }
        public static bool isDestroyUse(this PlayerControl player)
        {
            var IsDestroyUse = false;
            switch (player.getRole())
            {
                case RoleId.Neet:
                    IsDestroyUse = true;
                    break;
                    //使用ボタン無効か
            }
            return IsDestroyUse;
        }
        public static bool isDestroyVent(this PlayerControl player)
        {
            var IsDestroyVent = false;
            switch (player.getRole())
            {
                case RoleId.Minimalist:
                    return !RoleClass.Minimalist.UseVent;
                case RoleId.DoubleKiller:
                    return !RoleClass.DoubleKiller.CanUseVent;
                    //ベントボタン無効か
            }
            return IsDestroyVent;
        }
    }
    public class NormalButtonDestroy
    {
        public static void Postfix(PlayerControl player)
        {
            if (player.isDestroyKill())
            {
                FastDestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
            }
            if (player.isDestroyReport())
            {
                if (FastDestroyableSingleton<HudManager>.Instance.ReportButton.gameObject.active)
                {
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.SetActive(false);//通報
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.gameObject.SetActiveRecursively(false);
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.enabled = false;
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.enabled = false;
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.sprite = null;
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.buttonLabelText.enabled = false;
                    FastDestroyableSingleton<HudManager>.Instance.ReportButton.buttonLabelText.SetText("");
                }
            }
            if (player.isDestroySabo())
            {
                if (FastDestroyableSingleton<HudManager>.Instance.SabotageButton.gameObject.active)
                {
                    FastDestroyableSingleton<HudManager>.Instance.SabotageButton.gameObject.SetActive(false);
                }
            }
            if (player.isDestroyUse())
            {
                if (FastDestroyableSingleton<HudManager>.Instance.UseButton.gameObject.active)//使うボタンが有効の時
                {
                    FastDestroyableSingleton<HudManager>.Instance.UseButton.gameObject.SetActive(false);//使うボタンを無効化
                }
            }
            if (player.isDestroyVent())
            {
                if (FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.gameObject.active)
                {
                    FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.gameObject.SetActive(false);
                }
            }
        }
    }
}
