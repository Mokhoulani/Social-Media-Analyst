import axios from 'axios'
import {
    BehaviorSubject,
    catchError,
    filter,
    finalize,
    map,
    Observable,
    shareReplay,
    take,
    tap,
    throwError,
} from 'rxjs'
import { RefreshTokenResponse } from '../types/types'
import {
    clearTokens,
    getStoredRefreshToken,
    storeToken,
} from '../utils/jwt-utils'

export class RefreshTokenRequest {
    private static refreshAxios = axios.create({
        baseURL: 'http://localhost:5031/api/auth',
        headers: {
            'Content-Type': 'application/json',
        },
    })

    private static refreshInProgress$: BehaviorSubject<string | null> =
        new BehaviorSubject<string | null>(null)

    public refreshToken(): Observable<string> {
        const current = RefreshTokenRequest.refreshInProgress$.value

        if (current !== null) {
            return RefreshTokenRequest.refreshInProgress$.pipe(
                filter((token) => token !== null),
                take(1)
            ) as Observable<string>
        }

        RefreshTokenRequest.refreshInProgress$.next('pending')

        const refreshToken = getStoredRefreshToken()
        if (!refreshToken) {
            RefreshTokenRequest.refreshInProgress$.next(null)
            return throwError(() => new Error('No refresh token available'))
        }

        return new Observable<RefreshTokenResponse>((observer) => {
            RefreshTokenRequest.refreshAxios
                .post<RefreshTokenResponse>('/refresh-token', {
                    refreshToken,
                })
                .then((response) => {
                    observer.next(response.data)
                    observer.complete()
                })
                .catch((error) => observer.error(error))
        }).pipe(
            tap((response) => {
                const { accessToken, refreshToken } = response
                storeToken(accessToken, refreshToken)
                RefreshTokenRequest.refreshInProgress$.next(accessToken)
            }),
            map((response) => response.accessToken),
            catchError((error) => {
                clearTokens()
                RefreshTokenRequest.refreshInProgress$.next(null)
                return throwError(
                    () => new Error('Refresh token failed: ' + error.message)
                )
            }),
            finalize(() => {
                // Don't reset the subject here - it contains the new token
                // Just let it be reset on the next refresh attempt
            }),
            shareReplay(1)
        )
    }
}
