using JuniorCodeTest.Interfaces;
using JuniorCodeTest.Models;

using System.Text.Json;

namespace JuniorCodeTest.Services
{
	public class RandomUserApiService(HttpClient httpClient) : IRandomUserApiService
	{
		private const string randomUserEndPoint = "https://randomuser.me/api";

		public async Task<List<RequestedUsersModel>> GetRandomUserDataFromApi(int userAmount)
		{
			var requiredDataList = new List<RequestedUsersModel>();
			string extendedUserEndPoint = randomUserEndPoint + "/?results=" + userAmount;
			var response = await httpClient.GetAsync(extendedUserEndPoint);

			if (response.IsSuccessStatusCode)
			{
				string responseData = await response.Content.ReadAsStringAsync();
				var randomUserModelData = JsonSerializer.Deserialize<UserModel>(responseData);

				if (randomUserModelData != null)
				{

					foreach (var randomUser in randomUserModelData.results)
					{
						var requiredData = new RequestedUsersModel()
						{
							Age = randomUser.dob.age,
							First = randomUser.name.first,
							Last = randomUser.name.last,
							Title = randomUser.name.title,
							Country = randomUser.location.country,
							Coordinates = randomUser.location.coordinates
						};

						if (requiredDataList.Count >= userAmount)
						{
							break;
						}

						requiredDataList.Add(requiredData);
					}

				}

				return requiredDataList;

			}
			else
			{
				throw new Exception("Failed to fetch data from the random user API");
			}
		}
	}
}
