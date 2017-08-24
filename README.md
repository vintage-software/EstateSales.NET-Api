# EstateSales.NET-Api
* All api requests must set the host to: `https://www.estatesales.net`.
* All authenticated api requests must include the following header: `X_XSRF: X_XSRF`.
* Refresh tokens (api keys) will last indefinetely until revoked by the owner.
* Refresh tokens will not authenticate a user. Instead they must be exchanged for an access token. They can be used repeatedly.
* Access tokens will last 30 minutes, at which point the refresh token must be used again to obtain a new access token.

## Exchange *Refresh Token* for *Access Token*:

```
POST /token  
Host: https://www.estatesales.net  
Content-Type: application/x-www-form-urlencoded
```

```
grant_type=refresh_token&refresh_token={*refreshToken*}
```

## Sample Api Call

```
POST /api/sales  
Host: https://www.estatesales.net  
X_XSRF: X_XSRF  
Authorization: Bearer {*accessToken*}   
Content-Type: application/json
```

```
{
  "title": "My New Sale"
}
```
