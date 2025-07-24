using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _inGameCards;
    [SerializeField]
    private Sprite _defaultIcon;
    [SerializeField]
    private Transform _cardPrefab;

    private Card _selectedCard;
    private int _pairsCount;

    private void Start()
    {
        Card.OnCardSelected += Card_OnCardSelected;
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

            _pairsCount--;

            if (_pairsCount == 0)
            {
                Debug.Log("All pairs matched!");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        foreach (Transform child in transform)
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
        List<int> ShuffeldCards = new List<int>();
        var cardIds = Enumerable.Range(0, _inGameCards.Count).Concat(Enumerable.Range(0, _inGameCards.Count)).ToList();
        while (cardIds.Count() > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardIds.Count);
            ShuffeldCards.Add(cardIds[randomIndex]);
            cardIds.RemoveAt(randomIndex);
        }

        foreach (var cardIndex in ShuffeldCards)
        {
            Card card = Instantiate(_cardPrefab, transform).GetComponent<Card>();
            card.InitializeCard(
                _defaultIcon,
                _inGameCards[cardIndex],
                cardIndex
            );
        }
    }

}
