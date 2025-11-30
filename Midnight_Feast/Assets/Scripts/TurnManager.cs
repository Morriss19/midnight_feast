using UnityEngine;
public class TurnManager
{
    public event System.Action OnTick;
    public int m_TurnCount;

    public TurnManager()
    {
        m_TurnCount = 1;
    }

    public void Tick()
    {
        m_TurnCount += 1;
        Debug.Log("Current turn count : " + m_TurnCount);
        OnTick?.Invoke();
    }
}
