using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idatapersistant
{
   public void charger (SceneStat data);
    public  void sauvegarde(ref SceneStat data);
}
