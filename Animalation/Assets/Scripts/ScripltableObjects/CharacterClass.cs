using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "CharacterClass")]
public class CharacterClass : ScriptableObject {

    //public Sprite classIcon;
    public string className;
    public string classDescription;
    public Vector2 classSize;
    public Vector2 classOffset;
    public int classMass;
    public float classSpeed;
    public float classJumpForce;
}
