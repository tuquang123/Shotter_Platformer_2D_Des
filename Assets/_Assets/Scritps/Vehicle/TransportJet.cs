using UnityEngine;
using System.Collections;
using Spine.Unity;
using DG.Tweening;

public class TransportJet : MonoBehaviour
{
    public Transform playerStandPoint;
    public SkeletonAnimation skeletonAnimation;
    [SpineAnimation]
    public string move;
    [SpineBone]
    public string boneStand;

    private AudioSource audioMove;
    private AudioClip soundMove;


    void Awake()
    {
        audioMove = GetComponent<AudioSource>();
        soundMove = SoundManager.Instance.GetAudioClip(StaticValue.SOUND_SFX_PLANE_MOVE);

        //EventDispatcher.Instance.RegisterListener(EventID.StopSoundAndMusic, (sender, param) => EnableAudioMove(false));
    }

    public void Active(Vector2 destination)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, move, true);
        EnableAudioMove(true);
        transform.DOMove(destination, 4f).OnComplete(() =>
        {
            EventDispatcher.Instance.PostEvent(EventID.TransportJetDone);
            Invoke("Escape", 1f);
        });
    }

    private void Escape()
    {
        Vector2 escapePoint = transform.position;
        escapePoint.y += 5f;

        transform.DOMove(escapePoint, 4f).OnComplete(() =>
        {
            EnableAudioMove(false);
            //EventDispatcher.Instance.RemoveListener(EventID.StopSoundAndMusic, (sender, param) => EnableAudioMove(false));
            Destroy(gameObject);
        });
    }

    private void EnableAudioMove(bool isEnable)
    {
        if (isEnable)
        {
            if (audioMove.clip == null)
            {
                audioMove.clip = soundMove;
                audioMove.loop = true;
            }

            audioMove.Play();
        }
        else
        {
            if (audioMove.clip = soundMove)
            {
                audioMove.clip = null;
            }

            audioMove.Stop();
        }
    }
}
