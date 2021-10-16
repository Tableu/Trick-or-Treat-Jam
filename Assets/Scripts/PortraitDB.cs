using UnityEngine;

public class PortraitDB : MonoBehaviour
{
   private static PortraitDB _instance;

   public static PortraitDB Instance
   {
      get
      {
         return _instance;
      }
   }

   private void Awake()
   {
      if (Instance)
      {
         Destroy(gameObject);
         _instance = this;
      }
   }
}
