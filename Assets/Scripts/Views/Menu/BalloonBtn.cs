using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Views.Menu
{
    public class BalloonBtn : MonoBehaviour
    {
        public event Action OnPressBtnAction;
        public event Action OnAnimEndedAction;
        
        [SerializeField] 
        private Button _btn;
        [SerializeField] 
        private RectTransform _balloonRect;

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnPressBtn);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetActive(bool value)
        {
            _btn.interactable = value;
        }

        private void StartAnim()
        {
            _balloonRect.DOAnchorPosY(500, 1).OnComplete(OnAnimEnded);
        }

        private void OnPressBtn()
        {
            OnPressBtnAction?.Invoke();
            
            StartAnim();
        }

        private void OnAnimEnded()
        {
            OnAnimEndedAction?.Invoke();
        }
    }
}