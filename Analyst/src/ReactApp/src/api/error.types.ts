// error.types.ts

export interface BaseError {
    type: string
    title: string
    status: number
    detail: string
}

export interface ValidationError extends BaseError {
    type: 'ValidationError'
    errors: { [key: string]: string[] }
}

export interface UnauthorizedError extends BaseError {
    type: 'UnauthorizedError'
}

export interface ForbiddenError extends BaseError {
    type: 'ForbiddenError'
}

export interface NotFoundError extends BaseError {
    type: 'NotFoundError'
}

export interface ServerError extends BaseError {
    type: 'ServerError'
}

export interface NetworkError extends BaseError {
    type: 'NetworkError'
}

export interface UnknownError extends BaseError {
    type: 'UnknownError'
}

// Union of all possible API errors
export type ApiError =
    | ValidationError
    | UnauthorizedError
    | ForbiddenError
    | NotFoundError
    | ServerError
    | NetworkError
    | UnknownError
