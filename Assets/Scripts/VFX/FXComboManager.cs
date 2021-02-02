using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXComboManager : MonoBehaviour
{
    public Transform VFXStorage;
    public bool IsLooping = false;

    [SerializeField] private GameObject[] AllEffects;

    public void SpawnComboEffect()
    {
        foreach (GameObject Effect in AllEffects)
        {
            GameObject TemporaryEffect = Instantiate(Effect, transform.position, transform.rotation, VFXStorage);

            VFXLifetimeManager TemporaryEffect_VFXLM;
            TemporaryEffect.TryGetComponent<VFXLifetimeManager>(out TemporaryEffect_VFXLM);

            if (TemporaryEffect_VFXLM != null)
            {
                TemporaryEffect_VFXLM.IsLooping = IsLooping;
            }

            VFXSpawner TemporaryEffect_VFXS;
            TemporaryEffect.TryGetComponent<VFXSpawner>(out TemporaryEffect_VFXS);

            if (TemporaryEffect_VFXS != null)
            {
                TemporaryEffect_VFXS.VFXStorage = VFXStorage;
            }
        }
    }
}
