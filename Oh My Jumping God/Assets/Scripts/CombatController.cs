using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    public int vidas;
    public Text vidasText;
    public GameObject damageZone;

    [SerializeField]
    int currentVidas;

    // Start is called before the first frame update
    void Start()
    {
        currentVidas = vidas;
        vidasText.text = "Vidas: " + currentVidas;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            damageZone.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currentVidas--;
            vidasText.text = "Vidas: " + currentVidas;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
