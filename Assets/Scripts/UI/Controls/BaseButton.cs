using System;
using UnityEngine;

namespace Card
{
    
    public class BaseButton : MonoBehaviour
    {
        
        public event Action Click;

        
        public event Action ClickOnce;

       
        protected virtual void OnMouseUpAsButton()
        {
            ClickOnce?.Invoke();
            ClickOnce = null;

            Click?.Invoke(); 
        }
        
        public void SetVisible(bool isVisible) 
        {
            gameObject.SetActive(isVisible);
        }
    }
}