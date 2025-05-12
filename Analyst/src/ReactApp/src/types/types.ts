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
