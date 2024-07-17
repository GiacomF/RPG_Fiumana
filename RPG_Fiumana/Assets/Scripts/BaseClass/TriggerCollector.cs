using UnityEngine;

public class TriggerCollector : MonoBehaviour
{
    [SerializeField] private Perk myPerk;

    private void Start()
    {
        myPerk = gameObject.GetComponent<Perk>();
    }

    private void OnTriggerEnter(Collider obj)
    {
        Entity entity = obj.GetComponent<Entity>();
        if (entity != null)
        {
            Perk clonePerk = myPerk;
            entity.stats.AttachPerk(clonePerk);
        }

        gameObject.SetActive(false);
    }
}
