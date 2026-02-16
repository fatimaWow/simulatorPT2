
using TMPro;
using UnityEngine;

public class countDownTImer : MonoBehaviour
{
    public TMP_Text timertext;
    public float currentTime = 60f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Decrement time every frame

            // Format the time to display only whole seconds
            timertext.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0; // Ensure time doesn't show negative numbers
                timertext.text = "0";
                // Add code here for when the timer finishes (e.g., game over, end level)
                Debug.Log("Countdown Finished!");
            }
        }
    }
}

