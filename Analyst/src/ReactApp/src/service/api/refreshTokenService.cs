import { of, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import axios from 'axios'; // Assuming you are using axios for making API requests.

private static refreshTokenService(refreshToken: string): Observable<string> {
    // Replace this with an actual API call to refresh the token
    const refreshUrl = '/api/auth/refresh-token'; // Example API endpoint
    return axios
        .post(refreshUrl, { refreshToken })
        .then(response =>
         {
             const newAccessToken = response.data.accessToken; // Assume the new token is returned in response.data
             if (newAccessToken)
             {
                 return of(newAccessToken); // Return the new access token as an observable
             }
             return throwError('Failed to refresh token'); // Handle if no token is returned
         })
        .catch(error =>
        {
            // Handle API call error (e.g., network issues or server errors)
            return throwError(error.response?.data?.message || 'Failed to refresh token');
        });
}
