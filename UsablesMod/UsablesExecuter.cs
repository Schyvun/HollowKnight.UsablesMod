﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UsablesMod.Usables;

namespace UsablesMod
{
    class UsablesExecuter : MonoBehaviour
    {
        private readonly ActiveUsablesBar usablesBar;

        internal UsablesExecuter()
        {
            usablesBar = new ActiveUsablesBar();
        }

        public void RunUsable((IUsable Usable, GameObject icon) pair)
        {
            GameManager.instance.StartCoroutine(RunUsableRoutine(pair));
        }

        private IEnumerator RunUsableRoutine((IUsable Usable, GameObject icon) pair)
        {
            LogHelper.Log($"Running {pair.Usable.GetName()} routine");
            pair.Usable.Run();
            if (pair.Usable is IRevertable)
            {
                IRevertable revertable = pair.Usable as IRevertable;
                yield return Revert(revertable, pair.Usable.GetName(), pair.icon);
            }
            else
            {
                Destroy(pair.icon);
            }
        }

        private IEnumerator Revert(IRevertable revertable, string name, GameObject icon)
        {
            float duration = revertable.GetDuration();
            if (duration > 0)
            {
                usablesBar.Add(icon, revertable.GetDuration);

                yield return new WaitForSeconds(duration);

                float newDuration = revertable.GetDuration();
                float newDurationDiff = newDuration - duration;
                while (newDurationDiff > 0)
                {
                    LogHelper.Log($"Adding {newDurationDiff} before reverting {name}");
                    duration = newDuration;

                    yield return new WaitForSeconds(newDurationDiff);

                    newDuration = revertable.GetDuration();
                    newDurationDiff = newDuration - duration;
                }

                usablesBar.Remove(icon);
            }

            LogHelper.Log($"Reverting {name}");
            revertable.Revert();

            Destroy(icon);
        }
    }
}
