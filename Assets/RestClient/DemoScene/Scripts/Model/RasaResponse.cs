using System;

namespace Models
{
	[Serializable]
	public class RasaResponse
	{
		public string recipientId;

		public string text;

		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}
}

