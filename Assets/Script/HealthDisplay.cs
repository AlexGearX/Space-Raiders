using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI healthText;
    Player player;



    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        int health = player.GetHealth();
        if (health >= 1)
        {
        healthText.text = player.GetHealth().ToString();
        }
        else
        {
            healthText.text = 0.ToString();
        }
    }
}
