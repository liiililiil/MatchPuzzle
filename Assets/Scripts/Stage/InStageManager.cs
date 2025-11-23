using UnityEngine;

public abstract class InStageManager : MonoBehaviour
{
    protected int _leftMovement;
    public int leftMovement
    {
        get { return _leftMovement; }
        set { _leftMovement = value; }
    }

    protected int _score;
    public int score
    {
        get { return _score; }
        set { _score = value; }
    }
}
