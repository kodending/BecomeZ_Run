using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    List<AudioSource> sounds;

    List<float> fVolumes;

    void Start()
    {
        fVolumes = new List<float>();

        //볼륨 원래 크기 저장하기
        for (int idx = 0; idx < sounds.Count; idx++)
        {
            float volume = sounds[idx].volume;

            fVolumes.Add(volume);
        }

        RefreshSound();
    }

    public void RefreshSound()
    {
        bool isMute = OptionSetting.os.GetIsMute();

        for (int idx = 0; idx < sounds.Count; idx++)
        {
            sounds[idx].volume = isMute ? 0 : fVolumes[idx];
        }
    }
}
