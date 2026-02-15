using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class GameMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionProperty showButton;
    public GameObject gameMenu;
    public Transform head;
    public float spawndistance = 2;
    public ChargeManager manager;
    public TMP_Text textMag;
    private List<Charge> charges;


    void Start()
    {
        charges = ChargeManager.charges;
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            gameMenu.SetActive(!gameMenu.activeSelf);

            gameMenu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawndistance;

        }

        gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
        gameMenu.transform.forward *= -1;
        textMag.text = charges[charges.Count - 1].charge.ToString();

    }

    public void setview(GameObject menu2)
    {
       Vector3 loc = new Vector3(gameMenu.transform.position.x, gameMenu.gameObject.transform.position.y, gameMenu.gameObject.transform.position.z);
        
        gameMenu.SetActive(false);
        gameMenu = menu2;
        gameMenu.transform.position = loc;
        gameMenu.SetActive(true);
    }
}
