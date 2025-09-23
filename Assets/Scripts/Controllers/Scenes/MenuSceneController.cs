using UnityEngine;
using Views.Menu;
using Types;
using UnityEngine.UI;

namespace Controllers.Scenes
{
    public class MenuSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Buttons")] 
        [SerializeField] private BalloonBtn _survivalGameBtn;
        [SerializeField] private BalloonBtn _classicGameBtn;
        [SerializeField] private BalloonBtn _sprintGameBtn;
        [SerializeField] private Button _settingsBtn;
        
        protected override void OnSceneEnable()
        {
            
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            
        }

        protected override void Subscribe()
        {
            _survivalGameBtn.OnPressBtnAction += OnPressBtn;
            _survivalGameBtn.OnAnimEndedAction += OnSurvivalEndedAnim;

            _classicGameBtn.OnPressBtnAction += OnPressBtn;
            _classicGameBtn.OnAnimEndedAction += OnClassicEndedAnim;

            _sprintGameBtn.OnPressBtnAction += OnPressBtn;
            _sprintGameBtn.OnAnimEndedAction += OnSprintEndedAnim;
            
            _settingsBtn.onClick.AddListener(OnPressSettingsBtn);
        }

        protected override void Unsubscribe()
        {
            _survivalGameBtn.OnPressBtnAction -= OnPressBtn;
            _survivalGameBtn.OnAnimEndedAction -= OnSurvivalEndedAnim;

            _classicGameBtn.OnPressBtnAction -= OnPressBtn;
            _classicGameBtn.OnAnimEndedAction -= OnClassicEndedAnim;

            _sprintGameBtn.OnPressBtnAction -= OnPressBtn;
            _sprintGameBtn.OnAnimEndedAction -= OnSprintEndedAnim;
            
            _settingsBtn.onClick.RemoveAllListeners();
        }

        private void OnPressBtn()
        {
            _survivalGameBtn.SetActive(false);
            _classicGameBtn.SetActive(false);
            _sprintGameBtn.SetActive(false);
            _settingsBtn.interactable = false;
        }

        private void OnSurvivalEndedAnim()
        {
            base.LoadScene(SceneType.SurvivalGame);
        }

        private void OnClassicEndedAnim()
        {
            base.LoadScene(SceneType.ClassicGame);
        }

        private void OnSprintEndedAnim()
        {
            base.LoadScene(SceneType.SprintGame);
        }

        private void OnPressSettingsBtn()
        {
            base.LoadScene(SceneType.Settings);
        }
    }
}