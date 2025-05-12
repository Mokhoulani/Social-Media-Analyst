import { createAction } from '@reduxjs/toolkit'
import {
    AuthError,
    AuthResponse,
    LoginPayload,
    RefrreshTokenPayload,
    SignUpPayload,
} from './service'

export const AuthActions = {
    loginRequest: createAction<LoginPayload>('auth/loginRequest'),
    loginSuccess: createAction<AuthResponse>('auth/loginSuccess'),
    loginFailure: createAction<AuthError>('auth/loginFailure'),

    signUpRequest: createAction<SignUpPayload>('auth/signUpRequest'),
    signUpSuccess: createAction<AuthResponse>('auth/signUpSuccess'),
    signUpFailure: createAction<AuthError>('auth/signUpFailure'),

    refreshTokenRequest: createAction<RefrreshTokenPayload>(
        'auth/refreshTokenRequest'
    ),
    refreshTokenSuccess: createAction<AuthResponse>('auth/refreshTokenSuccess'),
    refreshTokenFailure: createAction<AuthError>('auth/refreshTokenFailure'),

    logout: createAction<{ token: string }>('auth/logout'),
    logoutComplete: createAction('auth/logoutComplete'),
    logoutError: createAction<AuthError>('auth/logoutError'),
}

export type AuthActionTypes =
    | ReturnType<typeof AuthActions.loginRequest>
    | ReturnType<typeof AuthActions.loginSuccess>
    | ReturnType<typeof AuthActions.loginFailure>
    | ReturnType<typeof AuthActions.signUpRequest>
    | ReturnType<typeof AuthActions.signUpSuccess>
    | ReturnType<typeof AuthActions.signUpFailure>
    | ReturnType<typeof AuthActions.refreshTokenRequest>
    | ReturnType<typeof AuthActions.refreshTokenSuccess>
    | ReturnType<typeof AuthActions.refreshTokenFailure>
    | ReturnType<typeof AuthActions.logout>
    | ReturnType<typeof AuthActions.logoutComplete>
    | ReturnType<typeof AuthActions.logoutError>
