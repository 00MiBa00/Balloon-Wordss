using System.Collections;
using UnityEngine;
using Controllers.SurvivalGame;

using Views.Game;
using Views.General;
using Views.Game.Sprint;

using Models.Scenes;
using Types;
using AudioType = Types.AudioType;

namespace Controllers.Scenes
{
    public class SprintGameSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Controllers")]
        [SerializeField] private SpawnController _spawnController;

        [Space(5)] [Header("Views")] 
        [SerializeField] private WordDisplay _wordDisplay;
        [SerializeField] private TimerUpdater _timerUpdater;

        [Space(5)] [Header("Panels")] 
        [SerializeField] private StartPanel _startPanel;
        [SerializeField] private PanelView _mainPanel;
        [SerializeField] private PanelView _pausePanel;
        [SerializeField] private ResultPanel _resultPanel;

        private SprintGameSceneModel _model;
        
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
            _model = new SprintGameSceneModel();
            
            _spawnController.Initialize();
        }

        protected override void Subscribe()
        {
            _spawnController.OnPressBalloonAction += OnPressBalloon;
        }

        protected override void Unsubscribe()
        {
            _spawnController.OnPressBalloonAction -= OnPressBalloon;
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
            
            SetEndGame(true);
        }

        private void SetActivePanel(GameObject go, bool active)
        {
            go.SetActive(active);
        }

        private void OpenResultPanel()
        {
            _resultPanel.SetLevelText(_model.HaveTime ? _model.LevelIndex -1 :_model.LevelIndex);
            _resultPanel.SetDescription(_model.HaveTime);
            _resultPanel.SetTimerState(_model.HaveTime, _model.SecLeft);
            _resultPanel.SetNextOrRestartBtnState(_model.HaveTime);
            _resultPanel.PressBtnAction += OnReceiveAnswerResultPanel;
            SetActivePanel(_resultPanel.gameObject, true);
        }

        private void OpenStartPanel()
        {
            _startPanel.SetLevelText(_model.LevelIndex);
            _startPanel.SetWordText(_model.DisplayText);
            _startPanel.SetTime(_model.SecLeft);
            
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
                    base.SetClickClip();
                    StartCoroutine(DelayContinueGame());
                    SetActivePanel(_pausePanel.gameObject, false);
                    break;
                case 1:
                    base.LoadScene(SceneType.SprintGame);
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
                base.LoadScene(SceneType.SprintGame);
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
                StartGame();
            }
        }

        private void OnReceiveAnswerMainPanel(int answer)
        {
            _mainPanel.PressBtnAction -= OnReceiveAnswerMainPanel;
            
            base.SetClickClip();

            Time.timeScale = 0;
            
            OpenPausePanel();
        }

        private void SetEndGame(bool isWin)
        {
            _spawnController.EndGame();

            AudioType type = isWin ? AudioType.WinClip : AudioType.LoseClip;
            
            base.PlaySound(type);
            
            SetActivePanel(_mainPanel.gameObject, false);
            
            if (isWin)
            {
                StopAllCoroutines();
                _model.UpdateLevel();
            }
            
            OpenResultPanel();
        }

        private void StartGame()
        {
            UpdateWord();
            OpenMainPanel();
            _spawnController.StartGame();

            StartCoroutine(StartTimer());
        }

        private IEnumerator DelayContinueGame()
        {
            yield return new WaitForSecondsRealtime(1);

            Time.timeScale = 1;
        }

        private IEnumerator StartTimer()
        {
            while (_model.HaveTime)
            {
                _timerUpdater.UpdateTime(_model.SecLeft);
                
                _model.SubtractTime();

                yield return new WaitForSeconds(1);
            }
            
            SetEndGame(false);
        }
    }
}