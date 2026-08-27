using System.ComponentModel.Design;
using UnityEngine;

//? Does this need to be a MonoBehavior
public class GameManager : MonoBehaviour, IGameManager
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Transform _gameViewParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private GameView _gameViewPrefab;
    #endregion

    #region Variables
    private GameView _currentGameView;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.Log("Creating game.");
        CreateGameView();
    }

    void OnDestroy()
    {
        
    }
    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void CreateGameView()
    {
        // Currently only one in there now.
        GameModeDef def = GameModeDefList.GetDefById(GameModeIds.Default);
        Map map = CreateMap(def);

        GameViewModel gameViewModel = new GameViewModel(def.Id, map);
        
        if (_currentGameView == null)
        {
            _currentGameView = Instantiate(_gameViewPrefab, _gameViewParentTransform);
        }

        _currentGameView.SetViewModel(gameViewModel);
    }

    private Map CreateMap(GameModeDef def)
    {
        return new Map(def.TotalRows, def.TotalColumns);
    }
}
