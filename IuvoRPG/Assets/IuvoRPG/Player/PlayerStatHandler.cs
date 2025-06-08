using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [Header("Player Level & Experience")]
    [SerializeField] private Level playerLevel;

    [Header("Player Core Stats")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Stamina playerStamina;
    [SerializeField] private Mana playerMana;


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

    #region Getters & Setters

    public Level GetPlayerLevel() => playerLevel;
    public Health GetPlayerHealth() => playerHealth;
    public Stamina GetPlayerStamina() => playerStamina;
    public Mana GetPlayerMana() => playerMana;
    


    #endregion
}
