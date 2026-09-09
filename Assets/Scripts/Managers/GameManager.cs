using System.Collections.Generic;
using System.ComponentModel.Design;
using System;
using UnityEngine;
using System.Linq;
using System.Text;

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
    private Dictionary<Id<Ship>, List<Point>> _shipLocations = new();
    private List<Point> _pointMap = new List<Point>();
    private GameView _currentGameView;
    #endregion

    #region Properties
    public List<Point> PointMap => _pointMap;
    #endregion

    #region Unity Overrides
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.Log("Creating game.");
        CreateGameView();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    // void OnDestroy()
    // {
        
    // }
    #endregion
#region Public Functions
    public Color DebugGetShipColorByPoint(Point point)
    {
        var shipLocation = _shipLocations.FirstOrDefault((shipLoc =>
        {
            Point p = shipLoc.Value.FirstOrDefault(p => p == point);
            return p != default;
        }));

       return DebugGetShipColorById(shipLocation.Key);
    }

#endregion
#region Private Functions
    private Color DebugGetShipColorById(Id<Ship> shipId)
    {
        var colorRGB = ShipDefList.GetDefById(shipId).DebugColor;
        Color color = new Color(colorRGB.r, colorRGB.g, colorRGB.b);
        return color;
    }

    private void CreateGameView()
    {
        // Only default for now
        GameModeDef def = GameModeDefList.GetDefById(GameModeIds.Default);

        if (def == null)
        {
            this.LogError("Game Mode def is null.");
        }

        CreatePointMap(totalRows: def.TotalRows, totalColumns: def.TotalColumns);
        DetermineShipLocations(def);

        DebugPrintShipLocations();

        GameViewModel gameViewModel = new GameViewModel(def.Id, GetOccupiedPoints(), this);        
        if (_currentGameView == null)
        {
            _currentGameView = Instantiate(_gameViewPrefab, _gameViewParentTransform);
        }

        _currentGameView.SetViewModel(gameViewModel);
    }

    private void CreatePointMap(int totalRows, int totalColumns)
    {
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

    private void DetermineShipLocations(GameModeDef def)
    {
        this.Log("DetermineShipLocations | start");

        if (_pointMap.Count == 0)
        {
            this.LogError("_pointMap.Count is 0. Unable to determine ship locations.");
            return;
        }

        System.Random random = new();
        List<ShipDef> shipDefs = ShipDefList.GetAllShipsInGroupById(def.ShipGroup);
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

    private List<Point> GetOccupiedPoints()
    {
        List<Point> points = new();

        foreach (var ship in _shipLocations)
        {
            points.AddRange(ship.Value);
        }

        return points;
    }

    private void DebugPrintShipLocations()
    {
        StringBuilder sb = new StringBuilder();
        Color color = default;
        string htmlColor = default;
        
        foreach(var shipLoc in _shipLocations)
        {
            color = DebugGetShipColorById(shipLoc.Key);
            htmlColor = $"#{ColorUtility.ToHtmlStringRGBA(color)}";

            sb.Append($"{shipLoc.Key} | <color={htmlColor}>Color</color> | Points: [");

            foreach(Point point in shipLoc.Value)
            {
                sb.Append($" {point} ");
            }
            sb.Append($"]");
            sb.AppendLine();
        }
        this.Log(sb.ToString());
    }
    #endregion
}
