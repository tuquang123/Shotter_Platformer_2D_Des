using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingSubmarine : MonoBehaviour
{
    public float defaultOffset = 7.5f;
    public float offset = 7.5f;

    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerObstacle, GetCloser);
        EventDispatcher.Instance.RegisterListener(EventID.BoatStop, Stop);
    }

    void Update()
    {
        if (GameController.Instance.Player)
        {
            Vector3 v = GameController.Instance.Player.transform.position;
            v.x -= offset;
            v.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, v, 1.5f * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        offset = defaultOffset;
    }

    private void GetCloser(Component arg1, object arg2)
    {
        offset--;
    }
    private void Stop(Component arg1, object arg2)
    {
        offset = defaultOffset;
    }
}
