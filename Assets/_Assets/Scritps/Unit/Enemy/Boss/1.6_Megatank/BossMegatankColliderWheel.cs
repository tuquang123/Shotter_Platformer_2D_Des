using UnityEngine;
using System.Collections;

public class BossMegatankColliderWheel : MonoBehaviour
{
    public AudioClip soundHit;

    private BossMegatank boss;

    private void Awake()
    {
        boss = transform.root.GetComponent<BossMegatank>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                float damage = boss.HpPercent > 0.5f ? ((SO_BossMegatankStats)boss.baseStats).GoreDamage : ((SO_BossMegatankStats)boss.baseStats).RageGoreDamage;
                AttackData atkData = new AttackData(boss, damage);
                unit.TakeDamage(atkData);

                if (unit.isDead == false)
                    unit.FallBackward(1.5f);
            }

            SoundManager.Instance.PlaySfx(soundHit);
            CameraFollow.Instance.AddShake(0.3f, 0.5f);
            gameObject.SetActive(false);
        }
    }
}
