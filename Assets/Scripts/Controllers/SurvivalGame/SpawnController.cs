using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Models.SurvivalGame;
using Views.Game;

namespace Controllers.SurvivalGame
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _balloonPrefab;
        [SerializeField] 
        private List<Sprite> _balloonSprites;
        [SerializeField] 
        private List<Sprite> _splatterSprites;
        [SerializeField] 
        private List<RectTransform> _startPositions;

        private List<BalloonView> _balloonViews;
        private SurvivalSpawnModel _model;

        public event Action<char> OnPressBalloonAction;
        public event Action OnMissedBalloonAction;

        public void Initialize()
        {
            _balloonViews = new List<BalloonView>();
            
            _model = new SurvivalSpawnModel(_balloonSprites.Count, _startPositions.Count);
        }

        public void SetWord(string word)
        {
            _model.SetWord(word);
        }

        public void StartGame()
        {
            _model.CanSpawn = true;

            StartCoroutine(StartSpawn());
        }

        public void EndGame()
        {
            _model.CanSpawn = false;
            StopAllCoroutines();

            foreach (var view in _balloonViews)
            {
                if (view != null)
                {
                    DestroyBalloon(view);
                }
            }
        }

        private void SpawnBalloon()
        {
            GameObject balloon = Instantiate(_balloonPrefab, transform);

            BalloonView balloonView = balloon.GetComponent<BalloonView>();

            balloonView.OnEndAnimAction += DestroyBalloon;
            balloonView.OnEndMoveAction += OnMissedBalloon;
            balloonView.OnPressAction += OnPressBalloon;
            
            balloonView.SetSize(_model.GetRandomSize());
            balloonView.SetLetter(_model.GetLetter());

            int skinIndex = _model.GetBalloonSpriteIndex();
            
            balloonView.SetSprite(_balloonSprites[skinIndex]);
            balloonView.SetSplatter(_splatterSprites[skinIndex]);

            RectTransform balloonRect = balloon.GetComponent<RectTransform>();

            balloonRect.anchoredPosition = _startPositions[_model.GetBalloonPositionIndex()].anchoredPosition;
            
            _balloonViews.Add(balloonView);
            
            balloonView.Move();
        }

        private void OnPressBalloon(BalloonView view)
        {
            OnPressBalloonAction?.Invoke(view.Letter);
        }

        private void OnMissedBalloon(BalloonView view)
        {
            OnMissedBalloonAction?.Invoke();
            
            DestroyBalloon(view);
        }

        private void DestroyBalloon(BalloonView view)
        {
            view.OnPressAction -= OnPressBalloon;
            view.OnEndMoveAction += OnMissedBalloon;
            view.OnEndAnimAction -= DestroyBalloon;

            int index = _balloonViews.IndexOf(view);
            
            Destroy(_balloonViews[index].gameObject);
        }

        private IEnumerator StartSpawn()
        {
            while (_model.CanSpawn)
            {
                SpawnBalloon();

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}