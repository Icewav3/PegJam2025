using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    private SceneGod.GameState targetState;

    public void ChangeScene()
    {
        string methodName = $"Enter{targetState}State";
        SceneGod.SInstance.SendMessage(methodName);
    }
}