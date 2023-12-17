using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idatapersistant
{
    void sauvegarde(SceneStat data);
    void charger(ref SceneStat data);
}
