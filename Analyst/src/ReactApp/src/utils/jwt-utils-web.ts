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

// Helpers
const safeParseInt = (value: string | null): number | null =>
    value ? parseInt(value, 10) : null

// Token Getters
export const getStoredToken = async (): Promise<string | null> => {
    try {
        const token = localStorage.getItem(TOKEN_KEY)
        if (token) tokenSubject.next(token)
        return token
    } catch (error) {
        console.error('Error retrieving token:', error)
        return null
    }
}

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
        const expiration = safeParseInt(localStorage.getItem(EXPIRATION_KEY))
        if (expiration) expirationSubject.next(expiration)
        return expiration
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

export const getStoredRefreshToken = async (): Promise<string | null> => {
    try {
        const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)
        if (refreshToken) refreshTokenSubject.next(refreshToken)
        return refreshToken
    } catch (error) {
        console.error('Error retrieving refresh token:', error)
        return null
    }
}

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

// Store
export const storeToken = async (
    token: string,
    refreshToken: string,
    expiration: number
): Promise<void> => {
    try {
        localStorage.setItem(TOKEN_KEY, token)
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken)
        localStorage.setItem(EXPIRATION_KEY, expiration.toString())

        tokenSubject.next(token)
        refreshTokenSubject.next(refreshToken)
        expirationSubject.next(expiration)
    } catch (error) {
        console.error('Error storing tokens:', error)
        throw error
    }
}

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

// Clear
export const clearTokens = async (): Promise<void> => {
    try {
        localStorage.removeItem(TOKEN_KEY)
        localStorage.removeItem(REFRESH_TOKEN_KEY)
        localStorage.removeItem(EXPIRATION_KEY)

        tokenSubject.next(null)
        refreshTokenSubject.next(null)
        expirationSubject.next(null)
    } catch (error) {
        console.error('Error clearing tokens:', error)
        throw error
    }
}

export const clearTokens$ = (): Observable<void> => {
    return from(clearTokens()).pipe(
        catchError((err) => {
            console.error('Error in clearTokens$:', err)
            return throwError(() => new Error('Failed to clear tokens'))
        })
    )
}

// Expiration helpers
export const isTokenExpired = async (): Promise<boolean> => {
    const exp = safeParseInt(localStorage.getItem(EXPIRATION_KEY))
    if (!exp) return true
    return Date.now() >= exp
}

export const isTokenExpired$ = (): Observable<boolean> => {
    return from(isTokenExpired()).pipe(
        catchError((err) => {
            console.error('Error in isTokenExpired$:', err)
            return of(true)
        })
    )
}

export const getTokenTimeLeft = async (): Promise<number> => {
    const exp = safeParseInt(localStorage.getItem(EXPIRATION_KEY))
    if (!exp) return 0
    return Math.max(exp - Date.now(), 0)
}

export const getTokenTimeLeft$ = (): Observable<number> => {
    return from(getTokenTimeLeft()).pipe(
        catchError((err) => {
            console.error('Error in getTokenTimeLeft$:', err)
            return of(0)
        })
    )
}
