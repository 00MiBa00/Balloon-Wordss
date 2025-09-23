using System.Collections;
using UnityEngine;
using Controllers.SurvivalGame;

using Views.Game;
using Views.Game.Classic;
using Views.General;

using Models.Scenes;
using Types;
using AudioType = Types.AudioType;

namespace Controllers.Scenes
{
    public class ClassicGameSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Controllers")]
        [SerializeField] private SpawnController _spawnController;

        [Space(5)] [Header("Views")] 
        [SerializeField] private WordDisplay _wordDisplay;

        [Space(5)] [Header("Panels")] 
        [SerializeField] private StartPanel _startPanel;
        [SerializeField] private PanelView _mainPanel;
        [SerializeField] private PanelView _pausePanel;
        [SerializeField] private ResultPanel _resultPanel;

        private ClassicGameSceneModel _model;
        
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
            _model = new ClassicGameSceneModel();
            
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
            
            _spawnController.EndGame();
            _model.UpdateLevel();
            
            SetActivePanel(_mainPanel.gameObject, false);
            
            OpenResultPanel();
        }

        private void SetActivePanel(GameObject go, bool active)
        {
            go.SetActive(active);
        }

        private void OpenResultPanel()
        {
            base.PlaySound(AudioType.WinClip);
            
            _resultPanel.SetLevelText(_model.LevelIndex-1);
            _resultPanel.PressBtnAction += OnReceiveAnswerResultPanel;
            SetActivePanel(_resultPanel.gameObject, true);
        }

        private void OpenStartPanel()
        {
            _startPanel.SetLevelText(_model.LevelIndex);
            _startPanel.SetWordText(_model.DisplayText);
            
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
            
            base.SetClickClip();

            switch (answer)
            {
                case 0:
                    StartCoroutine(DelayContinueGame());
                    SetActivePanel(_pausePanel.gameObject, false);
                    break;
                case 1:
                    base.LoadScene(SceneType.ClassicGame);
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
                base.LoadScene(SceneType.ClassicGame);
            }
        }

        private void OnReceiveAnswerStartPanel(int answer)
        {
            _startPanel.PressBtnAction -= OnReceiveAnswerStartPanel;
            
            base.SetClickClip();
            
            SetActivePanel(_startPanel.gameObject, false);

            if (answer == 0)
            {
                base.LoadScene(SceneType.Menu);
            }
            else
            {
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