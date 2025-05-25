import * as SecureStore from 'expo-secure-store'
import { BehaviorSubject, from, Observable, of, throwError } from 'rxjs'
import { catchError, map } from 'rxjs/operators'

// Token storage keys
const TOKEN_KEY = 'token'
const REFRESH_TOKEN_KEY = 'refreshToken'
const EXPIRATION_KEY = 'expiration'

// BehaviorSubjects to track authentication state
const tokenSubject = new BehaviorSubject<string | null>(null)
const refreshTokenSubject = new BehaviorSubject<string | null>(null)
const expirationSubject = new BehaviorSubject<number | null>(null)

// Observables to subscribe to token changes
export const token$ = tokenSubject.asObservable()
export const refreshToken$ = refreshTokenSubject.asObservable()
export const isAuthenticated$ = token$.pipe(map((token) => !!token))
export const expiration$ = expirationSubject.asObservable()

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

export const getStoredExpiration = async (): Promise<number | null> => {
    try {
        const expiration = await SecureStore.getItemAsync(EXPIRATION_KEY)
        if (expiration) {
            expirationSubject.next(parseInt(expiration, 10))
        }
        return expiration ? parseInt(expiration, 10) : null
    } catch (error) {
        console.error('Error retrieving expiration:', error)
        return null
    }
}

export const getStoredExpiration$ = (): Observable<number | null> => {
    return from(getStoredExpiration()).pipe(
        catchError((err) => {
            console.error('Error in getStoredExpiration$:', err)
            return throwError(() => new Error('Failed to retrieve expiration'))
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
    refreshToken: string,
    expiration: number
): Promise<void> => {
    try {
        await SecureStore.setItemAsync(TOKEN_KEY, token)
        await SecureStore.setItemAsync(REFRESH_TOKEN_KEY, refreshToken)
        await SecureStore.setItemAsync(EXPIRATION_KEY, expiration.toString())

        // Update subjects with new values
        tokenSubject.next(token)
        refreshTokenSubject.next(refreshToken)
        expirationSubject.next(expiration)
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
    refreshToken: string,
    expiration: number
): Observable<void> => {
    return from(storeToken(token, refreshToken, expiration)).pipe(
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
        await SecureStore.deleteItemAsync(EXPIRATION_KEY)

        // Update subjects to reflect cleared state
        tokenSubject.next(null)
        refreshTokenSubject.next(null)
        expirationSubject.next(null)
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

export const isTokenExpired = async (): Promise<boolean> => {
    const exp = await SecureStore.getItemAsync(EXPIRATION_KEY)
    if (!exp) return true

    const expiration = parseInt(exp, 10)
    return Date.now() >= expiration
}

export const isTokenExpired$ = (): Observable<boolean> => {
    return from(isTokenExpired()).pipe(
        catchError((err) => {
            console.error('Error in isTokenExpired$:', err)
            return of(true) // Assume expired if we can't determine
        })
    )
}

export const getTokenTimeLeft = async (): Promise<number> => {
    const exp = await SecureStore.getItemAsync(EXPIRATION_KEY)
    if (!exp) return 0

    const expiration = parseInt(exp, 10)
    const timeLeft = expiration - Date.now()
    return Math.max(timeLeft, 0) // never return negative
}

export const getTokenTimeLeft$ = (): Observable<number> => {
    return from(getTokenTimeLeft()).pipe(
        catchError((err) => {
            console.error('Error in getTokenTimeLeft$:', err)
            return of(0)
        })
    )
}
