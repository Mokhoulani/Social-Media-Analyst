// error.utils.ts

import { AxiosError } from 'axios'
import {
    ApiError,
    ValidationError,
    UnauthorizedError,
    ForbiddenError,
    NotFoundError,
    ServerError,
    NetworkError,
    UnknownError,
} from './error.types'

export function parseApiError(error: AxiosError): ApiError {
    if (!error.response) {
        const networkError: NetworkError = {
            type: 'NetworkError',
            title: 'Network Error',
            status: 0,
            detail: error.message || 'Cannot connect to the server',
        }
        return networkError
    }

    const status = error.response.status
    const data = error.response.data

    switch (status) {
        case 400:
            return {
                type: 'ValidationError',
                title: 'Validation Error',
                status,
                detail: 'One or more validation errors occurred.',
                errors: (data as any).errors ?? {},
            } satisfies ValidationError

        case 401:
            return {
                type: 'UnauthorizedError',
                title: 'Unauthorized',
                status,
                detail: data?.detail ?? 'You are not authorized.',
            } satisfies UnauthorizedError

        case 403:
            return {
                type: 'ForbiddenError',
                title: 'Forbidden',
                status,
                detail: data?.detail ?? 'Access denied.',
            } satisfies ForbiddenError

        case 404:
            return {
                type: 'NotFoundError',
                title: 'Not Found',
                status,
                detail: data?.detail ?? 'The resource was not found.',
            } satisfies NotFoundError

        case 500:
            return {
                type: 'ServerError',
                title: 'Server Error',
                status,
                detail: data?.detail ?? 'An internal server error occurred.',
            } satisfies ServerError

        default:
            return {
                type: 'UnknownError',
                title: 'Unknown Error',
                status,
                detail: data?.detail ?? error.message ?? 'An unexpected error occurred.',
            } satisfies UnknownError
    }
}
