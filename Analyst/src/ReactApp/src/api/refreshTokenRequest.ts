import axios from 'axios'
import { BehaviorSubject, from, Observable, throwError } from 'rxjs'
import {
    catchError,
    filter,
    finalize,
    map,
    switchMap,
    take,
    tap,
} from 'rxjs/operators'
import { RefreshTokenResponse } from '../types/types'
import {
    clearTokens,
    getStoredRefreshToken$,
    storeToken$,
} from '../utils/jwt-utils'

export class RefreshTokenRequest {
    private static refreshAxios = axios.create({
        baseURL: 'http://localhost:5031/api/auth',
        headers: { 'Content-Type': 'application/json' },
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

        return getStoredRefreshToken$().pipe(
            switchMap((refreshToken) => {
                if (!refreshToken) {
                    RefreshTokenRequest.refreshInProgress$.next(null)
                    return throwError(
                        () => new Error('No refresh token available')
                    )
                }

                return from(
                    RefreshTokenRequest.refreshAxios.post<RefreshTokenResponse>(
                        '/refresh-token',
                        { refreshToken }
                    )
                ).pipe(
                    map((response) => response.data),
                    switchMap((response) => {
                        const expiration = Date.now() + 30 * 60 * 1000
                        return storeToken$(
                            response.accessToken,
                            response.refreshToken,
                            expiration
                        ).pipe(
                            tap(() =>
                                RefreshTokenRequest.refreshInProgress$.next(
                                    response.accessToken
                                )
                            ),
                            map(() => response.accessToken)
                        )
                    }),
                    catchError((error) => {
                        clearTokens()
                        RefreshTokenRequest.refreshInProgress$.next(null)
                        return throwError(
                            () =>
                                new Error(
                                    'Refresh token failed: ' + error.message
                                )
                        )
                    }),
                    finalize(() => {
                        // The BehaviorSubject is not reset here on purpose
                    })
                )
            })
        )
    }
}
