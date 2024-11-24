using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    #region Fields

    [SerializeField] private PlayerHandler _playerHandler;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TMP_Text _playerNameText;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        InitializeUI();
        BindEvents();
    }

    private void OnDestroy()
    {
        UnbindEvents();        
    }

    #endregion

    #region Private Methods

    private void InitializeUI()
    {
        _playerNameText.text = _playerHandler.PlayerName;
        _playerNameText.colorGradient = new VertexGradient(Color.white, Color.white, _playerHandler.PlayerColor, _playerHandler.PlayerColor);
        _hpSlider.fillRect.GetComponent<Image>().color = _playerHandler.PlayerColor;
    }

    private void BindEvents()
    {
        _playerHandler.Health.SubcribeToHealthChange(OnHealthChanged);
    }

    private void UnbindEvents()
    {
        _playerHandler.Health.UnsubcribeToHealthChange(OnHealthChanged);
    }

    private void OnHealthChanged(float lastValue, float newValue)
    {
        var ratio = newValue / _playerHandler.Health.MaxHealth;
        _hpSlider.value = ratio;
    }



    #endregion
}
