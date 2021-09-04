using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthComponent : MonoBehaviour
{
    public Slider healthSlider;
    private PlayerScript playerScript;
    public bool indestructible;

    [SerializeField] private int _health;
    public int Health {
        get {
            return _health;
        }
        set {
            _health = value;
            if (healthSlider != null) healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
            if (_health <= 0) OnDeath();
        }
    }
    [SerializeField] private int _maxHealth;
    public int MaxHealth {
        get {
            return _maxHealth;
        }
        set {
            _maxHealth = value;
            if (healthSlider != null) healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
            if (_health <= 0) OnDeath();
        }
    }
    public ScriptableFloat damageReceivedMultiplier;
    public GameObject damageReceivedMessage;
    public GameObject damageReceivedEffect;
    public Sprite deathSprite;

    void Start() {

        indestructible = false;
        
        if (GetComponent<PlayerScript>()) playerScript = GetComponent<PlayerScript>();

        if (GetComponent<Obstacle>()) indestructible = !GetComponent<Obstacle>().destructable;
    }

    public void TakeDamage(int damageTaken) {

        if (indestructible) return;

        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        if (damageReceivedEffect) Instantiate(damageReceivedEffect, transform.position, Quaternion.identity);

        if (playerScript != null) {
            Health -= Mathf.FloorToInt(damageTaken*damageReceivedMultiplier.Value);
            GameManager.Instance.playerManager.currentPlayerHealthVar.Value = Health;
            if (damageReceivedMessage) {
                GameObject fleetingDamageMessage = Instantiate(damageReceivedMessage, transform.position + randomOffset, Quaternion.identity);
                fleetingDamageMessage.GetComponentInChildren<TextMeshProUGUI>().text = (damageTaken * damageReceivedMultiplier.Value).ToString();
                Destroy(fleetingDamageMessage, 1.0f);
            }
        } else {
            Health -= damageTaken;

            if (damageReceivedMessage) {
                GameObject fleetingDamageMessage = Instantiate(damageReceivedMessage, transform.position + randomOffset, Quaternion.identity);
                fleetingDamageMessage.GetComponentInChildren<TextMeshProUGUI>().text = (damageTaken).ToString();
                Destroy(fleetingDamageMessage, 1.0f);
            }    
        }

        
    }

    void OnDeath() {
        GameManager.Instance.mapManager.mapObjects.Remove(this.gameObject);
        if (playerScript is null) {
            Enemy thisEnemy = GetComponent<Enemy>();
            if (thisEnemy != null && GameManager.Instance.enemyManager.currentEnemies.Contains(thisEnemy)) {
                GameManager.Instance.enemyManager.currentEnemies.Remove(thisEnemy);
                if (deathSprite) {
                    GetComponentInChildren<Animator>().enabled = false;
                    GetComponentInChildren<SpriteRenderer>().sprite = deathSprite;
                    Destroy(gameObject, 1);
                } else {
                    Destroy(gameObject);
                }
            }
            Obstacle thisObstacle = GetComponent<Obstacle>();
            if (thisObstacle != null && GameManager.Instance.mapManager.destructableObjects.Contains(thisObstacle)) {
                GameManager.Instance.mapManager.destructableObjects.Remove(thisObstacle);
                Destroy(gameObject);
            }
            TerminalScript thisTerminal = GetComponent<TerminalScript>();
            if (thisTerminal != null && GameManager.Instance.mapManager.terminalsInLevel.Contains(thisTerminal)) {
                GameManager.Instance.mapManager.terminalsInLevel.Remove(thisTerminal);
                GameManager.Instance.uiManager.terminalsReachedText.text = "Terminals Reached: " + GameManager.Instance.NumberOfActiveTerminals.ToString() + "/" + GameManager.Instance.mapManager.terminalsInLevel.Count.ToString();
                GameManager.Instance.uiManager.terminalsIntactText.text = "Terminals Intact: " + GameManager.Instance.mapManager.terminalsInLevel.Count.ToString() + "/" + GameManager.Instance.mapManager.maxTerminals.ToString();
                if (GameManager.Instance.mapManager.terminalsInLevel.Count <= Mathf.FloorToInt(GameManager.Instance.mapManager.maxTerminals/2)) GameManager.Instance.EndGame(false);
                Destroy(gameObject);
            }
            
        } else {
            GameManager.Instance.EndGame(false);
            if (deathSprite) {
                    GetComponentInChildren<Animator>().enabled = false;
                    GetComponentInChildren<SpriteRenderer>().sprite = deathSprite;
                    Destroy(gameObject, 1);
                } else {
                    Destroy(gameObject);
                }
        }
    }
}
