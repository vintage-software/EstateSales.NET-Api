using EstateSalesNetPublicApi.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EstateSalesNetPublicApi
{
    public class Client
    {
        private readonly string apiKey;
        private readonly RestClient restClient;

        private string accessToken;
        private DateTime? accessTokenGoodUntil;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// This client allows you to easily communicate with the EstateSales.NET public API. This client is not setup with
        /// all of the API requests available, but it does have several related to creating sales. Please be aware that this
        /// is released as a BETA release and could break at ANY time. We will try to notify API users of potential breaking
        /// changes, but can't guarantee that we will.
        /// </summary>
        /// <param name="apiBaseUrl">The base URL will almost always be https://www.estatesales.net</param>
        /// <param name="apiKey">The key will be one that you generate inside your account on EstateSales.NET. You can do that here: https://www.estatesales.net/account/org/api-keys </param>
        public Client(string apiKey, string apiBaseUrl = "https://www.estatesales.net")
        {
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ArgumentNullException(nameof(apiBaseUrl));
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (apiBaseUrl.EndsWith("/"))
            {
                apiBaseUrl = apiBaseUrl.Substring(0, apiBaseUrl.Length - 1);
            }

            this.apiKey = apiKey;
            this.restClient = new RestClient(apiBaseUrl);
        }

        /// <summary>
        /// GetActiveSales will return the list of sales available to edit for the account number provided.
        /// </summary>
        /// <param name="orgId">The orgId is your account number on EstateSales.NET</param>
        /// <returns>This returns a list of sale objects</returns>
        public IReadOnlyCollection<Sale> GetEditableSales(int orgId)
        {
            RestRequest saleRequest = this.CreateRestRequest($"/api/public-sales/org/{orgId}", Method.GET);

            IRestResponse<List<Sale>> returnSale = this.restClient.Get<List<Sale>>(saleRequest);
            return returnSale.Data.AsReadOnly();
        }

        /// <summary>
        /// Get a sale based on the saleId. The sale must be one that is owned by the org authorized by the apikey.
        /// </summary>
        /// <param name="saleId">Id of the sale to be retrieved</param>
        public Sale GetSale(int saleId)
        {
            RestRequest saleRequest = this.CreateRestRequest($"/api/public-sales/{saleId}", Method.GET);

            IRestResponse<Sale> returnSale = this.restClient.Get<Sale>(saleRequest);
            return returnSale.Data;
        }

        /// <summary>
        /// Create a sale based on the properties on the sale object.
        /// </summary>
        public Sale CreateSale(Sale sale)
        {
            RestRequest saleRequest = this.CreateRestRequest("/api/public-sales/", Method.POST, sale);
            IRestResponse<Sale> returnSale = this.restClient.Post<Sale>(saleRequest);
            return returnSale.Data;
        }

        /// <summary>
        /// Create a sale date. There are many rules for sale dates. One to be aware of is that if
        /// the sale type is an online auction, the sale should only have one sale date with a start
        /// and end date that spans multiple days. If it is any other type of sale, each sale date
        /// should be for the same day but different times.
        /// </summary>
        public SaleDate CreateSaleDate(SaleDate saleDate)
        {
            RestRequest dateRequest = this.CreateRestRequest("/api/sale-dates/", Method.POST, saleDate);

            IRestResponse<SaleDate> returnSaleDate = this.restClient.Post<SaleDate>(dateRequest);
            return returnSaleDate.Data;
        }

        /// <summary>
        /// Create a sale picture. The following formats are supported: JPG, PNG, and GIF. Set the
        /// byte array on the salePicture object to upload the pic.
        /// </summary>
        public SalePicture CreateSalePicture(SalePicture salePicture)
        {
            RestRequest pictureRequest = this.CreateRestRequest("/api/sale-pictures/", Method.POST, salePicture);

            IRestResponse<SalePicture> returnSalePicture = this.restClient.Post<SalePicture>(pictureRequest);
            return returnSalePicture.Data;
        }

        /// <summary>
        /// A sale can be updated after it is created by sending through the entire sale object again
        /// with the changed information. The orgId is the one exception. It cannot be changed once it is set.
        /// </summary>
        public Sale UpdateSale(Sale sale)
        {
            if (sale == null)
            {
                throw new ArgumentNullException(nameof(sale));
            }

            if (sale.Id <= 0)
            {
                throw new Exception("Sale object must contain an Id in order to update");
            }

            RestRequest saleRequest = this.CreateRestRequest($"/api/public-sales/{sale.Id}", Method.PUT, sale);
            saleRequest.AddJsonBody(sale);

            IRestResponse<Sale> response = this.restClient.Put<Sale>(saleRequest);
            return response.Data;
        }

        /// <summary>
        /// This call will publish the sale onto the EstateSales.NET website for public viewing.
        /// </summary>
        /// <param name="saleId">The Id of the sale to be published</param>
        /// <param name="autoPayAnyBalance">If this is set to false and there is a balance on the sale, it will
        /// not be published. If it is set to true, the balance will be paid with the organization's credit card
        /// on file. If the payment fails, the sale will not be published.</param>
        public Sale PublishSale(int saleId, bool autoPayAnyBalance)
        {
            RestRequest publishRequest = this.CreateRestRequest($"/api/public-sales/{saleId}/publish/{autoPayAnyBalance}", Method.POST);

            IRestResponse<Sale> response = this.restClient.Post<Sale>(publishRequest);
            return response.Data;
        }

        /// <summary>
        /// This call will unpublish a sale. It is still technically viewable by the public if they have
        /// a direct link to the sale. A notice shows on the sale saying that it has been unpublished.
        /// </summary>
        public Sale UnpublishSale(int saleId)
        {
            RestRequest unpublishRequest = this.CreateRestRequest($"/api/public-sales/{saleId}/unpublish", Method.POST);

            IRestResponse<Sale> response = this.restClient.Post<Sale>(unpublishRequest);
            return response.Data;
        }

        /// <summary>
        /// If the sale needs to be made unavailable completely, it can be deleted. You can restore a sale by
        /// logging into EstateSales.NET and viewing the past sales report.
        /// </summary>
        /// <param name="saleId">The id of the sale to be deleted</param>
        public HttpStatusCode DeleteSale(int saleId)
        {
            RestRequest saleRequest = this.CreateRestRequest($"/api/public-sales/{saleId}", Method.DELETE);

            IRestResponse response = this.restClient.Delete(saleRequest);
            return response.StatusCode;
        }

        /// <summary>
        /// This will delete a sale date from a sale. If it is the last sale date on a published sale, the sale
        /// will become unpublished.
        /// </summary>
        public HttpStatusCode DeleteSaleDate(int dateId)
        {
            RestRequest dateRequest = this.CreateRestRequest($"/api/sale-dates/{dateId}", Method.DELETE);

            IRestResponse response = this.restClient.Delete(dateRequest);
            return response.StatusCode;
        }

        /// <summary>
        /// This will remove a picture from a sale. The url to the picture does usually still work. It is not
        /// guaranteed to be deleted or kept.
        /// </summary>
        public HttpStatusCode DeletePicture(int pictureId)
        {
            RestRequest pictureRequest = this.CreateRestRequest($"/api/sale-pictures/{pictureId}", Method.DELETE);

            IRestResponse response = this.restClient.Delete(pictureRequest);
            return response.StatusCode;
        }

        private RestRequest CreateRestRequest(string url, Method method, object jsonObject = null)
        {
            if (this.NeedNewAccessToken())
            {
                this.SetNewAccessToken();
            }

            RestRequest request = new RestRequest(url, method);
            request.AddHeader("Authorization", $"Bearer {this.accessToken}");
            request.AddHeader("X_XSRF", "X_XSRF");

            if (jsonObject != null)
            {
                request.AddJsonBody(jsonObject);
            }

            return request;
        }

        private bool NeedNewAccessToken()
        {
            return string.IsNullOrEmpty(this.accessToken) || this.accessTokenGoodUntil.HasValue == false || this.accessTokenGoodUntil.Value < DateTime.UtcNow.AddMinutes(-1);
        }

        private void SetNewAccessToken()
        {
            RestRequest tokenRequest = new RestRequest("/token", Method.POST);
            tokenRequest.AddParameter("grant_type", "refresh_token");
            tokenRequest.AddParameter("refresh_token", this.apiKey);

            IRestResponse<AccessTokenResponse> response = this.restClient.Post<AccessTokenResponse>(tokenRequest);

            this.accessToken = response.Data.AccessToken;
            this.accessTokenGoodUntil = DateTime.UtcNow.AddSeconds(response.Data.ExpiresIn);
        }
    }
}
