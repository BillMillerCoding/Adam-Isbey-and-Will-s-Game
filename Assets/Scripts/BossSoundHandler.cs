using UnityEngine;

public class BossSoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource playbackSource;
    [SerializeField] private AudioClip NuclearSound;
    [SerializeField] [Range(0f, 1f)] private float NuclearVolume = 1f;
    [SerializeField] private AudioClip ExplodingRingsSound;
    [SerializeField] [Range(0f, 1f)] private float ExplodingRingsVolume = 1f;
    [SerializeField] private AudioClip LazerCastSound;
    [SerializeField] [Range(0f, 1f)] private float LazerCastVolume = 1f;
    [SerializeField] private AudioClip PunchSound;
    [SerializeField] [Range(0f, 1f)] private float PunchVolume = 1f;

    private void Awake()
    {
        if (playbackSource == null)
        {
            playbackSource = GetComponent<AudioSource>();
        }
    }

    private void PlayIfAssigned(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            return;
        }

        float clampedVolume = Mathf.Clamp01(volume);

        if (playbackSource != null)
        {
            playbackSource.PlayOneShot(clip, clampedVolume);
            return;
        }

        AudioSource.PlayClipAtPoint(clip, transform.position, clampedVolume);
    }

    public void PlayNuclearSound()
    {
        PlayIfAssigned(NuclearSound, NuclearVolume);
    }

    public void PlayExplodingRingsSound()
    {
        PlayIfAssigned(ExplodingRingsSound, ExplodingRingsVolume);
    }

    public void PlayLazerCastSound()
    {
        PlayIfAssigned(LazerCastSound, LazerCastVolume);
    }

    public void PlayPunchSound()
    {
        PlayIfAssigned(PunchSound, PunchVolume);
    }
}
