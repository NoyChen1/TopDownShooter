using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController
{
    int MaxHealth { get; }
    int CurrentHealth { get; set; }


    void MovePlayer();
    void HandleAttack();
}
