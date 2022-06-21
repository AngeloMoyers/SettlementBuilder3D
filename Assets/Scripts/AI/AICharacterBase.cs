using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterBase : MonoBehaviour
{ 
    protected AIManager m_aiManager;

    protected AIMode m_currentMode;
    protected AIModeBase m_currentAIMode;

    protected AIModeBase m_settlementAiMode;
    protected AIModeBase m_combatAiMode;

    protected bool m_isPiloted;

    private void Awake()
    {
        m_aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        m_aiManager.AddAgent(this);
    }

    protected virtual void Run()
    {
        //m_currentAiMode.Run();
    }

    public virtual void AddAIMode(AIMode mode, AIModeBase script)
    {
        if (script == null)
            return;

        switch (mode)
        {
            case AIMode.Settlement:
                m_settlementAiMode = script;
                break;
            case AIMode.Combat:
                m_combatAiMode = script;
                break;
            case AIMode.Piloted:
                break;
        }
    }

    private void OnDestroy()
    {
        if (m_aiManager != null)
            m_aiManager.RemoveAgent(this);
    }
}
