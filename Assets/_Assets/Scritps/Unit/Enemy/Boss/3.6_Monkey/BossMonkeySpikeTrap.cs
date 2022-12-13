using UnityEngine;
using System.Collections;

public class BossMonkeySpikeTrap : MonoBehaviour
{
    public Transform mostLeftPoint;
    public Transform mostRightPoint;
    public Spike spikePrefab;

    private BossMonkey boss;
    private int totalSpikes;
    private int activeSpikes;
    private float spikeDamage;
    private float spikeDropSpeed;
    private WaitForSeconds waitDelaySpike;
    private IEnumerator coroutineDropSpikes;

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeTrapStart, (sender, param) => ActiveSpikeTrap((DropSpikeData)param));
        EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeDeactive, (sender, param) => OnSpikeDeactive());
    }

    private void ActiveSpikeTrap(DropSpikeData data)
    {
        boss = data.boss;
        totalSpikes = data.numberSpikes;
        spikeDamage = data.spikeDamage;
        spikeDropSpeed = data.spikeDropSpeed;
        waitDelaySpike = new WaitForSeconds(data.spikeDelay);

        if (coroutineDropSpikes != null)
        {
            StopCoroutine(coroutineDropSpikes);
            coroutineDropSpikes = null;
        }

        coroutineDropSpikes = CoroutineDropSpikes();
        StartCoroutine(coroutineDropSpikes);
    }

    private IEnumerator CoroutineDropSpikes()
    {
        int countSpike = 0;
        activeSpikes = totalSpikes;

        while (countSpike < totalSpikes)
        {
            SpawnSpike();
            countSpike++;
            yield return waitDelaySpike;
        }

        coroutineDropSpikes = null;
    }

    private void SpawnSpike()
    {
        Spike spike = PoolingController.Instance.poolSpike.New();

        if (spike == null)
        {
            spike = Instantiate(spikePrefab) as Spike;
        }

        AttackData atkData = new AttackData(boss, spikeDamage);

        float randomX = Random.Range(mostLeftPoint.position.x, mostRightPoint.position.x);
        Vector2 v = mostLeftPoint.position;
        v.x = randomX;

        spike.Active(atkData, v, spikeDropSpeed);
    }

    private void OnSpikeDeactive()
    {
        activeSpikes--;

        if (activeSpikes <= 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeTrapEnd);
        }
    }
}

public class DropSpikeData
{
    public BossMonkey boss;
    public int numberSpikes;
    public float spikeDamage;
    public float spikeDropSpeed;
    public float spikeDelay;
}

