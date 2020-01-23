using System;

namespace Models
{
	[Serializable]
	public class RasaMessage
	{
		public string sender;

		public string message;

		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}
}

