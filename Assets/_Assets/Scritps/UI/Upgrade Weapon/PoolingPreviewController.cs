using UnityEngine;
using System.Collections;

public class PoolingPreviewController : Singleton<PoolingPreviewController>
{
    public Transform group;

    public ObjectPooling<BulletPreviewUzi> uzi;
    public ObjectPooling<BulletPreviewM4> m4;
    public ObjectPooling<BulletPreviewScarH> scarH;
    public ObjectPooling<BulletPreviewAWP> awp;
    public ObjectPooling<BulletPreviewShotgun> shotgun;
    public ObjectPooling<BulletPreviewP100> p100;
    public ObjectPooling<BulletPreviewBullpup> bullpup;
    public ObjectPooling<BulletPreviewSniperRifle> sniperRifle;
    public ObjectPooling<BulletPreviewTeslaMini> teslaMini;

    public ObjectPooling<BulletPreviewSpread> spread;
    public ObjectPooling<BulletPreviewFamas> famas;
    public ObjectPooling<BulletPreviewRocketChaser> rocketChaser;
    public ObjectPooling<BulletPreviewSplit> split;
    public ObjectPooling<BulletPreviewFireball> fireball;
    public ObjectPooling<BulletPreviewKamePower> kamePower;

    public ObjectPooling<BaseGrenadePreview> grenadeBase;
    public ObjectPooling<GrenadePreviewTet> grenadeTet;

    public ObjectPooling<TextDamage> textDamage;

    private void Awake()
    {
        uzi = new ObjectPooling<BulletPreviewUzi>();
        m4 = new ObjectPooling<BulletPreviewM4>();
        scarH = new ObjectPooling<BulletPreviewScarH>();
        awp = new ObjectPooling<BulletPreviewAWP>();
        shotgun = new ObjectPooling<BulletPreviewShotgun>();
        p100 = new ObjectPooling<BulletPreviewP100>();
        bullpup = new ObjectPooling<BulletPreviewBullpup>();
        sniperRifle = new ObjectPooling<BulletPreviewSniperRifle>();
        teslaMini = new ObjectPooling<BulletPreviewTeslaMini>();

        spread = new ObjectPooling<BulletPreviewSpread>();
        famas = new ObjectPooling<BulletPreviewFamas>();
        rocketChaser = new ObjectPooling<BulletPreviewRocketChaser>();
        split = new ObjectPooling<BulletPreviewSplit>();
        fireball = new ObjectPooling<BulletPreviewFireball>();
        kamePower = new ObjectPooling<BulletPreviewKamePower>();

        grenadeBase = new ObjectPooling<BaseGrenadePreview>();
        grenadeTet = new ObjectPooling<GrenadePreviewTet>();

        textDamage = new ObjectPooling<TextDamage>();
    }
}
