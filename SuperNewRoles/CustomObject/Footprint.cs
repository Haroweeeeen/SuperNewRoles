using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperNewRoles.CustomObject
{
    public class Footprint
    {
        private static Sprite sprite;
        private Color color;
        private GameObject footprint;
        private SpriteRenderer spriteRenderer;
        private PlayerControl owner;
        private bool anonymousFootprints;

        public static Sprite getFootprintSprite()
        {
            if (sprite) return sprite;
            sprite = ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.Footprint.png", 600f);
            return sprite;
        }

        public Footprint(float footprintDuration, bool anonymousFootprints, PlayerControl player)
        {
            this.owner = player;
            this.anonymousFootprints = anonymousFootprints;
            if (anonymousFootprints)
                this.color = Palette.PlayerColors[6];
            else
                this.color = Palette.PlayerColors[(int)player.Data.DefaultOutfit.ColorId];

            footprint = new GameObject("Footprint");
            Vector3 position = new(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            footprint.transform.position = position;
            footprint.transform.localPosition = position;
            footprint.transform.SetParent(player.transform.parent);

            footprint.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));


            spriteRenderer = footprint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getFootprintSprite();
            spriteRenderer.color = color;

            footprint.SetActive(true);

            if (footprintDuration > 0)
            {
                HudManager.Instance.StartCoroutine(Effects.Lerp(footprintDuration, new Action<float>((p) =>
                {
                    Color c = color;

                    if (spriteRenderer) spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

                    if (p == 1f && footprint != null)
                    {
                        UnityEngine.Object.Destroy(footprint);
                    }
                })));
            }
        }

        public Footprint(float footprintDuration, bool anonymousFootprints, Vector2 pos, Color? color = null)
        {
            this.anonymousFootprints = anonymousFootprints;
            if (anonymousFootprints || color == null)
                this.color = Palette.PlayerColors[6];
            else
                this.color = (Color)color;

            footprint = new GameObject("Footprint");
            Vector3 position = new(pos.x, pos.y, 1f);
            footprint.transform.position = position;
            footprint.transform.localPosition = position;

            footprint.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));


            spriteRenderer = footprint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getFootprintSprite();
            spriteRenderer.color = this.color;

            footprint.SetActive(true);

            if (footprintDuration > 0)
            {
                HudManager.Instance.StartCoroutine(Effects.Lerp(footprintDuration, new Action<float>((p) =>
                {
                    Color c = this.color;

                    if (spriteRenderer) spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

                    if (p == 1f && footprint != null)
                    {
                        UnityEngine.Object.Destroy(footprint);
                    }
                })));
            }
        }
    }
}