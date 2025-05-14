import { Observable } from 'rxjs'
import { BaseApi } from '../../service/api/base-api'

export interface LoginPayload {
    email: string
    password: string
}

export interface SignUpPayload {
    email: string
    password: string
}

export interface AuthResponse {
    accessToken: string
    refreshToken: string
}

export interface RefrreshTokenPayload {
    refreshToken: string
}
export interface RefreshTokenResponse {
    accessToken: string
    refreshToken: string
}

export interface LoginPayload {
    email: string
    password: string
}

export interface TokenResponse {
    accessToken: string
    refreshToken: string
}

export interface AuthState {
    accessToken: string | null
    refreshToken: string | null
    isAuthenticated: boolean
    loading: boolean
    error: string | null
}
export interface SignUpPayload {
    firstName: string
    lastName: string
    email: string
    password: string
}
export interface LoginPayload {
    email: string
    password: string
}

export interface SignUpPayload {
    firstName: string
    lastName: string
    email: string
    password: string
}

export interface TokenResponse {
    accessToken: string
    refreshToken: string
}

export interface AuthError {
    message: string
}

type RefreshTokenPayload = string

export const AuthService = {
    login: (payload: LoginPayload): Observable<AuthResponse> =>
        BaseApi.post<AuthResponse>('/User/login', payload),

    signUp: (payload: SignUpPayload): Observable<AuthResponse> =>
        BaseApi.post<AuthResponse>('/User/signup', payload),

    refreshToken: (
        refreshToken: RefreshTokenPayload
    ): Observable<RefreshTokenResponse> =>
        BaseApi.post<RefreshTokenResponse>('/auth/refresh-token', {
            refreshToken,
        }),

    logout: (token: string): Observable<void> =>
        BaseApi.post<void>('/User/logout', undefined, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        }),
}
