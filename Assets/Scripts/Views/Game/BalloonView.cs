using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Views.Game
{
    public class BalloonView : MonoBehaviour
    {
        public event Action<BalloonView> OnEndMoveAction;
        public event Action<BalloonView> OnPressAction;
        public event Action<BalloonView> OnEndAnimAction;

        [SerializeField] 
        private Image _splatterImage;
        [SerializeField]
        private Text _prizeText;
        [SerializeField] 
        private float _speed;

        private bool _isPressed;
        private char _letter;
        private Image _mainImage;
        private Tween _moveTween;

        public char Letter => _letter;

        public void SetSprite(Sprite sprite)
        {
            if (!_mainImage)
            {
                _mainImage = GetComponent<Image>();
            }
            
            _mainImage.sprite = sprite;
        }

        public void SetLetter(char letter)
        {
            _letter = letter;
            _prizeText.text = letter.ToString();
        }

        public void SetSize(float size)
        {
            if (!_mainImage)
            {
                _mainImage = GetComponent<Image>();
            }

            _mainImage.rectTransform.localScale = new Vector2(size, size);
        }

        public void SetSplatter(Sprite sprite)
        {
            _splatterImage.sprite = sprite;
        }

        public void Move()
        {
            _moveTween =
                _mainImage.rectTransform.DOAnchorPosY(_mainImage.rectTransform.anchoredPosition.y + 600, _speed)
                    .SetEase(Ease.Linear)
                    .OnComplete(OnEndMove);
        }

        public void OnPointerDown()
        {
            if (_isPressed)
            {
                return;
            }

            _isPressed = true;

            _moveTween?.Kill();
            
            OnPressAction?.Invoke(this);
            
            ExplodeAnim();
        }

        private void ExplodeAnim()
        {
            Sequence explosionSequence = DOTween.Sequence();
            
            explosionSequence.Append(_mainImage.DOFade(0, 0.1f)
                .SetEase(Ease.OutQuad));
            
            explosionSequence.AppendCallback(() =>
            {
                _mainImage.enabled = false;
                _splatterImage.gameObject.SetActive(true);
                //_splatterImage.TryGetComponent<CanvasGroup>().alpha = 0;
            });
            
            explosionSequence.Append(_splatterImage.DOFade(1, 0.3f)
                .SetEase(Ease.OutQuad));
            
            explosionSequence.Append(_splatterImage.DOFade(0, 1.0f)
                .SetEase(Ease.InQuad));
            
            explosionSequence.OnComplete(() =>
            {
                _prizeText.enabled = false;
                OnEndAnimAction?.Invoke(this);
            });
        }
        

        private void OnEndMove()
        {
            OnEndMoveAction?.Invoke(this);
        }
    }
}