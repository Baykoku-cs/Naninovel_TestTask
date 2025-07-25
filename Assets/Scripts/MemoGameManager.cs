using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class MemoGameManager : MonoBehaviour
{
    public event EventHandler<OnGameStartedArgs> OnGameStarted;
    public event EventHandler OnMatch;
    public class OnGameStartedArgs : EventArgs
    {
        public int pairsCount;
    }

    public static MemoGameManager Instance { get; private set; }

    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private List<Sprite> _inGameCards;
    [SerializeField]
    private Sprite _defaultIcon;
    [SerializeField]
    private Transform _cardPrefab;
    [SerializeField]
    private Transform _allCardsParent;

    private Card _selectedCard;
    private int _pairsCount;
    private bool _isGameActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("MemoGameCamera instance already exists.");
        }

        Instance = this;
    }
    private void Start()
    {
        Card.OnCardSelected += Card_OnCardSelected;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isGameActive)
        {
            EndGame();
        }
        if (Input.GetKeyDown(KeyCode.S) && !_isGameActive)
        {
            Debug.Log("Game started");
            StartGame();
        }
    }

    public void ToggleCamera(bool isEnabled)
    {
        _camera.enabled = isEnabled;

        if (isEnabled)
        {
            StartGame();
        }
    }

    private void Card_OnCardSelected(Card selectedCard)
    {
        Debug.Log("CardSelected");
        if (_selectedCard == null)
        {
            Debug.Log("first select");

            _selectedCard = selectedCard;
            return;
        }
        else if (_selectedCard.Id == selectedCard.Id)
        {
            Debug.Log("Match");
            OnMatch?.Invoke(this, EventArgs.Empty);
            _pairsCount--;

            if (_pairsCount == 0)
            {
                Debug.Log("All pairs matched!");
                EndGame();
                return;
            }

            _selectedCard = null;
        }
        else
        {
            Debug.Log("Not a match");

            _selectedCard.DeSelect();
            selectedCard.DeSelect();

            _selectedCard = null;
        }
    }

    private void StartGame()
    {
        _isGameActive = true;
        foreach (Transform child in _allCardsParent)
        {
            Destroy(child.gameObject);
        }

        if (_inGameCards == null || _inGameCards.Count == 0)
        {
            Debug.LogError("No cards available to start the game.");
            return;
        }

        _pairsCount = _inGameCards.Count;

        // Shuffle
        List<int> ShuffledCards = new List<int>();
        var cardIds = Enumerable.Range(0, _inGameCards.Count).Concat(Enumerable.Range(0, _inGameCards.Count)).ToList();
        while (cardIds.Count() > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardIds.Count);
            ShuffledCards.Add(cardIds[randomIndex]);
            cardIds.RemoveAt(randomIndex);
        }

        foreach (var cardIndex in ShuffledCards)
        {
            Card card = Instantiate(_cardPrefab, _allCardsParent).GetComponent<Card>();
            card.InitializeCard(
                _defaultIcon,
                _inGameCards[cardIndex],
                cardIndex
            );
        }

        OnGameStarted?.Invoke(this, new OnGameStartedArgs() { pairsCount = _pairsCount }); 
    }

    private void EndGame()
    {
        _isGameActive = false;
        SwitchToNovel();
    }

    private void SwitchToNovel()
    {
        // 1. Enable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = true;

        Engine.GetService<UIManager>().GetUI<ContinueInputUI>().Show();

        // 2. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.PreloadAndPlayAsync("Forest", "AfterGame");

        // 4. Swap Cameras.
        ToggleCamera(false);
        Engine.GetService<ICameraManager>().Camera.enabled = true;
    }
}
