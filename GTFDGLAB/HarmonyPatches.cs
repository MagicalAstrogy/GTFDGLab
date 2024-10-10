
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Player;
using UnityEngine;
using HttpClient = System.Net.Http.HttpClient;
using HttpContent = System.Net.Http.HttpContent;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
using Logger = MagicalAstrogy.GTFDGLAB.Logger;

namespace MagicalAstrogy.GTFDGLAB
{
    public static class DGLabHttpRequest
    {
        private static HttpClient _client = new HttpClient();
        private static Task<HttpResponseMessage> _currentTask;
        public static async void Fire(float strength, float duration)
        {
            var url = $"{ConfigManager.BaseUrl}api/game/{ConfigManager.ClientId}/fire";
            var jsonContent = $@"
                    {{
                        ""strength"": {strength},
                        ""time"": {(int)(duration * 1000)}
                    }}";
            HttpContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            var response =  _client.PostAsync(url, content);
            if (_currentTask != null && _currentTask.IsCompleted)
            {
                Logger.Log(_currentTask.Result.Content?.ReadAsStringAsync()?.Result);
                _currentTask = null;
            }

            if (_currentTask == null)
                _currentTask = response;
            else
            {
                /*
                _currentTask.ContinueWith(finished_task =>
                {
                    if (finished_task != null)
                        Logger.Log(finished_task.Result.Content?.ReadAsStringAsync()?.Result);
                    _currentTask = null;
                    _currentTask = response;
                }, TaskScheduler.FromCurrentSynchronizationContext());
                */
            }
            //Logger.Log(response.Content.ReadAsStringAsync().Result);
            //no wait.
        }
    }

    [HarmonyPatch(typeof(Dam_PlayerDamageLocal), nameof(Dam_PlayerDamageLocal.ReceiveSetHealth))]
    public static class OnInfectDamage
    {
        public static void Prefix(Dam_PlayerDamageLocal __instance, pSetHealthData data)
        {
            if (!__instance.Owner.IsLocallyOwned) return;
            var diff = (__instance.Health - data.health.Get(__instance.HealthMax)) * 4;
            if (diff > 0 && diff < 1) // Damage.
            {
                Logger.Log($"Received Damage222 {diff}");
                if (diff < 2) // Infection Damage
                    DGLabHttpRequest.Fire(2, 
                        __instance.Owner.PlayerData.healthRegenDelay);
                else
                    DGLabHttpRequest.Fire(diff * ConfigManager.StrengthMultiplier, 
                        diff * ConfigManager.TimeMultiplier);
            }
        }
    }

    [HarmonyPatch(typeof(Dam_PlayerDamageBase), nameof(Dam_PlayerDamageBase.OnIncomingDamage))]
    public static class OnLocalDamage
    {
        public static void Prefix(Dam_PlayerDamageBase __instance, float damage)
        {
            if (!__instance.Owner.IsLocallyOwned) return;
            var diff = damage * 4;
            if (diff > 0)
            {
                Logger.Log($"Received Damage333 {diff}");
                DGLabHttpRequest.Fire(diff * ConfigManager.StrengthMultiplier,
                    diff * ConfigManager.TimeMultiplier);
            }
        }
    }
}
