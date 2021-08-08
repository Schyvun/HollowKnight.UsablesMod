﻿using System.Collections;
using UnityEngine;
using UsablesMod.Usables;

namespace UsablesMod
{
    class UsabalesManager : MonoBehaviour
    {
        public void Run(string usableName)
        {
            IUsable usable = GetUsable(usableName);
            RunUsable(usable);
        }

        private IUsable GetUsable(string usableName)
        {
            switch (usableName)
            {
                default:
                    return new SampleUsable();
            }
        }

        private void RunUsable(IUsable usable)
        {
            GameManager.instance.StartCoroutine(RunUsableRoutine(usable));
        }

        private IEnumerator RunUsableRoutine(IUsable usable)
        {
            LogHelper.Log($"Running {usable.GetName()} routine");
            usable.Run();
            if (usable.IsRevertable())
            {
                float duration = usable.GetDuration();
                LogHelper.Log($"Reverting {usable.GetName()} in {duration}"); 
                yield return new WaitForSeconds(duration);
                usable.Revert();
            }
        }
    }
}
