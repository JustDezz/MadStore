using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
	private string GoogleID = "4240411";
	private string rewardedPlacementID = "Rewarded_Android";
#elif UNITY_IOS
	private string GoogleID = "4240410";
	private string rewardedPlacementID = "Rewarded_iOS";
#endif
	private Action OnRewardedVideoSucceed;
	private bool videoReTried = false;

	private IEnumerator Start()
	{
		Advertisement.Initialize(GoogleID);
		Advertisement.AddListener(this);
		yield return null;
	}
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
