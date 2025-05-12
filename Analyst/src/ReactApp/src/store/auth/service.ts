import { Observable } from 'rxjs'
import { BaseApi } from '../../service/api/base-api'
import { ApiResponse } from '../../types/types'

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

export const AuthSevice = {
    login: (payload: LoginPayload): Observable<ApiResponse<AuthResponse>> =>
        BaseApi.post<AuthResponse, LoginPayload>('/User/login', payload),

    signUp: (payload: SignUpPayload): Observable<ApiResponse<AuthResponse>> =>
        BaseApi.post<AuthResponse, SignUpPayload>('/User/signup', payload),

    refreshToken: (
        payload: RefrreshTokenPayload
    ): Observable<ApiResponse<RefreshTokenResponse>> =>
        BaseApi.post<RefreshTokenResponse, RefrreshTokenPayload>(
            '/User/refresh-token',
            payload
        ),
    logout: (token: string): Observable<ApiResponse<void>> =>
        BaseApi.post<void, void>('/User/logout', null, token),
}
