using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BR.Utils.Notification
{
    public class RestService
    {
        private HttpClient _httpClient;
        private readonly string _serviceAddress = "http://localhost:52978/api/notifications";
        public RestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetPushRegistrationId()
        {
            string registrationId = string.Empty;
            HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(_serviceAddress, "/register"));
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                registrationId = JsonConvert.DeserializeObject<string>(jsonResponse);
            }

            return registrationId;
        }

        public async Task<bool> UnregisterFromNotifications(string registrationId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(string.Concat(_serviceAddress, $"/unregister/{registrationId}"));
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> EnablePushNotifications(string id, DeviceRegistration deviceUpdate)
        {
            string registrationId = string.Empty;
            var content = new StringContent(JsonConvert.SerializeObject(deviceUpdate), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(string.Concat(_serviceAddress, $"/enable/{id}"), content);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> SendNotification(NotificationBody newNotification)
        {
            var content = new StringContent(JsonConvert.SerializeObject(newNotification), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(string.Concat(_serviceAddress, "/send"), content);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
