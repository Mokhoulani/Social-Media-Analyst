export interface ErrorDetail {
    code: string
    message: string
}

export interface ErrorResponse {
    type: string
    title: string
    status: number
    detail: string
    errors: ErrorDetail[]
}

export interface ValidationError extends ErrorResponse {
    type: 'ValidationError'
}

export interface UnauthorizedError extends ErrorResponse {
    type: 'UnauthorizedError'
}

export interface ForbiddenError extends ErrorResponse {
    type: 'ForbiddenError'
}

export interface SuccessResponse<T> {
    accessToken?: string
    refreshToken?: string
    data?: T
}

export type ApiResponse<T> =
    | SuccessResponse<T>
    | ValidationError
    | UnauthorizedError
    | ForbiddenError
    | ErrorResponse
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
