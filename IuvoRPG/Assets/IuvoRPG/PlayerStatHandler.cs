using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [Header("Player Level & Experience")]
    [SerializeField] public Level playerLevel;

    [Header("Player Core Stats")]
    [SerializeField] public Health playerHealth;
    [SerializeField] public Stamina playerStamina;
    [SerializeField] public Mana playerMana;


    [Header("Player Additional Stats")]
    public int luck;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
