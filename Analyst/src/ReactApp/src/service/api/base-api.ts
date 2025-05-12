import { catchError, map, Observable, retry, throwError } from 'rxjs'
import { ajax, AjaxError, AjaxResponse } from 'rxjs/ajax'
import {
    ApiResponse,
    ErrorResponse,
    SuccessResponse,
    UnauthorizedError,
    ValidationError,
} from '../../types/types'

export class BaseApi {
    private static apiUrl = process.env.API_URL || 'http://localhost:5031/api'
    private static defaultTimeout = 30000 // 30 seconds

    /**
     * Creates headers object with content type and optional auth token
     */
    private static getHeaders(token?: string): Record<string, string> {
        const headers: Record<string, string> = {
            'Content-Type': 'application/json',
        }

        if (token) {
            headers['Authorization'] = `Bearer ${token}`
        }

        return headers
    }

    /**
     * Shared method for making HTTP requests
     */
    private static request<T>(
        method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE',
        endpoint: string,
        token?: string,
        data?: unknown,
        params?: Record<string, string>
    ): Observable<ApiResponse<T>> {
        let url = `${this.apiUrl}${endpoint}`

        if (params && method === 'GET') {
            const query = new URLSearchParams(params).toString()
            if (query) {
                url += `?${query}`
            }
        }

        return ajax({
            url,
            method,
            headers: this.getHeaders(token),
            body: data,
            timeout: this.defaultTimeout,
        }).pipe(
            // You can tweak retry count or move it to GET-only if needed
            method === 'GET' ? retry(1) : map((res) => res),
            map((res) => this.handleResponse<T>(this.castResponse<T>(res))),
            catchError((err) => this.handleError(err))
        )
    }

    /**
     * GET request
     */
    public static get<T>(
        endpoint: string,
        token?: string,
        params?: Record<string, string>
    ): Observable<ApiResponse<T>> {
        return this.request<T>('GET', endpoint, token, null, params)
    }

    /**
     * POST request
     */
    public static post<T, D>(
        endpoint: string,
        data: D | null,
        token?: string
    ): Observable<ApiResponse<T>> {
        return this.request<T>('POST', endpoint, token, data)
    }

    /**
     * PUT request
     */
    public static put<T, D>(
        endpoint: string,
        data: D,
        token?: string
    ): Observable<ApiResponse<T>> {
        return this.request<T>('PUT', endpoint, token, data)
    }

    /**
     * PATCH request
     */
    public static patch<T, D>(
        endpoint: string,
        data: D,
        token?: string
    ): Observable<ApiResponse<T>> {
        return this.request<T>('PATCH', endpoint, token, data)
    }

    /**
     * DELETE request
     */
    public static delete<T>(
        endpoint: string,
        token?: string
    ): Observable<ApiResponse<T>> {
        return this.request<T>('DELETE', endpoint, token)
    }

    /**
     * Safely cast AjaxResponse from unknown
     */
    private static castResponse<T>(
        response: AjaxResponse<unknown>
    ): AjaxResponse<SuccessResponse<T>> {
        return response as AjaxResponse<SuccessResponse<T>>
    }

    /**
     * Handle successful API responses
     */
    private static handleResponse<T>(
        response: AjaxResponse<SuccessResponse<T>>
    ): SuccessResponse<T> {
        return {
            accessToken: response.response?.accessToken,
            refreshToken: response.response?.refreshToken,
            data: response.response?.data as T,
        }
    }

    /**
     * Handle API errors based on status code
     */
    private static handleError(error: AjaxError): Observable<never> {
        if (process.env.NODE_ENV !== 'production') {
            console.error('API Error:', error)
        }

        if (error.status === 400) {
            return throwError(() => error.response as ValidationError)
        }

        if (error.status === 401) {
            return throwError(() => error.response as UnauthorizedError)
        }

        if (error.status === 404) {
            return throwError(
                () =>
                    ({
                        type: 'NotFoundError',
                        title: 'Resource Not Found',
                        status: 404,
                        detail: 'The requested resource could not be found',
                        errors: [],
                    }) as ErrorResponse
            )
        }

        if (error.status === 403) {
            return throwError(
                () =>
                    ({
                        type: 'ForbiddenError',
                        title: 'Access Forbidden',
                        status: 403,
                        detail: 'You do not have permission to access this resource',
                        errors: [],
                    }) as ErrorResponse
            )
        }

        if (error.status >= 500) {
            return throwError(
                () =>
                    ({
                        type: 'ServerError',
                        title: 'Server Error',
                        status: error.status,
                        detail: 'An error occurred on the server',
                        errors: [],
                    }) as ErrorResponse
            )
        }

        return throwError(
            () =>
                ({
                    type: 'UnknownError',
                    title: 'Unknown Error',
                    status: error.status || 0,
                    detail: error.message || 'An unknown error occurred',
                    errors: [],
                }) as ErrorResponse
        )
    }
}
