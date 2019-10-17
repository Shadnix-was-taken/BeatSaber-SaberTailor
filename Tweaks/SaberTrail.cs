﻿using BS_Utils.Utilities;
using SaberTailor.Settings;
using System.Collections;
using UnityEngine;
using Xft;
using LogLevel = IPA.Logging.Logger.Level;

namespace SaberTailor.Tweaks
{
    public class SaberTrail : MonoBehaviour, ITweak
    {
        public string Name => "SaberTrail";
        public bool IsPreventingScoreSubmission => false;

#pragma warning disable IDE0051 // Used by MonoBehaviour
        private void Awake() => Load();
#pragma warning restore IDE0051 // Used by MonoBehaviour

        private void Load()
        {
            StartCoroutine(ApplyGameCoreModifications());
        }

        private IEnumerator ApplyGameCoreModifications()
        {
            BasicSaberModelController[] basicSaberModelControllers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
            foreach (BasicSaberModelController basicSaberModelController in basicSaberModelControllers)
            {
                SaberWeaponTrail saberTrail = ReflectionUtil.GetPrivateField<SaberWeaponTrail>(basicSaberModelController, "_saberWeaponTrail");
                if (saberTrail.name == "BasicSaberModel")
                {
                    ModifyTrail(saberTrail, Configuration.Trail.Length);
                    this.Log("Successfully modified trails!");
                }
            }

            yield return null;
        }

        private void ModifyTrail(XWeaponTrail trail, int length)
        {
            if (Configuration.Trail.TrailEnabled)
            {
                trail.enabled = true;
                ReflectionUtil.SetPrivateField(trail, "_maxFrame", length);
                ReflectionUtil.SetPrivateField(trail, "_granularity", length * 3);
            }
            else
            {
                trail.enabled = false;
            }
        }
    }
}
