using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SceneStat
{
   public int Scene;
    public int VieBoss;
    public float VieJoueur;
    public bool CinématiqueenCour;
    public Vector3  PositionJoueur;

    public SceneStat()
    {
        VieJoueur = 5;
         VieBoss = 100;
        Scene = 1;
        CinématiqueenCour = true;
    }

}
