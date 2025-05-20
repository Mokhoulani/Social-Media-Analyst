import { clearTokens, storeToken$ } from '../../utils/jwt-utils'

/**
 * Auth Effects handle side effects related to authentication
 * These are primarily token storage functions
 */
export const AuthEffects = {
    /**
     * Handle successful login by storing tokens in localStorage
     *
     * @param accessToken JWT token for API access
     * @param refreshToken Token used to refresh the access token
     */
    handleLoginSuccess: (accessToken: string, refreshToken: string) => {
        if (!accessToken || !refreshToken) {
            console.error('Invalid tokens received during login')
            return
        }

        storeToken$(accessToken, refreshToken).subscribe({
            next: () => console.log('Tokens stored successfully'),
            error: (err) => console.error('Failed to store tokens:', err),
        })
    },
    /**
     * Handle logout by removing tokens from localStorage
     */
    handleLogout: () => {
        clearTokens()
    },

    /**
     * Handle token refresh by updating stored tokens
     *
     * @param accessToken New JWT token for API access
     * @param refreshToken New token used to refresh the access token
     */
    handleTokenRefresh: (accessToken: string, refreshToken: string) => {
        if (!accessToken || !refreshToken) {
            console.error('Invalid tokens received during refresh')
            return
        }
        storeToken$(accessToken, refreshToken).subscribe({
            next: () => console.log('Tokens stored successfully'),
            error: (err) => console.error('Failed to store tokens:', err),
        })
    },
}
