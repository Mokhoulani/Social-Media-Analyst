import * as SecureStore from 'expo-secure-store'
import { BehaviorSubject, from, Observable, throwError } from 'rxjs'
import { catchError, map } from 'rxjs/operators'

// Token storage keys
const TOKEN_KEY = 'token'
const REFRESH_TOKEN_KEY = 'refreshToken'

// BehaviorSubjects to track authentication state
const tokenSubject = new BehaviorSubject<string | null>(null)
const refreshTokenSubject = new BehaviorSubject<string | null>(null)

// Observables to subscribe to token changes
export const token$ = tokenSubject.asObservable()
export const refreshToken$ = refreshTokenSubject.asObservable()
export const isAuthenticated$ = token$.pipe(map((token) => !!token))

/**
 * Retrieves the stored authentication token
 * @returns Promise resolving to the authentication token or null
 */
export const getStoredToken = async (): Promise<string | null> => {
    try {
        const token = await SecureStore.getItemAsync(TOKEN_KEY)
        if (token) {
            tokenSubject.next(token)
        }
        return token
    } catch (error) {
        console.error('Error retrieving token:', error)
        return null
    }
}

/**
 * Retrieves the stored token as an Observable
 * @returns Observable of the authentication token
 */
export const getStoredToken$ = (): Observable<string | null> => {
    return from(getStoredToken()).pipe(
        catchError((err) => {
            console.error('Error in getStoredToken$:', err)
            return throwError(() => new Error('Failed to retrieve token'))
        })
    )
}

/**
 * Retrieves the stored refresh token
 * @returns Promise resolving to the refresh token or null
 */
export const getStoredRefreshToken = async (): Promise<string | null> => {
    try {
        const refreshToken = await SecureStore.getItemAsync(REFRESH_TOKEN_KEY)
        if (refreshToken) {
            refreshTokenSubject.next(refreshToken)
        }
        return refreshToken
    } catch (error) {
        console.error('Error retrieving refresh token:', error)
        return null
    }
}

/**
 * Retrieves the stored refresh token as an Observable
 * @returns Observable of the refresh token
 */
export const getStoredRefreshToken$ = (): Observable<string | null> => {
    return from(getStoredRefreshToken()).pipe(
        catchError((err) => {
            console.error('Error in getStoredRefreshToken$:', err)
            return throwError(
                () => new Error('Failed to retrieve refresh token')
            )
        })
    )
}

/**
 * Stores authentication and refresh tokens securely
 * @param token The authentication token to store
 * @param refreshToken The refresh token to store
 * @returns Promise that resolves when tokens are stored
 */
export const storeToken = async (
    token: string,
    refreshToken: string
): Promise<void> => {
    try {
        await SecureStore.setItemAsync(TOKEN_KEY, token)
        await SecureStore.setItemAsync(REFRESH_TOKEN_KEY, refreshToken)

        // Update subjects with new values
        tokenSubject.next(token)
        refreshTokenSubject.next(refreshToken)
    } catch (error) {
        console.error('Error storing tokens:', error)
        throw error
    }
}

/**
 * Stores tokens as an Observable operation
 * @param token The authentication token to store
 * @param refreshToken The refresh token to store
 * @returns Observable that completes when tokens are stored
 */
export const storeToken$ = (
    token: string,
    refreshToken: string
): Observable<void> => {
    return from(storeToken(token, refreshToken)).pipe(
        catchError((err) => {
            console.error('Error in storeToken$:', err)
            return throwError(() => new Error('Failed to store tokens'))
        })
    )
}

/**
 * Clears all stored tokens
 * @returns Promise that resolves when tokens are cleared
 */
export const clearTokens = async (): Promise<void> => {
    try {
        await SecureStore.deleteItemAsync(TOKEN_KEY)
        await SecureStore.deleteItemAsync(REFRESH_TOKEN_KEY)

        // Update subjects to reflect cleared state
        tokenSubject.next(null)
        refreshTokenSubject.next(null)
    } catch (error) {
        console.error('Error clearing tokens:', error)
        throw error
    }
}

/**
 * Clears tokens as an Observable operation
 * @returns Observable that completes when tokens are cleared
 */
export const clearTokens$ = (): Observable<void> => {
    return from(clearTokens()).pipe(
        catchError((err) => {
            console.error('Error in clearTokens$:', err)
            return throwError(() => new Error('Failed to clear tokens'))
        })
    )
}
