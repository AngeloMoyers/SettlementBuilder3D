using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePrinter : MonoBehaviour
{
    static TextMeshProUGUI m_messageText;

    static float m_startDisplaytime;
    static float m_currentMessageDisplayTime;

    private void Awake()
    {
        m_messageText = GetComponent<TextMeshProUGUI>();
        m_messageText.text = "";
        m_messageText.gameObject.SetActive(false);
    }
    public static void DisplayMessage(string message, float time = 5f)
    {
        m_messageText.gameObject.SetActive(true);

        m_messageText.text = message;
        m_messageText.color = new Color(255, 251, 0, 1); //Yellow

        m_startDisplaytime = Time.time;
        m_currentMessageDisplayTime = time;
    }

    public static void DisplayMessage(string message, Color color, float time = 5f)
    {
        m_messageText.gameObject.SetActive(true);

        m_messageText.text = message;
        m_messageText.color = color;

        m_startDisplaytime = Time.time;
        m_currentMessageDisplayTime = time;
    }

    private void Update()
    {
        HandleDisplayTimer();
    }

    private void HandleDisplayTimer()
    {
        if (Time.time > m_startDisplaytime + m_currentMessageDisplayTime)
        {
            m_messageText.text = "";
            m_messageText.gameObject.SetActive(false);

            m_startDisplaytime = 0;
            m_currentMessageDisplayTime = 0;
        }
    }
}
