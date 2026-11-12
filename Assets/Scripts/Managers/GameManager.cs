using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public partial class GameManager : MonoBehaviour, IGameManager
{
    #region Unity References
    [Header("Options")]
    [SerializeField]
    private Id<GameMode> _selectedGameMode = GameModeIds.Default;

    [Header("Asset References")]
    [SerializeField]
    private Transform _gameViewParentTransform;
    [SerializeField]
    private Transform _overlayViewParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private GameView _gameViewPrefab;
    [SerializeField]
    private OverlayView _overlayViewPrefab;

    [Header("Scriptable Objects")]
    [SerializeField]
    private List<GameModeRenderDef> _gameModeRenderDefs = new List<GameModeRenderDef>();
    #endregion

    #region Variables
    /// <summary>
    /// Stores the location of each ship.
    /// </summary>
    private Dictionary<Id<Ship>, List<Point>> _shipLocations = new();
    /// <summary>
    /// Stores the health of each ship.
    /// Ideally this would be stored on the backend, 
    /// but it'll live in the GameManager.cs since this project is
    /// Client Side only.
    /// </summary>
    private Dictionary<Id<Ship>, int> _shipHealthList = new();
    /// <summary>
    /// Stores the starting ShipDto objects that will
    /// be passed into the GameViewModel instance.
    /// </summary>
    private List<ShipDto> _startingShipDtoList = new();
    private List<Point> _pointMap = new();

    private OverlayView _currentGameOverOverlayView;
    private OverlayViewModel _currentGameOverOverlayViewModel;
    private OverlayView _currentTitleOverlayView;
    private OverlayViewModel _currentTitleOverlayViewModel;
    private GameView _currentGameView;
    private GameViewModel _currentGameViewModel;
    private int _totalAttemptsLeft;
    #endregion

    #region Properties
    public List<Point> PointMap => _pointMap;
    public List<ShipDto> StartingShipDtoList => _startingShipDtoList;
    #endregion

    #region Debug
    private Keyboard _debugKeyboard;
    private GameModeDef _currentGameModeDef;
    private GameModeRenderDef _currentGameModeRenderDef;
    #endregion

    #region Unity Overrides
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.Log("Creating game.");
        // Only default for now
        _currentGameModeDef = GameModeDefList.GetDefById(_selectedGameMode);
        _currentGameModeRenderDef = GetGameModeRenderDef(_selectedGameMode);

        CreatePointMap(_currentGameModeDef);
        ResetGameView();
        CreateTitleOverlayView();

        //!Debug
        _debugKeyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        //! Debug
        if (_debugKeyboard == null)
            return;

        if (_debugKeyboard.spaceKey.wasPressedThisFrame)
        {
            ResetGameView();
        }
    }
    #endregion

    #region Public Functions
    public ShipRenderDef GetShipRenderDefByPoint(Point point)
    {
        // Get the ship location
        var shipLocation = GetShipLocation(point);

        // shipLocation.Key is the ship id. 
        return _currentGameModeRenderDef.GetShipRenderDefById(shipLocation.Key);
    }

    public bool CheckForShipHit(Point point)
    {
        bool didHitShip = false;
        // Decrease the number of attempts left.
        _totalAttemptsLeft = Mathf.Max(0, _totalAttemptsLeft - 1);
        _currentGameViewModel.SetTotalAttemptsLeft(_totalAttemptsLeft);

        // See if there is a ship at the point passed in.
        // [Key: Ship Id, Value: Health]
        var shipLocation = GetShipLocation(point);

        // A ship has been hit!
        if (shipLocation.Key != default && _shipHealthList.ContainsKey(shipLocation.Key))
        {
            _shipHealthList[shipLocation.Key] = Mathf.Max(0, _shipHealthList[shipLocation.Key] - 1);

            //! Debug
            DebugPrintShipHealth();

            ShipDto shipDto = new ShipDto(shipId: shipLocation.Key, health: _shipHealthList[shipLocation.Key]);
            _currentGameViewModel.UpdateShipDto(shipDto);

            didHitShip = true;
        }

        // Check for win condition.
        bool allShipsSunk = _shipHealthList.All(ship => ship.Value == 0);
        if (allShipsSunk)
        {
            UpdateGameOverOverlayView(
                titleTextString: Constants.YouWinString, 
                actionButtonTextString: Constants.PlayAgainString,
                showView: true);
        }

        if (!allShipsSunk && _totalAttemptsLeft <= 0)
        {
            UpdateGameOverOverlayView(titleTextString: Constants.YouLoseString, 
            actionButtonTextString: Constants.PlayAgainString,
            showView: true);
        }
        
        return didHitShip;
    }

    public void ResetGameView()
    {
        if (_currentGameOverOverlayViewModel != null)
        {
            _currentGameOverOverlayViewModel.SetShowOverlayView(false);
        }

        // Reset Game Locations.
        // Use the same game mode def as before.
        _shipLocations.Clear();
        _shipHealthList.Clear();
        _startingShipDtoList.Clear();
        CreateGameView(_currentGameModeDef);
    }
    #endregion

    #region Creation Helpers
    private void CreateGameView(GameModeDef def)
    {
        if (def == null)
        {
            this.LogError("Game Mode def is null.");
            return;
        }
        
        List<ShipDef> shipDefs = ShipDefList.GetAllShipsInGroupById(def.ShipGroup);
        SetShipLocations(shipDefs);
        SetShipHealth(shipDefs);
        SetShipDtoList();

        //! Debug
        DebugPrintShipLocations();
   
        if (_currentGameView == null)
        {
            // I want to only need to have to create the game view model once instead
            // of making it each time.
            _currentGameViewModel = new GameViewModel(this);        
            _currentGameView = Instantiate(_gameViewPrefab, _gameViewParentTransform);
            _currentGameView.SetViewModel(_currentGameViewModel);
        }

        // If the game view already exists, then we need to reset the gameViewModel.
        _currentGameViewModel.Reset(def.Id);

        // Set the number of attempts the player has to win the game.
        _totalAttemptsLeft = def.TotalAttempts;
        _currentGameViewModel.SetTotalAttemptsLeft(_totalAttemptsLeft);

        // Make occupied points a property.
        var occupiedPoints = GetOccupiedPoints();
        _currentGameViewModel.SetOccupiedPointList(occupiedPoints);
        
        // Set up the starting ShipDto list in the Game View Model.
        // the Game View Model will hold a copy of this list
        // that will then be modified as the game progresses.
        _currentGameViewModel.SetShipDtoList(StartingShipDtoList);
    }

    private void CreatePointMap(GameModeDef def)
    {
        if (def == null)
        {
            this.LogError("Game Mode def is null.");
            return;
        }

        int totalRows = def.TotalRows;
        int totalColumns = def.TotalColumns;

        if (totalRows != totalColumns)
        {
            this.LogError("The number of rows must equal the number of columns!");
            return;
        }

        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                Point point = new Point(i,j);
                _pointMap.Add(point);
            }
        }
    }

    private void CreateTitleOverlayView()
    {
        if (_overlayViewPrefab == null)
        {
            this.LogError("_overlayViewPrefab is null. Unable to create overlay view.");
            return;
        }

        _currentTitleOverlayViewModel = new OverlayViewModel(
            titleTextString: "Welcome!",
            actionButtonTextString: "Play",
            showView: true,
            actionButtonCallback: () =>
            {
                _currentTitleOverlayViewModel.SetShowOverlayView(false);
            });
        
        _currentTitleOverlayView = Instantiate(_overlayViewPrefab, _overlayViewParentTransform);
        _currentTitleOverlayView.SetViewModel(_currentTitleOverlayViewModel);
    }

    private void UpdateGameOverOverlayView(string titleTextString, string actionButtonTextString, bool showView)
    {
        if (_overlayViewPrefab == null)
        {
            this.LogError("_overlayViewPrefab is null. Unable to create overlay view.");
            return;
        }

        // Create a new game over overlay view if one hasn't been created yet.
        // This view will persist in the object hierarchy after it is created.
        if (_currentGameOverOverlayView == null)
        {
            _currentGameOverOverlayViewModel = new OverlayViewModel(
                titleTextString: titleTextString,
                actionButtonTextString: actionButtonTextString,
                showView: showView,
                actionButtonCallback: ()=>
                {
                    _currentGameOverOverlayViewModel.SetShowOverlayView(false);
                    ResetGameView();
                });

            _currentGameOverOverlayView = Instantiate(_overlayViewPrefab, _overlayViewParentTransform);
            _currentGameOverOverlayView.SetViewModel(_currentGameOverOverlayViewModel);
        }
        // Otherwise a game overlay view already exists.
        else
        {
            if (!string.IsNullOrEmpty(titleTextString))
            {
                _currentGameOverOverlayViewModel.SetTitleTextString(titleTextString);
            }
        }

        _currentGameOverOverlayViewModel.SetShowOverlayView(showView);
    }
    #endregion

    #region Set Helpers 
    private void SetShipLocations(List<ShipDef> shipDefs)
    {
        if (_pointMap.Count == 0)
        {
            this.LogError("_pointMap.Count is 0. Unable to set ship locations.");
            return;
        }

        System.Random random = new();
        // Make a copy of the point map to modify.
        List<Point> emptyPoints = new List<Point>(_pointMap);
        bool isLocationFound;
        List<Point> unCheckedPoints = new();

        try
        {
            foreach (var ship in shipDefs)
            {
                ResetLocalVariables();

                while (!isLocationFound)
                {
                    // If we've run out of Points to check,
                    // then we are unable to place the ship.
                    if (unCheckedPoints.Count == 0)
                    {
                        isLocationFound = true;
                        throw new Exception($"Unable to find location for ship with id {ship.Id}");
                    }

                    // Since unCheckedPoints is a copy of the emptyPoints list,
                    // we can assume the sourcePoint is already empty.
                    Point sourcePoint = GetSourcePoint(unCheckedPoints);

                    var isValidHorizontal = IsValidHorizontalLocation(sourcePoint, ship.Size);
                    var isValidVertical = IsValidVerticalLocation(sourcePoint, ship.Size);

                    // If neither horizontal or vertical options are valid, continue searching.
                    if (!isValidHorizontal.isValid && !isValidVertical.isValid)
                    {
                        continue;
                    }

                    // If only the horizontal points are valid, assign the ship
                    // to the horizontal points.
                    if (isValidHorizontal.isValid && !isValidVertical.isValid)
                    {
                        _shipLocations.Add(ship.Id, isValidHorizontal.points);
                        isLocationFound = true;
                    }
                    // If only the vertical option is valid, assign the ship
                    // to the vertical points.
                    else if (!isValidHorizontal.isValid && isValidVertical.isValid)
                    {
                        _shipLocations.Add(ship.Id, isValidVertical.points);
                        isLocationFound = true;
                    }
                    // If both options are valid, choose a random one.
                    else if (isValidHorizontal.isValid && isValidVertical.isValid)
                    {
                        int randomInt = random.Next(0, 1);
                        List<Point> points = (randomInt == 0) ? isValidHorizontal.points : isValidVertical.points;
                        
                        _shipLocations.Add(ship.Id, points);
                        isLocationFound = true;
                    }

                    // Remove the Points assigned to the ship from the "emptyPoints" list
                    // since they are now being used by a ship.
                    foreach (Point p in _shipLocations[ship.Id])
                    {
                        emptyPoints.Remove(p);
                    }
                }
            }
        
        }
        catch(Exception e)
        {
            this.LogError($"Exception: {e}");
        }


        // Internal Functions
        (bool isValid, List<Point> points) IsValidHorizontalLocation(Point sourcePoint, int shipSize)
        {
            List<Point> points = new();
            for (int i = 0; i < shipSize; i++)
            {
                // Check if the points to the right of the source point are empty.
                var rightPoint = emptyPoints.FirstOrDefault(p => p.X == sourcePoint.X + i && p.Y == sourcePoint.Y);
                if (!rightPoint.Equals(default))
                {
                    points.Add(rightPoint);
                }
            }
            
            bool isSuccessful = points.Count == shipSize;
            return (isSuccessful, points);
        }

        (bool isValid, List<Point> points) IsValidVerticalLocation(Point sourcePoint, int shipSize)
        {
            List<Point> points = new();
            for (int i = 0; i < shipSize; i++)
            {
                // Check if the points to the bottom of the source point are empty.
                var bottomPoint = emptyPoints.FirstOrDefault(p => p.X == sourcePoint.X && p.Y == sourcePoint.Y + i);
                if (!bottomPoint.Equals(default))
                {
                    points.Add(bottomPoint);
                }
            }
            
            bool isSuccessful = points.Count == shipSize;
            return (isSuccessful, points);
        }

        Point GetSourcePoint(List<Point> unCheckedPoints)
        {
            // Get a random index from the points list we have not checked yet.
            int randomIndex = random.Next(unCheckedPoints.Count);
            // Grab the Point from the list.
            Point sourcePoint = unCheckedPoints[randomIndex];
            // Remove the chosen point from the list since it's
            // going to be checked later in the callstack.
            unCheckedPoints.Remove(sourcePoint);

            return sourcePoint;
        }

        void ResetLocalVariables()
        {
            isLocationFound = false;
            unCheckedPoints.Clear();
            unCheckedPoints.AddRange(emptyPoints);
        }
    }

    private void SetShipHealth(List<ShipDef> shipDefs)
    {
        if (shipDefs == null || shipDefs.Count == 0)
        {
            this.LogError("Unable to set ship health when there are no ships.");
            return;
        }

        foreach (ShipDef def in shipDefs)
        {
            _shipHealthList.Add(def.Id, def.Size);
        }
    }

    private void SetShipDtoList()
    {
        List<ShipDto> shipList = new List<ShipDto>();
        foreach (var ship in _shipHealthList)
        {
            ShipDto shipDto = new ShipDto(ship.Key, ship.Value);
            _startingShipDtoList.Add(shipDto);
        }
    }
    #endregion

    #region Get Helpers
    private List<Point> GetOccupiedPoints()
    {
        List<Point> points = new();

        foreach (var ship in _shipLocations)
        {
            points.AddRange(ship.Value);
        }

        return points;
    }

    private GameModeRenderDef GetGameModeRenderDef(Id<GameMode> modeId)
    {
        if (_gameModeRenderDefs == null || _gameModeRenderDefs.Count == 0)
        {
            this.LogError($"Unable to get GameModeRenderDef! Make sure the GameModeRenderDef list in GameManager isn't empty!");
            return null;
        }

        // If 'None' is passed in, grab whatever use the first element in the list
        // as a fallback.
        if (modeId == GameModeIds.None)
        {
            this.LogWarning($"Game mode id passed in is 'None'. Defaulting to first element in the _gameModeRenderDefs list.");
            return _gameModeRenderDefs[0];
        }

        GameModeRenderDef renderDef = _gameModeRenderDefs.FirstOrDefault(mode => mode.GameModeId == modeId);

        if (renderDef == null)
        {
            this.LogWarning($"Unable to find GameModeRenderDef with id {modeId}. Defaulting to first element in the _gameModeRenderDefs list.");
            return _gameModeRenderDefs[0];
        }

        return renderDef;
    }

    private KeyValuePair<Id<Ship>, List<Point>> GetShipLocation(Point point)
    {
        var shipLocation = _shipLocations.FirstOrDefault((shipLoc =>
        {
            Point p = shipLoc.Value.FirstOrDefault(p => p == point);
            return p != default;
        }));
        
        return shipLocation;
    }
    #endregion
}
