using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerHealth
{
    int MaxHealth { get; }
    int CurrentHealth { get; set; }

    void TakeDamage(int damage);

}
