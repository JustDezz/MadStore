using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
	private string appID = "4240411";
	private string rewardedPlacementID = "Rewarded_Android";
#elif UNITY_IOS
	private string appID = "4240410";
	private string rewardedPlacementID = "Rewarded_iOS";
#else 
	private string appID = "";
	private string rewardedPlacementID = "";
#endif
	private Action OnRewardedVideoSucceed;
	private bool videoReTried = false;

#if UNITY_ANDROID || UNITY_IOS
	private IEnumerator Start()
	{
		Advertisement.Initialize(appID);
		Advertisement.AddListener(this);
		yield return null;
	}
#endif
	public void ShowRewardedAd(Action onAdSucceed)
	{
		OnRewardedVideoSucceed = onAdSucceed;
		if (Advertisement.IsReady(rewardedPlacementID))
		{
			Advertisement.Show(rewardedPlacementID);
		}
		else
		{
			if (!videoReTried)
			{
				videoReTried = true;
				StartCoroutine(TryRepeatRewardedAd(onAdSucceed));
			}
			else
			{
				videoReTried = false;
			}
		}
	}
	private IEnumerator TryRepeatRewardedAd(Action onAdSucceed)
	{
		yield return new WaitForSecondsRealtime(3f);
		ShowRewardedAd(onAdSucceed);
	}
	void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
	{
		if (placementId == rewardedPlacementID && showResult == ShowResult.Finished)
		{
			OnRewardedVideoSucceed?.Invoke();
		}
		Time.timeScale = 1;
	}
	void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
	{
		Time.timeScale = 0;
	}
	void IUnityAdsListener.OnUnityAdsReady(string placementId)
	{
	}
	void IUnityAdsListener.OnUnityAdsDidError(string message)
	{
		Time.timeScale = 1;
	}
}
