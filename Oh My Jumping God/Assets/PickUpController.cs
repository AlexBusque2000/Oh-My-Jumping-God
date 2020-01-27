using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public Text scoreText;

    [SerializeField]
    int score;


    void Start()
    {
        score = 0;
        scoreText.text = "Puntuación: " + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            score++;
            scoreText.text = "Puntuación: " + score;
            Destroy(other.gameObject);
        }
    }
}
