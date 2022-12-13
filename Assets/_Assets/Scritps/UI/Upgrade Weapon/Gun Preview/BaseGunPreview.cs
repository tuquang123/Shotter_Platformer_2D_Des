using UnityEngine;
using System.Collections;

public class BaseGunPreview : MonoBehaviour
{
    public int id;
    public SO_GunStats baseStats;

    public BaseBulletPreview bulletPrefab;
    public BaseMuzzle muzzlePrefab;
    public Transform firePoint;
    public Transform muzzlePoint;
    public ParticleSystem bulletTrash;
    public AudioClip soundAttack;

    protected BaseMuzzle muzzle;

    public virtual void Fire()
    {
        if (bulletTrash)
            bulletTrash.Play();

        //PlaySoundAttack();
        ReleaseBullet();
    }

    public virtual void ActiveMuzzle()
    {
        if (muzzlePrefab)
        {
            if (muzzle == null)
            {
                muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
            }

            muzzle.Active();
        }
    }

    protected virtual void ReleaseBullet() { }

    protected virtual void PlaySoundAttack()
    {
        if (soundAttack)
        {
            SoundManager.Instance.PlaySfx(soundAttack);
        }
    }
}
