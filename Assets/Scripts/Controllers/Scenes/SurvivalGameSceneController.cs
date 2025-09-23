using System.Collections;
using UnityEngine;
using Controllers.SurvivalGame;

using Views.Game;
using Views.General;
using Views.Game.Survival;

using Models.Scenes;
using Types;
using AudioType = Types.AudioType;

namespace Controllers.Scenes
{
    public class SurvivalGameSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Controllers")]
        [SerializeField] private SpawnController _spawnController;

        [Space(5)] [Header("Views")] 
        [SerializeField] private WordDisplay _wordDisplay;
        [SerializeField] private HealthView _healthView;

        [Space(5)] [Header("Panels")] 
        [SerializeField] private StartPanel _startPanel;
        [SerializeField] private PanelView _mainPanel;
        [SerializeField] private PanelView _pausePanel;
        [SerializeField] private ResultPanel _resultPanel;

        private SurvivalGameSceneModel _model;
        
        protected override void OnSceneEnable()
        {
            OpenStartPanel();
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            _model = new SurvivalGameSceneModel();
            
            _spawnController.Initialize();
        }

        protected override void Subscribe()
        {
            _spawnController.OnPressBalloonAction += OnPressBalloon;
            _spawnController.OnMissedBalloonAction += OnMissedBalloon;
        }

        protected override void Unsubscribe()
        {
            _spawnController.OnPressBalloonAction -= OnPressBalloon;
            _spawnController.OnMissedBalloonAction -= OnMissedBalloon;
        }

        private void UpdateDisplayWord()
        {
            _wordDisplay.UpdateWord(_model.DisplayText);
        }

        private void UpdateWord()
        {
            UpdateDisplayWord();
            _spawnController.SetWord(_model.CurrentWord);
        }

        private void OnPressBalloon(char letter)
        {
            base.PlaySound(AudioType.BallonClip);
            
            if (!_model.TryChangeLetterState(letter))
            {
                return;
            }
            
            UpdateDisplayWord();

            if (!_model.IsEndRound())
            {
                return;
            }
            
            _model.SetNewWord();
            UpdateWord();
        }

        private void OnMissedBalloon()
        {
            _model.SubtractHealth();

            _healthView.SetHealth(_model.HealthCount);

            if (_model.CanContinueGame)
            {
                return;
            }
            
            OnEndGame();
        }

        private void OnEndGame()
        {
            _spawnController.EndGame();
            
            SetActivePanel(_mainPanel.gameObject, false);
            OpenResultPanel();
        }

        private void SetActivePanel(GameObject go, bool active)
        {
            go.SetActive(active);
        }

        private void OpenResultPanel()
        {
            AudioType type = _model.IsNewBest ? AudioType.WinClip : AudioType.LoseClip;
            
            base.PlaySound(type);
            
            _resultPanel.SetDescription(_model.IsNewBest);
            _resultPanel.SetBestWords(_model.BestWords);
            _resultPanel.SetCurrentWords(_model.CurrentWords);
            _resultPanel.PressBtnAction += OnReceiveAnswerResultPanel;
            SetActivePanel(_resultPanel.gameObject, true);
        }

        private void OpenStartPanel()
        {
            _startPanel.SetBestWords(_model.BestWords);
            _startPanel.PressBtnAction += OnReceiveAnswerStartPanel;
            
            SetActivePanel(_startPanel.gameObject, true);
        }

        private void OpenMainPanel()
        {
            _mainPanel.PressBtnAction += OnReceiveAnswerMainPanel;
            SetActivePanel(_mainPanel.gameObject, true);
        }

        private void OpenPausePanel()
        {
            _pausePanel.PressBtnAction += OnReceiveAnswerPausePanel;
            SetActivePanel(_pausePanel.gameObject, true);
        }

        private void OnReceiveAnswerPausePanel(int answer)
        {
            _pausePanel.PressBtnAction -= OnReceiveAnswerPausePanel;

            switch (answer)
            {
                case 0:
                    StartCoroutine(DelayContinueGame());
                    SetActivePanel(_pausePanel.gameObject, false);
                    break;
                case 1:
                    base.LoadScene(SceneType.SurvivalGame);
                    break;
                case 2:
                    base.LoadScene(SceneType.Menu);
                    break;
            }
        }

        private void OnReceiveAnswerResultPanel(int answer)
        {
            _resultPanel.PressBtnAction -= OnReceiveAnswerResultPanel;

            if (answer == 0)
            {
                base.LoadScene(SceneType.Menu);
            }
            else
            {
                base.LoadScene(SceneType.SurvivalGame);
            }
        }

        private void OnReceiveAnswerStartPanel(int answer)
        {
            _startPanel.PressBtnAction -= OnReceiveAnswerStartPanel;
            
            SetActivePanel(_startPanel.gameObject, false);

            if (answer == 0)
            {
                base.LoadScene(SceneType.Menu);
            }
            else
            {
                base.SetClickClip();
                UpdateWord();
                OpenMainPanel();
                _spawnController.StartGame();
            }
        }

        private void OnReceiveAnswerMainPanel(int answer)
        {
            _mainPanel.PressBtnAction -= OnReceiveAnswerMainPanel;
            
            base.SetClickClip();

            Time.timeScale = 0;
            
            OpenPausePanel();
        }

        private IEnumerator DelayContinueGame()
        {
            yield return new WaitForSecondsRealtime(1);

            Time.timeScale = 1;
        }
    }
}