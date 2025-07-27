using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{
    public static event Action<Card> OnCardSelected;

    private static Image _defaultIconImage;

    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private Outline _outline;
    [SerializeField]
    private Transform _projectTails;

    private Sprite _defaultIcon;
    private Sprite _iconSprite;

    public int Id { get; private set; }
    public bool IsSelected { get; private set; }

    public void InitializeCard(Sprite defaultIcon, Sprite iconSprite, int id)
    {
        Id = id;
        _defaultIcon = defaultIcon;
        _iconSprite = iconSprite;

        SetIconSprite(_defaultIcon);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            Select();
        }
    }
    private void SetIconSprite(Sprite sprite) => _iconImage.sprite = sprite;
    public void Select()
    {
        IsSelected = true;
        _outline.effectColor = Color.green;
        SetIconSprite(_iconSprite);
        OnCardSelected?.Invoke(this);
    }
    public async void DeSelect()
    {
        IsSelected = false;
        await Task.Delay(300);
        SetIconSprite(_defaultIcon);
        _outline.effectColor = Color.black;
    }


}
