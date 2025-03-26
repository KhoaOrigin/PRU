using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyEnenmy : Enemy
{
    [SerializeField] private GameObject energyObject;
    
    protected override void Die()
    {
        if(energyObject != null)
        {
            GameObject enegy = Instantiate(energyObject, transform.position, Quaternion.identity);
            Destroy(enegy, 6f);
        }
        base.Die();
    }
}
