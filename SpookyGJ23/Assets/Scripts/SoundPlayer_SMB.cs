using UnityEngine;

public class SoundPlayer_SMB : StateMachineBehaviour
{
    [SerializeField] Sound sound;
    AudioSource audioSource;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource == null) audioSource = animator.GetComponent<AudioSource>();

        audioSource.clip = sound.GetClip;
        audioSource.loop = sound.loop;
        audioSource.volume = sound.GetVolume;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource != null) audioSource.Stop();
    }
}