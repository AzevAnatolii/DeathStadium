using System;
using UnityEngine;

namespace _App.Scripts.Popups
{
	internal abstract class PopupBase : MonoBehaviour
	{
		public string Name { get; private set; }
		public event Action<PopupBase> OnPopupDestroy;
		
		private void OnDestroy()
		{
			OnPopupDestroy?.Invoke(this);
		}

		public virtual void Init(params object[] args) { }

		public void SetName(string popupName)
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				Name = popupName;
			}
		} 
	}
}