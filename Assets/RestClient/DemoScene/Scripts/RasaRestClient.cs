using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RasaRestClient : MonoBehaviour {

	private readonly string basePath = "http://localhost:15008";
	private RequestHelper currentRequest;

	private void LogMessage(string title, string message) {
#if UNITY_EDITOR
		EditorUtility.DisplayDialog (title, message, "Ok");
#else
		Debug.Log(message);
#endif
	}
	
	public void Greet(){

		RestClient.DefaultRequestParams["lang"] = "en";

		currentRequest = new RequestHelper {
			Uri = basePath + "/webhooks/rest/webhook",
			Params = new Dictionary<string, string> {
				{ "param1", "value 1" },
				{ "param2", "value 2" }
			},
			Body = new RasaMessage {
				sender = "samlet",
				message = "hello"
			},
			EnableDebug = true
		};
		RestClient.PostArray<RasaResponse>(currentRequest)
			.Then(res => {

				// And later we can clear the default query string params for all requests
				RestClient.ClearDefaultParams();

				// this.LogMessage("Success", JsonUtility.ToJson(res, true));
				this.LogMessage("Success", JsonHelper.ArrayToJsonString<RasaResponse>(res, true));
			})
			.Catch(err => this.LogMessage("Error", err.Message));
	}


	public void AbortRequest(){
		if (currentRequest != null) {
			currentRequest.Abort();
			currentRequest = null;
		}
	}

	public void DownloadFile(){

		var fileUrl = "https://raw.githubusercontent.com/IonDen/ion.sound/master/sounds/bell_ring.ogg";
		var fileType = AudioType.OGGVORBIS;

		RestClient.Get(new RequestHelper {
			Uri = fileUrl,
			DownloadHandler = new DownloadHandlerAudioClip(fileUrl, fileType)
		}).Then(res => {
			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = ((DownloadHandlerAudioClip)res.Request.downloadHandler).audioClip;
			audio.Play();
		}).Catch(err => {
			this.LogMessage("Error", err.Message);
		});
	}
}