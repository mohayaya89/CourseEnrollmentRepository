namespace CourseEnrollment.Web.Blazor.Auth;

/// <summary>
/// Outgoing HTTP handler that attaches the Bearer token to every request.
/// On a 401 response it attempts a single silent token refresh, then retries the original request.
/// If the refresh also fails it gives up and returns the 401 to the caller.
/// </summary>
public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly AccessTokenStore _store;
    private readonly AuthService _authService;

    public JwtAuthorizationMessageHandler(AccessTokenStore store, AuthService authService)
    {
        _store = store;
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        AttachToken(request);
        var response = await base.SendAsync(request, ct);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshed = await _authService.TryRefreshAsync();
            if (refreshed && request.RequestUri is not null)
            {
                // Clone the request (it can't be sent twice as-is)
                var retryRequest = new HttpRequestMessage(request.Method, request.RequestUri);
                if (request.Content is not null)
                {
                    var content = await request.Content.ReadAsByteArrayAsync(ct);
                    retryRequest.Content = new ByteArrayContent(content);
                    foreach (var header in request.Content.Headers)
                        retryRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                foreach (var header in request.Headers)
                    retryRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);

                AttachToken(retryRequest);
                response = await base.SendAsync(retryRequest, ct);
            }
        }

        return response;
    }

    private void AttachToken(HttpRequestMessage request)
    {
        if (_store.HasToken && _store.Token is not null)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _store.Token);
    }
}
