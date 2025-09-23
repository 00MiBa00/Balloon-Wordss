using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Views.Game
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] 
        private List<Image> _healthImages;

        public void SetHealth(int value)
        {
            for (int i = 0; i < _healthImages.Count; i++)
            {
                bool sameAlfa = Mathf.Approximately(_healthImages[i].color.a, 0.2f);
                
                if (i >= value && !sameAlfa)
                {
                    _healthImages[i].DOFade(0.2f, 0.3f);
                }
            }
        }
    }
}