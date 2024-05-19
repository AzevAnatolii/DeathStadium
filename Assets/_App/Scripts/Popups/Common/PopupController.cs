using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

namespace _App.Scripts.Popups
{
	internal static class PopupController
	{
		private static readonly Dictionary<string, HashSet<PopupBase>> _popups = new();

		public static bool TryShowPopup<T>(string name, out T popup, params object[] args) where T : PopupBase
		{
			UIPopup uiPopup = UIPopupManager.ShowPopup(name, false, false);
			if (uiPopup.TryGetComponent(out popup) == false)
			{
				Object.Destroy(uiPopup.gameObject);
				return false;
			}

			popup.SetName(name);
			popup.Init(args);
			RegisterPopup(popup, name);
			return true;
		}

		private static void RegisterPopup(PopupBase popup, string name)
		{
			if (_popups.TryGetValue(name, out HashSet<PopupBase> instances) == false)
			{
				instances = new HashSet<PopupBase>();
				_popups.Add(name, instances);
			}

			instances.Add(popup);
			popup.OnPopupDestroy += OnPopupDestroy;
		}

		private static void OnPopupDestroy(PopupBase popup)
		{
			_popups[popup.Name].Remove(popup);
			popup.OnPopupDestroy -= OnPopupDestroy;
		}
	}
}