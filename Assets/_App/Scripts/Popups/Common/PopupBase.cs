using System;
using Doozy.Engine.UI;
using UnityEngine;

namespace _App.Scripts.Popups
{
	internal abstract class PopupBase : MonoBehaviour
	{
		private bool _isInitialized;
		private UIPopup _popup;
		
		public string Name { get; private set; }
		public event Action<PopupBase> OnPopupDestroy;

		private void OnDestroy()
		{
			OnDestroyInternal();
			OnPopupDestroy?.Invoke(this);
		}

		public void Init(string popupName, UIPopup uiPopup, params object[] args)
		{
			if (!_isInitialized)
			{
				Name = popupName;
				_popup = uiPopup;
				InitInternal(args);
				_isInitialized = true;
			}
		}

		public void Hide()
		{
			_popup.Hide();
		}

		protected virtual void InitInternal(object[] args) { }
		
		protected virtual void OnDestroyInternal() { }
	}
}