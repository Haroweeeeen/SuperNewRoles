using HarmonyLib;
using SuperNewRoles.Patches;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace SuperNewRoles.Patches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuPatch
    {
        private static bool horseButtonState = HorseModeOption.enableHorseMode;
        private static Sprite horseModeOffSprite = null;

        private static void Prefix()
        {
            // Horse mode stuff
            ClientModOptionsPatch.SelectionBehaviour horseModeSelectionBehavior = new ClientModOptionsPatch.SelectionBehaviour("Enable Horse Mode", () => HorseModeOption.enableHorseMode = ConfigRoles.EnableHorseMode.Value = !ConfigRoles.EnableHorseMode.Value, ConfigRoles.EnableHorseMode.Value);

            GameObject bottomTemplate = GameObject.Find("InventoryButton");
            if (bottomTemplate == null) return;
            GameObject horseButton = Object.Instantiate(bottomTemplate, bottomTemplate.transform.parent);
            PassiveButton passiveHorseButton = horseButton.GetComponent<PassiveButton>();
            SpriteRenderer spriteHorseButton = horseButton.GetComponent<SpriteRenderer>();

            horseModeOffSprite = ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.HorseModeButtonOff.png", 75f);

            spriteHorseButton.sprite = horseModeOffSprite;

            passiveHorseButton.OnClick = new ButtonClickedEvent();

            passiveHorseButton.OnClick.AddListener((UnityEngine.Events.UnityAction)delegate
            {
                horseButtonState = horseModeSelectionBehavior.OnClick();
                if (horseModeOffSprite == null) horseModeOffSprite = ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.HorseModeButtonOff.png", 75f);
                spriteHorseButton.sprite = horseModeOffSprite;
                spriteHorseButton.transform.localScale *= -1;
                CredentialsPatch.LogoPatch.UpdateSprite();
                // Avoid wrong Player Particles floating around in the background
                PlayerParticles particles = GameObject.FindObjectOfType<PlayerParticles>();
                if (particles != null)
                {
                    particles.pool.ReclaimAll();
                    particles.Start();
                }
            });


            GameObject CreditsButton = Object.Instantiate(bottomTemplate, bottomTemplate.transform.parent);
            PassiveButton passiveCreditsButton = CreditsButton.GetComponent<PassiveButton>();
            SpriteRenderer spriteCreditsButton = CreditsButton.GetComponent<SpriteRenderer>();

            spriteCreditsButton.sprite = ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.CreditsButton.png", 75f);

            passiveCreditsButton.OnClick = new ButtonClickedEvent();

            passiveCreditsButton.OnClick.AddListener((UnityEngine.Events.UnityAction)delegate
            {
                SuperNewRolesPlugin.Logger.LogInfo("クリック");
                if (CredentialsPatch.LogoPatch.CreditsPopup != null)
                {
                    CredentialsPatch.LogoPatch.CreditsPopup.SetActive(true);
                }
            });
        }
    }

    public static class HorseModeOption
    {
        public static bool enableHorseMode = false;

        public static void ClearAndReloadMapOptions()
        {
            enableHorseMode = ConfigRoles.EnableHorseMode.Value;
        }
    }
}