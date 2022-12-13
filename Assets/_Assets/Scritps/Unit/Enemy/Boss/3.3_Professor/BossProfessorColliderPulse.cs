using UnityEngine;
using System.Collections;

public class BossProfessorColliderPulse : MonoBehaviour
{
    public BossProfessorEnergyPulse pulse;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit && pulse.pulseVictims.Contains(unit) == false)
            {
                pulse.pulseVictims.Add(unit);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit && pulse.pulseVictims.Contains(unit))
            {
                pulse.pulseVictims.Remove(unit);
            }
        }
    }
}
