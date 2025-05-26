import axios, {
    AxiosError,
    AxiosHeaders,
    AxiosResponse,
    InternalAxiosRequestConfig,
} from 'axios'
import { firstValueFrom, from, Observable, throwError, timer } from 'rxjs'
import { catchError, delay, map, switchMap, tap } from 'rxjs/operators'
import { getStoredToken$ } from '../utils/jwt-utils'
import { RefreshTokenRequest } from './refreshTokenRequest'

type EnhancedConfig = InternalAxiosRequestConfig<unknown> & {
    skipAuth?: boolean
    retryCount?: number
    retryDelay?: number
    retryStatusCodes?: number[]
    _retry?: boolean
}

type RequestInterceptor = (config: EnhancedConfig) => EnhancedConfig
type ResponseInterceptor = (response: unknown) => unknown

export class APIClient {
    private static axiosInstance = axios.create({
        baseURL: process.env.API_URL || 'http://localhost:5031/api',
    })

    private static requestInterceptors: RequestInterceptor[] = []
    private static responseInterceptors: ResponseInterceptor[] = []

    private static defaultRetryCount = 2
    private static defaultRetryDelay = 1000
    private static defaultRetryStatusCodes = [408, 429, 500, 502, 503, 504]

    public static setup(): void {
        this.axiosInstance.interceptors.request.use(
            async (config) => {
                const enhancedConfig = config as EnhancedConfig

                if (!enhancedConfig.skipAuth) {
                    try {
                        const token = await firstValueFrom(getStoredToken$())
                        if (token) {
                            const headers = new AxiosHeaders(
                                enhancedConfig.headers
                            )
                            headers.set('Authorization', `Bearer ${token}`)
                            enhancedConfig.headers = headers
                        }
                    } catch (error) {
                        console.warn('Error getting token', error)
                    }
                }

                return this.applyRequestInterceptors(enhancedConfig)
            },
            (error) => Promise.reject(error)
        )

        // Response interceptor
        this.axiosInstance.interceptors.response.use(
            (response) => {
                response.data = this.applyResponseInterceptors(response.data)
                return response
            },
            (error) => Promise.reject(error)
        )
    }

    static addRequestInterceptor(interceptor: RequestInterceptor) {
        this.requestInterceptors.push(interceptor)
    }

    static addResponseInterceptor(interceptor: ResponseInterceptor) {
        this.responseInterceptors.push(interceptor)
    }

    private static applyRequestInterceptors(
        config: EnhancedConfig
    ): EnhancedConfig {
        return this.requestInterceptors.reduce(
            (acc, interceptor) => interceptor(acc),
            config
        )
    }

    private static applyResponseInterceptors(response: unknown): unknown {
        return this.responseInterceptors.reduce(
            (acc, interceptor) => interceptor(acc),
            response
        )
    }

    static get<T>(url: string, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({
            ...config,
            method: 'get',
            url,
            headers: config?.headers ?? new AxiosHeaders(),
        })
    }

    static post<T, D = unknown>(
        url: string,
        data?: D,
        config?: EnhancedConfig
    ): Observable<T> {
        return this.request<T>({
            ...config,
            method: 'post',
            url,
            data,
            headers: config?.headers ?? new AxiosHeaders(),
        })
    }

    static patch<T, D = unknown>(
        url: string,
        data?: D,
        config?: EnhancedConfig
    ): Observable<T> {
        return this.request<T>({
            ...config,
            method: 'patch',
            url,
            data,
            headers: config?.headers ?? new AxiosHeaders(),
        })
    }

    static delete<T>(url: string, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({
            ...config,
            method: 'delete',
            url,
            headers: config?.headers ?? new AxiosHeaders(),
        })
    }

    static put<T, D = unknown>(
        url: string,
        data?: D,
        config?: EnhancedConfig
    ): Observable<T> {
        return this.request<T>({
            ...config,
            method: 'put',
            url,
            data,
            headers: config?.headers ?? new AxiosHeaders(),
        })
    }

    private static request<T>(config: EnhancedConfig): Observable<T> {
        const retryCount = config.retryCount ?? this.defaultRetryCount
        const retryDelay = config.retryDelay ?? this.defaultRetryDelay
        const retryStatusCodes =
            config.retryStatusCodes ?? this.defaultRetryStatusCodes

        let attempts = 0

        const makeRequest = (): Observable<T> => {
            return from(this.axiosInstance.request<T>(config)).pipe(
                map((res: AxiosResponse<T>) => res.data),
                catchError((error: AxiosError) => {
                    const status = error.response?.status

                    if (status === 401 && !config._retry) {
                        console.log('Token expired, attempting refresh...')
                        config._retry = true

                        const tokenRefresher = new RefreshTokenRequest()

                        return tokenRefresher.refreshToken().pipe(
                            tap((newToken) => {
                                APIClient.axiosInstance.defaults.headers.common[
                                    'Authorization'
                                ] = `Bearer ${newToken}`
                                if (config.headers) {
                                    config.headers['Authorization'] =
                                        `Bearer ${newToken}`
                                }
                            }),
                            delay(300),
                            switchMap(() => makeRequest()),
                            catchError((refreshError) => {
                                console.error(
                                    'Token refresh failed:',
                                    refreshError
                                )
                                return throwError(() => this.handleError(error))
                            })
                        )
                    }

                    const shouldRetry =
                        !status || retryStatusCodes.includes(status)

                    if (attempts >= retryCount || !shouldRetry) {
                        console.error(
                            'Retry limit reached or non-retryable status.'
                        )
                        return throwError(() => this.handleError(error))
                    }

                    attempts++
                    const backoffDelay = retryDelay * Math.pow(2, attempts)
                    console.warn(
                        `Retrying request in ${backoffDelay}ms... (Attempt ${attempts}/${retryCount})`
                    )

                    return timer(backoffDelay).pipe(
                        switchMap(() => makeRequest())
                    )
                })
            )
        }

        return makeRequest()
    }

    private static handleError(error: AxiosError) {
        if (!error) {
            return {
                status: 500,
                message: 'Network Error',
                details: 'Please check your internet connection.',
            }
        }
        const status = error.response?.status
        const data = error.response?.data

        return {
            status,
            message: error.message || 'Request failed',
            details:
                typeof data === 'object' && data !== null
                    ? data
                    : { raw: data },
        }
    }
}

export default APIClient
APIClient.setup()
