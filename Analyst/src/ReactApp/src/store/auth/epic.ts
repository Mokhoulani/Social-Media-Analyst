import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap, tap } from 'rxjs/operators'
import { Dependencies, RootState } from '../store'
import { AuthActions } from './actions'
import { AuthEffects } from './effects'
import { AuthSevice } from './service'

export const loginEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(AuthActions.loginRequest.type),
        mergeMap((action) =>
            AuthSevice.login(
                (action as ReturnType<typeof AuthActions.loginRequest>).payload
            ).pipe(
                map((response) => {
                    if (
                        'accessToken' in response &&
                        'refreshToken' in response
                    ) {
                        return AuthActions.loginSuccess({
                            accessToken: response.accessToken!,
                            refreshToken: response.refreshToken!,
                        })
                    }
                    return AuthActions.loginFailure({
                        message: 'Invalid response',
                    })
                }),
                tap((action) => {
                    if (action.type === AuthActions.loginSuccess.type) {
                        const successAction = action as ReturnType<
                            typeof AuthActions.loginSuccess
                        >
                        AuthEffects.handleLoginSuccess(
                            successAction.payload.accessToken,
                            successAction.payload.refreshToken
                        )
                    }
                }),
                catchError((error) =>
                    of(
                        AuthActions.loginFailure({
                            message: error.message || 'Unknown error',
                        })
                    )
                )
            )
        )
    )

export const signUpEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(AuthActions.signUpRequest.type),
        mergeMap((action) =>
            AuthSevice.signUp(
                (action as ReturnType<typeof AuthActions.signUpRequest>).payload
            ).pipe(
                map((response) => {
                    if (
                        'accessToken' in response &&
                        'refreshToken' in response
                    ) {
                        return AuthActions.signUpSuccess({
                            accessToken: response.accessToken!,
                            refreshToken: response.refreshToken!,
                        })
                    }
                    return AuthActions.signUpFailure({
                        message: 'Invalid response',
                    })
                }),
                tap((action) => {
                    if (action.type === AuthActions.signUpSuccess.type) {
                        const successAction = action as ReturnType<
                            typeof AuthActions.signUpSuccess
                        >
                        AuthEffects.handleLoginSuccess(
                            successAction.payload.accessToken,
                            successAction.payload.refreshToken
                        )
                    }
                }),
                catchError((error) =>
                    of(
                        AuthActions.signUpFailure({
                            message: error.message || 'Unknown error',
                        })
                    )
                )
            )
        )
    )

export const refreshTokenEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(AuthActions.refreshTokenRequest.type),
        mergeMap((action) =>
            AuthSevice.refreshToken(
                (action as ReturnType<typeof AuthActions.refreshTokenRequest>)
                    .payload
            ).pipe(
                map((response) => {
                    if (
                        'accessToken' in response &&
                        'refreshToken' in response
                    ) {
                        return AuthActions.refreshTokenSuccess({
                            accessToken: response.accessToken!,
                            refreshToken: response.refreshToken!,
                        })
                    }
                    return AuthActions.refreshTokenFailure({
                        message: 'Invalid response',
                    })
                }),
                tap((action) => {
                    if (action.type === AuthActions.refreshTokenSuccess.type) {
                        const successAction = action as ReturnType<
                            typeof AuthActions.refreshTokenSuccess
                        >
                        AuthEffects.handleLoginSuccess(
                            successAction.payload.accessToken,
                            successAction.payload.refreshToken
                        )
                    }
                }),
                catchError((error) =>
                    of(
                        AuthActions.refreshTokenFailure({
                            message: error.message || 'Unknown error',
                        })
                    )
                )
            )
        )
    )

export const logoutEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(AuthActions.logout.type),
        tap(() => {
            AuthEffects.handleLogout()
        }),
        mergeMap((action) => {
            // Only call API if a token is provided
            const logoutAction = action as ReturnType<typeof AuthActions.logout>
            if (logoutAction.payload?.token) {
                return AuthSevice.logout(logoutAction.payload.token).pipe(
                    map(() => {
                        // Just complete the action, the reducer will handle state reset
                        return AuthActions.logoutComplete()
                    }),
                    catchError((error) =>
                        of(
                            AuthActions.logoutError({
                                message: error.message || 'Logout failed',
                            })
                        )
                    )
                )
            }
            return of(AuthActions.logoutComplete())
        })
    )

export const authEpics: Epic<Action, Action, RootState, Dependencies>[] = [
    loginEpic,
    signUpEpic,
    refreshTokenEpic,
    logoutEpic,
]
