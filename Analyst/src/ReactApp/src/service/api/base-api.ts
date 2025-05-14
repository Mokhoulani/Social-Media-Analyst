import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios'
import { Observable, from, throwError } from 'rxjs'
import { catchError, map, retry } from 'rxjs/operators'

type RequestInterceptor = (config: AxiosRequestConfig) => AxiosRequestConfig
type ResponseInterceptor = (response: AxiosResponse) => AxiosResponse

export class BaseApi {
    private static axios = axios.create({
        baseURL: 'http://localhost:5031/api',
    })
    private static requestInterceptors: RequestInterceptor[] = []
    private static responseInterceptors: ResponseInterceptor[] = []

    // Add request interceptor
    static addRequestInterceptor(interceptor: RequestInterceptor) {
        this.requestInterceptors.push(interceptor)
    }

    // Add response interceptor
    static addResponseInterceptor(interceptor: ResponseInterceptor, p0?: (error: any) => Promise<never>) {
        this.responseInterceptors.push(interceptor)
    }

    static get<T = any>(
        url: string,
        config?: AxiosRequestConfig
    ): Observable<T> {
        return this.request<T>({ ...config, method: 'GET', url })
    }

    static post<T = any>(
        url: string,
        data?: any,
        config?: AxiosRequestConfig
    ): Observable<T> {
        return this.request<T>({ ...config, method: 'POST', url, data })
    }

    // ... other HTTP methods (put, delete, etc)

    private static applyRequestInterceptors(
        config: AxiosRequestConfig
    ): AxiosRequestConfig {
        return this.requestInterceptors.reduce(
            (acc, interceptor) => interceptor(acc),
            config
        )
    }

    private static applyResponseInterceptors(
        response: AxiosResponse
    ): AxiosResponse {
        return this.responseInterceptors.reduce(
            (acc, interceptor) => interceptor(acc),
            response
        )
    }

    private static request<T = any>(config: AxiosRequestConfig): Observable<T> {
        const interceptedConfig = this.applyRequestInterceptors(config)

        return from(this.axios.request<T>(interceptedConfig)).pipe(
            map((response: AxiosResponse<T>) => {
                const processedResponse =
                    this.applyResponseInterceptors(response)
                return processedResponse.data
            }),
            catchError((error: AxiosError) =>
                throwError(() => this.handleError(error))
            ),
            retry({ count: 2, delay: 1000 })
        )
    }

    private static handleError(error: AxiosError) {
        if (!error.response) {
            return {
                status: 0,
                message: 'Network Error',
                details: error.message || 'Cannot connect to server',
            }
        }

        return {
            status: error.response.status,
            message: error.response.statusText,
            details: error.response.data,
        }
    }
}

export default BaseApi
