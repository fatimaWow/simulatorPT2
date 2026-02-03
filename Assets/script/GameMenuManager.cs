using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor;


public class GameMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionProperty showButton;
    public GameObject gameMenu;
    public Transform head;
    public float spawndistance = 2;


    void Start()
    {

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

    }

    public void setview(GameObject menu2)
    {
        gameMenu.SetActive(false);

        gameMenu = menu2;
        gameMenu.SetActive(true);
    }
}
