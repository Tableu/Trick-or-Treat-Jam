using System.Collections.Generic;
using UnityEngine;

public class PortraitDB : MonoBehaviour
{
   private static PortraitDB _instance;
   [SerializeField] public List<CharacterExpressions> characters;
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
      }
      _instance = this;
      DontDestroyOnLoad(gameObject);
   }

   public Sprite GetPortrait(Character charID, string expression)
   {
      return characters[(int) charID].GetExpression(expression);
   }
}

public enum Character
{
   Sherry,
   Danny,
   Policeman
}
[System.Serializable]
public class CharacterExpressions
{
   public Character charID;
   public List<Expression> expressions;

   public Sprite GetExpression(string ID)
   {
      return expressions.Find(x => x.id == ID).sprite;
   }
}
[System.Serializable]
public class Expression
{
   public string id;
   public Sprite sprite;
}