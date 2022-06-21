using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIMode
{
    Settlement,
    Combat,
    Piloted
}

public class AIManager : MonoBehaviour
{
    List<AICharacterBase> m_aiAgents = new List<AICharacterBase>();

    public void AddAgent(AICharacterBase agent)
    {
        if (!m_aiAgents.Contains(agent))
        {
            m_aiAgents.Add(agent);
        }
    }

    public void RemoveAgent(AICharacterBase agent)
    {
        if (m_aiAgents.Contains(agent))
        {
            m_aiAgents.Remove(agent);
        }
    }
}
