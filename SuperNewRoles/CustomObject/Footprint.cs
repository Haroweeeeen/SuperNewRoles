using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperNewRoles.CustomObject
{
    public class Footprint
    {
        public static List<Footprint> footprints = new();
        private static Sprite sprite;
        private Color color;
        public GameObject footprint;
        private SpriteRenderer spriteRenderer;
        private PlayerControl owner;
        private bool anonymousFootprints;

        public static Sprite getFootprintSprite()
        {
            if (sprite) return sprite;
            sprite = ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.Footprint.png", 600f);
            return sprite;
        }
        public Footprint(float footprintDuration, bool anonymousFootprints, PlayerControl player, Vector3? pos = null)
        {
            this.owner = player;
            this.anonymousFootprints = anonymousFootprints;
            if (anonymousFootprints)
                this.color = Palette.PlayerColors[6];
            else
                this.color = Palette.PlayerColors[(int)player.Data.DefaultOutfit.ColorId];

            Vector3 posdata = pos != null ? (Vector3)pos : player.transform.position;
            this.footprint = new GameObject("Footprint");
            Vector3 position = new(posdata.x, posdata.y, posdata.z + 1f);
            this.footprint.transform.position = position;
            this.footprint.transform.localPosition = position;
            this.footprint.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0f, 360f));
            this.footprint.transform.SetParent(player.transform.parent);

            this.spriteRenderer = this.footprint.AddComponent<SpriteRenderer>();
            this.spriteRenderer.sprite = getFootprintSprite();
            this.spriteRenderer.color = this.color;

            this.footprint.SetActive(true);

            if (footprintDuration > 0)
            {
                footprints.Add(this);
                HudManager.Instance.StartCoroutine(Effects.Lerp(footprintDuration, new Action<float>((p) =>
                {
                    Color c = this.color;

                    if (this.spriteRenderer) this.spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

                    if (p == 1f && this.footprint != null)
                    {
                        UnityEngine.Object.Destroy(this.footprint);
                        footprints.Remove(this);
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

            this.footprint = new GameObject("Footprint");
            Vector3 position = new(pos.x, pos.y, 1f);
            this.footprint.transform.position = position;
            this.footprint.transform.localPosition = position;

            this.footprint.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));


            this.spriteRenderer = this.footprint.AddComponent<SpriteRenderer>();
            this.spriteRenderer.sprite = getFootprintSprite();
            this.spriteRenderer.color = this.color;

            this.footprint.SetActive(true);

            if (footprintDuration > 0)
            {
                HudManager.Instance.StartCoroutine(Effects.Lerp(footprintDuration, new Action<float>((p) =>
                {
                    Color c = this.color;

                    if (this.spriteRenderer) this.spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

                    if (p == 1f && this.footprint != null)
                    {
                        UnityEngine.Object.Destroy(this.footprint);
                    }
                })));
            }
        }
    }
}