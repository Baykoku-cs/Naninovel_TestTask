using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Image _hpBarImage;

    private int _maxHealth;
    private int _health;
    private float _wishedFillAmount;
    private float _transitionDuration = 3f;
    private float _transitionTimer;

    public static Thug Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Thug instance already exists.");
        }

        Instance = this;
    }
    private void Start()
    {
        MemoGameManager.Instance.OnGameStarted += MemoGameManager_OnGameStarted;
        MemoGameManager.Instance.OnMatch += MemoGameManager_OnMatch; ;
    }

    private void MemoGameManager_OnMatch(object sender, System.EventArgs e)
    {
        TakeDamage();
    }

    private void MemoGameManager_OnGameStarted(object sender, MemoGameManager.OnGameStartedArgs e)
    {
        Debug.Log(e.pairsCount);
        _maxHealth = e.pairsCount;
        _health = e.pairsCount;
        _wishedFillAmount = _health / _maxHealth;
    }


    private void Update()
    {
        if (_hpBarImage.fillAmount != _wishedFillAmount)
        {
            _hpBarImage.fillAmount = Mathf.Lerp(_hpBarImage.fillAmount, _wishedFillAmount, _transitionTimer / _transitionDuration);
            if (_transitionTimer < _transitionDuration)
            {
                _transitionTimer += Time.deltaTime;
            }
        }
    }
    private void TakeDamage()
    {
        _transitionTimer = 0f;
        _health -= 1;
        _wishedFillAmount = (float)_health / _maxHealth;
    }

}
