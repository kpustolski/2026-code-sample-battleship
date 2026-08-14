using UnityEngine;

//? Does this need to be a MonoBehavior
public class GameManager : MonoBehaviour, IGameManager
{
    private IGame _currentGame;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGame();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void CreateGame()
    {
        _currentGame = new Game();
    }
}
