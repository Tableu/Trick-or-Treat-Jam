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
         if (_instance == null && !FindObjectOfType<PortraitDB>())
         {
            _instance = new GameObject("PortraitDB").AddComponent<PortraitDB>();
         }
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
      DontDestroyOnLoad(this);
   }

   public Sprite GetPortrait(Character charID, string expression)
   {
      return characters[(int) charID].GetExpression(expression);
   }

   public SpriteRenderer GetSpriteRenderer(Character charID)
   {
      return characters[(int) charID].characterSpriteRenderer;
   }
   public AudioManager.Sound GetDialogueSound(Character charID)
   {
      return characters[(int) charID].dialogueSound;
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
   public AudioManager.Sound dialogueSound;
   public SpriteRenderer characterSpriteRenderer;
   public List<Expression> expressions;

   public Sprite GetExpression(string id)
   {
      return expressions.Find(x => x.id == id).sprite;
   }
}
[System.Serializable]
public class Expression
{
   public string id;
   public Sprite sprite;
}