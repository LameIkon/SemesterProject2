using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
   public static Persistence _Instance;

   private void Awake()
   {
      if (_Instance == null)
      {
         _Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }
}
