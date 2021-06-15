using Assets.Code.CollectableObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollector
{
    void CollectPallet(Pallet pallet);

    void PlacePallet(Transform place, Transform parent);

    void CollectMap(MapPart mapPart);

    void CollectWarrior(Warrior warrior);

}
